using System;
using System.Collections.Generic;
using System.Text;

using System.Xml;

namespace RCIS.Utility
{
    public class LS_XMLHelper
    {
        #region Create an xml file
        /// <summary>
        /// 创建一个XML文档
        /// </summary>
        /// <param name="xmlDoc">自定义的XmlCocument</param>
        /// <param name="sXMLPath">创建文档要保存的路径和名称</param>
        public static void CreateXMLFile(XmlDocument xmlDoc, string sXMLPath)
        {
            XmlDeclaration xmlDec = xmlDoc.CreateXmlDeclaration("1.0", "GB2312", null);
            xmlDoc.AppendChild(xmlDec);

            XmlElement xmlElement = xmlDoc.CreateElement("lsAppConfig");
            xmlDoc.AppendChild(xmlElement);

            xmlDoc.Save(sXMLPath);
        }

        /// <summary>
        /// 创建ls配置文件的connection节点
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="xmlFilePathName"></param>
        /// <param name="sSection"></param>
        /// <param name="sSection2"></param>
        public static void CreateXMLNode(XmlDocument xmlDoc, string xmlFilePathName,
            string sSection, string sSection2)
        {
            XmlNode root = xmlDoc.SelectSingleNode(sSection);
            XmlElement xmlElement = xmlDoc.CreateElement(sSection2);
            xmlElement.SetAttribute("name", "edit");
            xmlElement.SetAttribute("password", "edit");
            xmlElement.SetAttribute("class", "");
            xmlElement.SetAttribute("inited", "true");

            root.AppendChild(xmlElement);

            xmlDoc.Save(xmlFilePathName);
        }
        /// <summary>
        /// 插入新的节点
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="xmlFilePathName"></param>
        /// <param name="sSection"></param>
        /// <param name="sSection2"></param>
        /// <param name="sKey"></param>
        /// <param name="sDefaultValue"></param>
        public static void CreateXMLNode(XmlDocument xmlDoc, string xmlFilePathName,
            string sSection, string sSection2, string sKey, string sDefaultValue)
        {
            XmlNode root = xmlDoc.SelectSingleNode(sSection);

            XmlNodeList xmlNodelist = root.ChildNodes;

            for (int i = 0; i < xmlNodelist.Count; i++)
            {
                XmlNode xmlChildNode = xmlNodelist[i];
                if (xmlChildNode.LocalName == sSection2)
                {
                    XmlElement xmlEle = xmlDoc.CreateElement(sKey);
                    xmlEle.InnerText = sDefaultValue;
                    xmlChildNode.AppendChild(xmlEle);

                    root.AppendChild(xmlChildNode);
                }
            }

            xmlDoc.Save(xmlFilePathName);
        }

        #endregion

        #region 读取XML文件指定
        /// <summary>
        /// 读取XML文件
        /// </summary>
        /// <param name="Section">部分</param>
        /// <param name="Key">关键字</param>
        /// <returns>返回字符串</returns>
        public static string XmlReadValue(XmlDocument doc, string Section, string Key)
        {
            try
            {
                XmlNode XmlCodeNode = doc.SelectSingleNode("//" + Section);

                string ss = "";
                if (null != XmlCodeNode)
                {
                    ss = XmlCodeNode.SelectSingleNode(Key).InnerText;
                    if (ss == "") ss = "空";
                }
                return ss;
            }
            catch { return ""; }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="Section"></param>
        /// <param name="Section1"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
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
                            XmlNodeList childchildNodeList = xmlchildnode.ChildNodes;
                            for (int j = 0; j < childchildNodeList.Count; j++)
                            {
                                XmlNode xmlchildchildnode = childchildNodeList[j];
                                if (xmlchildchildnode.LocalName.Equals(Section2))
                                {
                                    XmlNodeList childchildchildNodeList = xmlchildchildnode.ChildNodes;
                                    for (int k = 0; k < childchildchildNodeList.Count; k++)
                                    {
                                        XmlNode xmlchildchildchildnode = childchildchildNodeList[k];
                                        if (xmlchildchildchildnode.LocalName.Equals(Section3))
                                        {
                                            ss = xmlchildchildchildnode.SelectSingleNode(Key).InnerText;
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
        #endregion

        #region //写XML文件
        /// <summary>
        /// 写入XML文件
        /// </summary>
        /// <param name="doc">xml文档名</param>
        /// <param name="sXMLPath">文档路径</param>
        /// <param name="Section">节点</param>
        /// <param name="Key">键值</param>
        /// <param name="Value">值</param>
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
                        XmlNodeList childchildNodeList = xmlchildnode.ChildNodes;
                        for (int j = 0; j < childchildNodeList.Count; j++)
                        {
                            XmlNode xmlchildchildnode = childchildNodeList[j];
                            if (xmlchildchildnode.LocalName.Equals(sValue2))
                            {
                                XmlNodeList childchildchildNodeList = xmlchildchildnode.ChildNodes;
                                for (int k = 0; k < childchildchildNodeList.Count; k++)
                                {
                                    XmlNode xmlchildchildchildnode = childchildchildNodeList[k];
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
        #endregion

        #region 获取属性值
        public static String GetAttribute(XmlNode pNode, String pAttrName, String pDefResult)
        {
            String result = pDefResult;
            try
            {
                XmlAttribute xmlAttr = pNode.Attributes[pAttrName];
                if (xmlAttr != null)
                {
                    result = xmlAttr.Value;
                }
            }
            catch (Exception ex)
            {
                result = pDefResult;
            }
            return result;
        }

        public static double GetAttribute(XmlNode pNode, String pAttrName, double pDefResult)
        {
            double result = pDefResult;
            String resultStr = GetAttribute(pNode, pAttrName, pDefResult.ToString());
            if (!double.TryParse(resultStr, System.Globalization.NumberStyles.Any
                , new System.Globalization.NumberFormatInfo(), out result))
            {
                result = pDefResult;
            }
            return result;
        }

        public static int GetAttribute(XmlNode pNode, String pAttrName, int pDefResult)
        {
            int result = pDefResult;
            String resultStr = GetAttribute(pNode, pAttrName, pDefResult.ToString());
            try
            {
                result = Convert.ToInt32(resultStr);
            }
            catch (Exception ex)
            {
                result = pDefResult;
            }
            return result;
        }

        public static bool GetAttribute(XmlNode pNode, String pAttrName, bool pDefResult)
        {
            bool result = pDefResult;
            String resultStr = GetAttribute(pNode, pAttrName, pDefResult.ToString());
            try
            {
                result = Convert.ToBoolean(resultStr);
            }
            catch (Exception ex)
            {
                result = pDefResult;
            }
            return result;
        }
        #endregion
    }
}
