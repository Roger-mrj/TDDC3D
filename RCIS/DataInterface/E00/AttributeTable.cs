using ESRI.ArcGIS.Geodatabase;

using System;
using System.Collections;
namespace RCIS.DataExchange.E00
{
/// <summary>
/// TableStruct 的摘要说明。
/// </summary>
public class AttributeTable
{
private String m_tableName;
private int m_fieldCount;
private int m_blockSize;
private int m_recordCount;
private ArrayList m_recordList;
private ArrayList m_fieldList;
public AttributeTable(String pTableName
,int pFieldCount
,int pBlockSize
,int pRecordCount)
{
this.m_tableName =pTableName;
this.m_fieldCount =pFieldCount;
this.m_blockSize =pBlockSize;
this.m_recordCount =pRecordCount;
this.m_recordList =new ArrayList ();
this.m_fieldList =new ArrayList ();
}
public String TableName
{
get
{
return this.m_tableName ;
}
}
public int FieldCount
{
get
{
return this.m_fieldCount ;
}
}
public int BlockSize
{
get
{
return this.m_blockSize ;
}
}
#region 数据管理
public int RecordCount
{
get
{
return this.m_recordCount ;
}
}

public void AppendRecord(ArrayList pRecord)
{
this.m_recordList.Add (pRecord);
}
public int GetRecordCount()
{
return this.m_recordList .Count ;
}
public ArrayList GetRecord(int pRecordIndex)
{
return this.m_recordList [pRecordIndex] as ArrayList;
}
#endregion
#region 字段管理
public int GetFieldCount()
{
return this.m_fieldList .Count ;
}
public TableField GetTableField(int pFieldIndex)
{
return this.m_fieldList [pFieldIndex] as TableField;
}
public void AppendField(TableField pField)
{
this.m_fieldList .Add (pField);
}
#endregion
}
}
