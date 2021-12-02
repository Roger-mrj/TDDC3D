using System;
using ESRI.ArcGIS.Geodatabase;
namespace RCIS.DataExchange.E00
{
	/// <summary>
	/// TableField 的摘要说明。
	/// </summary>
	public class TableField
	{
        private String m_fieldName;
        esriFieldType  m_fieldType;
        
        private int    m_fieldLength;
        private int    m_fieldScale;
		public TableField(String pFieldName,esriFieldType pFT
            ,int pFieldLength,int pFieldScale)
		{
            this.m_fieldName=pFieldName;
            this.m_fieldType =pFT;            
            this.m_fieldLength =pFieldLength;
            this.m_fieldScale =pFieldScale;
		}
        public String FieldName
        {
            get
            {
                return this.m_fieldName ;
            }
        }
        public esriFieldType FieldType
        {
            get
            {
                return this.m_fieldType;
            }
        }
        public int FieldLength
        {
            get
            {
                return this.m_fieldLength;
            }
        }
        public int FieldScale
        {
            get
            {
                return this.m_fieldScale;
            }
        }
	}
}
