using System;
using System.Collections.Generic;
using ESRI.ArcGIS.Geodatabase;
using System.Data;
using System.Data.OleDb;
namespace RuleCheck
{
    public class TableSchemaRead
    {        

        public static List<string> GetRelationDms(string confMdbFile, string tableName)
        {
            List<string> dms = new List<string>();
            RCIS.Database.AccdbOperateHelper dbhelper = new RCIS.Database.AccdbOperateHelper(confMdbFile);
            try
            {
                DataTable dt = dbhelper.GetDatatable("select DM from " + tableName);
                foreach (DataRow dr in dt.Rows)
                {
                    dms.Add(dr["DM"].ToString());
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                dbhelper.Close();
            }

            return dms;
            //IDbString dbstr = new AccessDbString(confMdbFile);
            //using (AccessConnectionManager conn = new AccessConnectionManager((AccessDbString)dbstr))
            //{
            //    try
            //    {
            //        DataSet ds = conn.Fill("select DM from " + tableName);
            //        foreach (DataRow dr in ds.Tables[0].Rows)
            //        {
            //            dms.Add(dr["DM"].ToString());
            //        }
            //    }
            //    catch { }
            //}
            //return dms;


        }



        //获取一个表的所有字段
        public static Dictionary<string, ATabField> GetColumnByLayer(IFeatureWorkspace feaWorkspace, string layerName)
        {
            Dictionary<string, ATabField> aDicFields = new Dictionary<string, ATabField>();
            
            if (feaWorkspace != null)
            {
                try
                {
                    //IFeatureClass aClass = feaWorkspace.OpenFeatureClass(layerName);
                    ITable aClass = feaWorkspace.OpenTable(layerName);

                    IFields pFields = aClass.Fields;
                    for (int i = 0; i < pFields.FieldCount; i++)
                    {

                        IField pField = pFields.get_Field(i);
                        string fieldName = pField.Name.ToUpper();
                        if (fieldName.Contains("SHAPE") || fieldName.Contains("SHP") || fieldName.Contains("OBJECTID"))
                            continue;
                        ATabField aFieldObj = new ATabField();
                        aFieldObj.fieldIsMust = !pField.IsNullable;
                        aFieldObj.fieldLength = pField.Length;
                        aFieldObj.fieldName = fieldName;
                        aFieldObj.fieldPrecision = pField.Precision;
                        string fieldType = "";
                        switch (pField.Type)
                        {
                            case esriFieldType.esriFieldTypeDate:
                                fieldType = "date";
                                break;
                            case esriFieldType.esriFieldTypeDouble:
                                fieldType = "float";
                                break;
                            case esriFieldType.esriFieldTypeInteger:
                                fieldType = "int";
                                break;
                            case esriFieldType.esriFieldTypeSingle:
                                fieldType = "float";
                                break;
                            case esriFieldType.esriFieldTypeSmallInteger:
                                fieldType = "int";
                                break;
                            case esriFieldType.esriFieldTypeString:
                                fieldType = "char";
                                break;
                            default:
                                fieldType = "char";
                                break;
                        }
                        aFieldObj.fieldType = fieldType;

                        aDicFields.Add(fieldName, aFieldObj);

                    }

                }
                catch (Exception ex)
                {
                }

            }

            return aDicFields;
        }


        public static List<StandTabSchemaObj> GetAllSpatialTab(string confMdb)
        {
            List<StandTabSchemaObj> lst = new List<StandTabSchemaObj>();
            RCIS.Database.AccdbOperateHelper conn = new RCIS.Database.AccdbOperateHelper(confMdb);
            try
            {
                string sql = "select distinct TabName from CHK_LAYER ";
                DataTable table = conn.GetDatatable(sql);
                foreach (DataRow dr in table.Rows)
                {
                    string aTabName = dr["TabName"].ToString();
                    List<ATabField> lstFields = new List<ATabField>();

                    DataTable dtFields = conn.GetDatatable("select * from CHK_LAYER where TabName='" + aTabName + "' ");
                    foreach (DataRow drField in dtFields.Rows)
                    {
                        try
                        {
                            ATabField aField = new ATabField();
                            aField.fieldClause = drField["fieldClause"].ToString();
                            aField.fieldClause2 = drField["fieldClause2"].ToString();
                            string sfldMust = drField["fieldIsMust"].ToString();
                            bool.TryParse(sfldMust, out aField.fieldIsMust);

                            string sfldLen = drField["fieldLeng"].ToString();
                            int.TryParse(sfldLen, out aField.fieldLength);

                            aField.fieldName = drField["fileldName"].ToString();
                            aField.fieldPrecision = drField["fieldPrecision"].ToString().Trim() == "" ? 0 : Convert.ToInt32(drField["fieldPrecision"].ToString());
                            aField.fieldType = drField["fileldType"].ToString();
                            aField.relationCode = drField["relationCode"].ToString();

                            lstFields.Add(aField);
                        }
                        catch (Exception ex)
                        {
                        }
                    }

                    StandTabSchemaObj obj = new StandTabSchemaObj();
                    obj.TabName = aTabName;
                    obj.allFields = lstFields;
                    lst.Add(obj);
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                conn.Close();
            }

            //IDbString dbstr = new AccessDbString(confMdb);
            //using (AccessConnectionManager conn = new AccessConnectionManager((AccessDbString)dbstr))
            //{
            //    try
            //    {
            //        string sql = "select distinct TabName from CHK_LAYER ";
            //        DataSet dsTab = conn.Fill(sql);
            //        foreach (DataRow dr in dsTab.Tables[0].Rows)
            //        {
            //            string aTabName = dr["TabName"].ToString();
            //            List<ATabField> lstFields = new List<ATabField>();
            //            DataSet dsFields = conn.Fill("select * from CHK_LAYER where TabName='" + aTabName + "' ");
            //            DataTable dtFields = dsFields.Tables[0];
            //            foreach (DataRow drField in dtFields.Rows)
            //            {
            //                try
            //                {
            //                    ATabField aField = new ATabField();
            //                    aField.fieldClause = drField["fieldClause"].ToString();
            //                    aField.fieldClause2 = drField["fieldClause2"].ToString();
            //                    string sfldMust = drField["fieldIsMust"].ToString();
            //                    bool.TryParse(sfldMust,out aField.fieldIsMust);

            //                    string sfldLen = drField["fieldLeng"].ToString();
            //                    int.TryParse(sfldLen, out aField.fieldLength);
                               
            //                    aField.fieldName = drField["fileldName"].ToString();
            //                    aField.fieldPrecision = drField["fieldPrecision"].ToString().Trim() == "" ? 0 : Convert.ToInt32(drField["fieldPrecision"].ToString());
            //                    aField.fieldType = drField["fileldType"].ToString();
            //                    aField.relationCode = drField["relationCode"].ToString();

            //                    lstFields.Add(aField);
            //                }
            //                catch (Exception ex)
            //                {
            //                }
            //            }

            //            StandTabSchemaObj obj = new StandTabSchemaObj();
            //            obj.TabName = aTabName;
            //            obj.allFields = lstFields;
            //            lst.Add(obj);
            //        }

            //    }
            //    catch (Exception ex)
            //    {
            //    }
            //}

            return lst;
        }



        //根据acess表获取表结构
        /// <summary>
        /// 根据acess表获取表结构
        /// </summary>
        /// <param name="mdbfile"></param>
        /// <param name="tabName">表名</param>
        /// <returns></returns>
        public static Dictionary<string, ATabField> GetColumnByAcess(string mdbfile, string tabName)
        {
            Dictionary<string, ATabField> aDicFields = new Dictionary<string, ATabField>();

            OleDbConnection conn = new OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mdbfile);
            conn.Open();
            DataTable dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new System.Object[] { null, null, tabName, null });
            foreach (DataRow dr in dt.Rows)
            {
                //获取字段信息
                ATabField aField = new ATabField();
                aField.fieldName = dr["Column_Name"].ToString().ToUpper();
                string sType = dr["Data_Type"].ToString();
                int length = 0;
                int scale = 0;
                if (sType == "131" || sType == "4" || sType == "5")
                {
                    sType = "float";

                    int.TryParse(dr["numeric_precision"].ToString(), out length);
                    int.TryParse(dr["numeric_scale"].ToString(), out scale);
                }
                else if (sType == "130")
                {
                    sType = "char";
                    int.TryParse(dr["Character_maximum_length"].ToString(), out length);
                }
                else if (sType == "3")
                {
                    sType = "int";
                    int.TryParse(dr["numeric_precision"].ToString(), out length);
                }
                aField.fieldType = sType;
                aField.fieldLength = length;
                aField.fieldPrecision = scale;
                aField.fieldIsMust = !Convert.ToBoolean(dr["Is_Nullable"].ToString());

                aDicFields.Add(aField.fieldName, aField);
            }
            conn.Close();
            return aDicFields;
        }

    }
}
