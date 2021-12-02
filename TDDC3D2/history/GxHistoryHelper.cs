using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using RCIS.GISCommon;
using RCIS.Utility;
using System.Collections;


namespace TDDC3D.edit
{
    public class GxHistoryHelper
    {

        public static bool CreateDLTBH(IFeatureDataset pDS)
        {
            IWorkspace pWS = pDS.Workspace;
            IWorkspace2 currWs2 = pWS as IWorkspace2;
            if (currWs2.get_NameExists(esriDatasetType.esriDTFeatureClass, "DLTB_H"))
                return true;
            
            IFeatureClass dltbClass = null;
            try
            {
                dltbClass = (pWS as IFeatureWorkspace).OpenFeatureClass("DLTB");
            }
            catch (Exception ex)
            {
            }
            if (dltbClass == null) return false;
            try
            {
                //实倒化字段集合对象
                IFields pFields = new FieldsClass();
                IFieldsEdit tFieldsEdit = (IFieldsEdit)pFields;

                //创建几何对象字段定义
                IGeometryDef tGeometryDef = new GeometryDefClass();
                IGeometryDefEdit tGeometryDefEdit = tGeometryDef as IGeometryDefEdit;
                //指定几何对象字段属性值

                tGeometryDefEdit.GeometryType_2 = dltbClass.ShapeType;
                tGeometryDefEdit.GridCount_2 = 1;
                tGeometryDefEdit.set_GridSize(0, 1000);
                tGeometryDefEdit.SpatialReference_2 = (dltbClass as IGeoDataset).SpatialReference;

                //创建OID字段
                IField fieldOID = new FieldClass();
                IFieldEdit fieldEditOID = fieldOID as IFieldEdit;
                fieldEditOID.Name_2 = "OBJECTID";
                fieldEditOID.AliasName_2 = "OBJECTID";
                fieldEditOID.Type_2 = esriFieldType.esriFieldTypeOID;
                tFieldsEdit.AddField(fieldOID);

                //创建几何字段
                IField fieldShape = new FieldClass();
                IFieldEdit fieldEditShape = fieldShape as IFieldEdit;
                fieldEditShape.Name_2 = "SHAPE";
                fieldEditShape.AliasName_2 = "SHAPE";
                fieldEditShape.Type_2 = esriFieldType.esriFieldTypeGeometry;
                fieldEditShape.GeometryDef_2 = tGeometryDef;
                tFieldsEdit.AddField(fieldShape);

                //添加其他字段
                for (int i = 0; i < dltbClass.Fields.FieldCount; i++)
                {
                    IField aFld = dltbClass.Fields.get_Field(i);
                    if (aFld.Type == esriFieldType.esriFieldTypeGUID || aFld.Type == esriFieldType.esriFieldTypeOID || aFld.Type == esriFieldType.esriFieldTypeOID
                        || aFld.Type == esriFieldType.esriFieldTypeGeometry
                        )
                    {
                        continue;
                    }
                    IField newFld = new FieldClass();
                    IFieldEdit newFldEdt = newFld as IFieldEdit;
                    newFldEdt.Name_2 = aFld.Name;
                    newFldEdt.AliasName_2 = aFld.AliasName;
                    newFldEdt.Type_2 = aFld.Type;
                    newFldEdt.Length_2 = aFld.Length;
                    newFldEdt.Precision_2 = aFld.Precision;
                    newFldEdt.Scale_2 = aFld.Scale;

                    tFieldsEdit.AddField(newFld);

                }
                //增加时间戳字段
                IField timeFld = new FieldClass();
                IFieldEdit timeFldEdt = timeFld as IFieldEdit;
                timeFldEdt.Name_2 = "GXSJ";
                timeFldEdt.AliasName_2 = "更新时间";
                timeFldEdt.Type_2 = esriFieldType.esriFieldTypeString;
                tFieldsEdit.AddField(timeFld);


                IFeatureClass tFeatureClass = pDS.CreateFeatureClass("DLTB_H",
                    pFields, null, null, esriFeatureType.esriFTSimple, "SHAPE", "");
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
           
        }


        /// <summary>
        /// 创建地类图斑历史和过程表 图层
        /// </summary>
        /// <param name="pws"></param>
        /// <returns></returns>
        public static bool CreateHGXTable(IFeatureDataset pDS)
        {

            bool bRet = CreateDLTBH(pDS);
            bRet &= CreateDLTBGxgc(pDS);
            return bRet;

        }

        
        /// <summary>
        /// 创建更新过程层
        /// </summary>
        /// <param name="pWS"></param>
        /// <returns></returns>
        public static bool CreateDLTBGxgc(IFeatureDataset pDS)
        {
            IWorkspace pWS = pDS.Workspace;
            IWorkspace2 currWs2 = pWS as IWorkspace2;
            if (currWs2.get_NameExists(esriDatasetType.esriDTFeatureClass, "DLTB_GXGC"))
                return true;
            
            IFeatureClass dltbClass = null;
            try
            {
                dltbClass = (pWS as IFeatureWorkspace).OpenFeatureClass("DLTB");
            }
            catch (Exception ex)
            {
            }
            if (dltbClass == null) return false;
            
            try
            {
                //实倒化字段集合对象
                IFields pFields = new FieldsClass();
                IFieldsEdit tFieldsEdit = (IFieldsEdit)pFields;

                //创建几何对象字段定义
                IGeometryDef tGeometryDef = new GeometryDefClass();
                IGeometryDefEdit tGeometryDefEdit = tGeometryDef as IGeometryDefEdit;
                //指定几何对象字段属性值

                tGeometryDefEdit.GeometryType_2 = dltbClass.ShapeType;
                tGeometryDefEdit.GridCount_2 = 1;
                tGeometryDefEdit.set_GridSize(0, 1000);
                tGeometryDefEdit.SpatialReference_2 = (dltbClass as IGeoDataset).SpatialReference;

                //创建OID字段
                IField fieldOID = new FieldClass();
                IFieldEdit fieldEditOID = fieldOID as IFieldEdit;
                fieldEditOID.Name_2 = "OBJECTID";
                fieldEditOID.AliasName_2 = "OBJECTID";
                fieldEditOID.Type_2 = esriFieldType.esriFieldTypeOID;
                tFieldsEdit.AddField(fieldOID);

                //创建几何字段
                IField fieldShape = new FieldClass();
                IFieldEdit fieldEditShape = fieldShape as IFieldEdit;
                fieldEditShape.Name_2 = "SHAPE";
                fieldEditShape.AliasName_2 = "SHAPE";
                fieldEditShape.Type_2 = esriFieldType.esriFieldTypeGeometry;
                fieldEditShape.GeometryDef_2 = tGeometryDef;
                tFieldsEdit.AddField(fieldShape);

                //添加其他字段
                for (int i = 0; i < dltbClass.Fields.FieldCount; i++)
                {
                    IField aFld = dltbClass.Fields.get_Field(i);
                    if (aFld.Type == esriFieldType.esriFieldTypeGUID || aFld.Type == esriFieldType.esriFieldTypeOID || aFld.Type == esriFieldType.esriFieldTypeOID
                        || aFld.Type == esriFieldType.esriFieldTypeGeometry
                        )
                    {
                        continue;
                    }
                    if (aFld.Name.ToUpper().StartsWith("SHAPE"))
                        continue;
                    
                    IField newFld = new FieldClass();
                    IFieldEdit newFldEdt = newFld as IFieldEdit;
                    newFldEdt.Name_2 = aFld.Name;                    
                    newFldEdt.AliasName_2 = aFld.AliasName;
                    newFldEdt.Type_2 = aFld.Type;
                    newFldEdt.Length_2 = aFld.Length;
                    newFldEdt.Precision_2 = aFld.Precision;
                    newFldEdt.Scale_2 = aFld.Scale;

                    tFieldsEdit.AddField(newFld);

                    if ((aFld.Name.ToUpper()!="YSDM") )
                    {
                        newFld = new FieldClass();
                        newFldEdt = newFld as IFieldEdit;
                        newFldEdt.Name_2 = "BGH" + aFld.Name.ToUpper();
                        newFldEdt.AliasName_2 ="变更后"+ aFld.AliasName;
                        newFldEdt.Type_2 = aFld.Type;
                        newFldEdt.Length_2 = aFld.Length;
                        newFldEdt.Precision_2 = aFld.Precision;
                        newFldEdt.Scale_2 = aFld.Scale;
                        tFieldsEdit.AddField(newFld);
                    }

                }
                //增加时间戳字段
                IField timeFld = new FieldClass();
                IFieldEdit timeFldEdt = timeFld as IFieldEdit;
                timeFldEdt.Name_2 = "GXSJ";
                timeFldEdt.AliasName_2 = "更新时间";
                timeFldEdt.Type_2 = esriFieldType.esriFieldTypeString;
                tFieldsEdit.AddField(timeFld);

                

                IFeatureClass tFeatureClass = pDS.CreateFeatureClass("DLTB_GXGC",
                    pFields, null, null, esriFeatureType.esriFTSimple, "SHAPE", "");
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
           

        }

        //将 要素插入到 历史图层中
        public static void InsertHistory(IWorkspace pws,List<IFeature> lst)
        {            
            IFeatureClass dltbHClass = null;
            try
            {               
                dltbHClass = (pws as IFeatureWorkspace).OpenFeatureClass("DLTB_H");
            }
            catch (Exception ex)
            {
            }
            if ( dltbHClass == null) return;
            DateTime dt = DateTime.Now;
            IFeatureCursor insertCursor = dltbHClass.Insert(true);
            try
            {
                foreach (IFeature aFea in lst)
                {
                    IFeatureBuffer buffer = dltbHClass.CreateFeatureBuffer();
                    buffer.Shape = aFea.ShapeCopy;
                    int fldCount = dltbHClass.Fields.FieldCount;
                    for (int fi = 0; fi < fldCount; fi++)
                    {
                        IField aSrcFld = dltbHClass.Fields.get_Field(fi);
                        #region 判断源字段是否需要处理
                        if (aSrcFld.Type == esriFieldType.esriFieldTypeOID
                            || aSrcFld.Type == esriFieldType.esriFieldTypeGeometry
                            || aSrcFld == dltbHClass.LengthField
                            || aSrcFld == dltbHClass.AreaField)
                        {
                            continue;
                        }

                        #endregion
                        string aSrcFldName = aSrcFld.Name.ToUpper();
                        int srcIdx = aFea.Fields.FindField(aSrcFldName);
                        if (srcIdx > -1)
                        {
                            try
                            {
                                buffer.set_Value(fi, aFea.get_Value(srcIdx));
                            }
                            catch { }
                        }
                        else if ( aSrcFldName.Trim()=="GXSJ")
                        {
                            buffer.set_Value(fi, dt);//记录时间戳
                        }
                    }
                    insertCursor.InsertFeature(buffer);
                }
                insertCursor.Flush();

            }
            catch (Exception ex)
            {
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(insertCursor);
                GC.Collect();
            }


        }


        /// <summary>
        /// 将 要素插入到 历史图层中
        /// </summary>
        /// <param name="pws">目标空间</param>
        /// <param name="lst"></param>
        /// <param name="gxsj">记录该批次时间</param>
        public static void InsertHistory(IWorkspace pws, List<IFeature> lst,DateTime gxsj)
        {
            IFeatureClass dltbHClass = null;
            try
            {
                dltbHClass = (pws as IFeatureWorkspace).OpenFeatureClass("DLTB_H");
            }
            catch (Exception ex)
            {
            }
            if (dltbHClass == null) return;
           
            IFeatureCursor insertCursor = dltbHClass.Insert(true);
            try
            {
                foreach (IFeature aFea in lst)
                {
                    IFeatureBuffer buffer = dltbHClass.CreateFeatureBuffer();
                    buffer.Shape = aFea.ShapeCopy;
                    int fldCount = dltbHClass.Fields.FieldCount;
                    for (int fi = 0; fi < fldCount; fi++)
                    {
                        IField aSrcFld = dltbHClass.Fields.get_Field(fi);
                        #region 判断源字段是否需要处理
                        if (aSrcFld.Type == esriFieldType.esriFieldTypeOID
                            || aSrcFld.Type == esriFieldType.esriFieldTypeGeometry
                            || aSrcFld == dltbHClass.LengthField
                            || aSrcFld == dltbHClass.AreaField)
                        {
                            continue;
                        }

                        #endregion
                        string aSrcFldName = aSrcFld.Name.ToUpper();
                        int srcIdx = aFea.Fields.FindField(aSrcFldName);
                        if (srcIdx > -1)
                        {
                            try
                            {
                                buffer.set_Value(fi, aFea.get_Value(srcIdx));
                            }
                            catch { }
                        }
                        else if (aSrcFldName.Trim() == "GXSJ")
                        {
                            buffer.set_Value(fi, gxsj);//记录时间戳
                        }
                    }
                    insertCursor.InsertFeature(buffer);
                }
                insertCursor.Flush();

            }
            catch (Exception ex)
            {
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(insertCursor);
                GC.Collect();
            }


        }

        //将 要素插入到 历史图层中
        public static void InsertHistory(IWorkspace pws, ArrayList lst)
        {
            IFeatureClass dltbHClass = null;
            try
            {
                dltbHClass = (pws as IFeatureWorkspace).OpenFeatureClass("DLTB_H");
            }
            catch (Exception ex)
            {
            }
            if (dltbHClass == null) return;
            DateTime dt = DateTime.Now;
            IFeatureCursor insertCursor = dltbHClass.Insert(true);
            try
            {
                foreach (IFeature aFea in lst)
                {
                    IFeatureBuffer buffer = dltbHClass.CreateFeatureBuffer();
                    buffer.Shape = aFea.ShapeCopy;
                    int fldCount = dltbHClass.Fields.FieldCount;
                    for (int fi = 0; fi < fldCount; fi++)
                    {
                        IField aSrcFld = dltbHClass.Fields.get_Field(fi);
                        #region 判断源字段是否需要处理
                        if (aSrcFld.Type == esriFieldType.esriFieldTypeOID
                            || aSrcFld.Type == esriFieldType.esriFieldTypeGeometry
                            || aSrcFld == dltbHClass.LengthField
                            || aSrcFld == dltbHClass.AreaField)
                        {
                            continue;
                        }

                        #endregion
                        string aSrcFldName = aSrcFld.Name.ToUpper();
                        int srcIdx = aFea.Fields.FindField(aSrcFldName);
                        if (srcIdx > -1)
                        {
                            try
                            {
                                buffer.set_Value(fi, aFea.get_Value(srcIdx));
                            }
                            catch { }
                        }
                        else if (aSrcFldName.Trim() == "GXSJ")
                        {
                            buffer.set_Value(fi, dt);//记录时间戳
                        }
                    }
                    insertCursor.InsertFeature(buffer);
                }
                insertCursor.Flush();

            }
            catch (Exception ex)
            {
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(insertCursor);
                GC.Collect();
            }


        }


        public static void RestoreByHistory(IFeatureClass dltbClass, List<IFeature> hFeatures)
        {
            //删除就得，插入历史的

            if (dltbClass == null) return;
            DateTime dt = DateTime.Now;
            IFeatureCursor insertCursor = dltbClass.Insert(true);
            try
            {
                foreach (IFeature aFea in hFeatures)
                {
                    IFeatureBuffer buffer = dltbClass.CreateFeatureBuffer();
                    buffer.Shape = aFea.ShapeCopy;
                    int fldCount = dltbClass.Fields.FieldCount;
                    for (int fi = 0; fi < fldCount; fi++)
                    {
                        IField aSrcFld = dltbClass.Fields.get_Field(fi);
                        #region 判断源字段是否需要处理
                        if (aSrcFld.Type == esriFieldType.esriFieldTypeOID
                            || aSrcFld.Type == esriFieldType.esriFieldTypeGeometry
                            || aSrcFld == dltbClass.LengthField
                            || aSrcFld == dltbClass.AreaField)
                        {
                            continue;
                        }

                        #endregion
                        string aSrcFldName = aSrcFld.Name.ToUpper();
                        int srcIdx = aFea.Fields.FindField(aSrcFldName);
                        if (srcIdx > -1)
                        {
                            try
                            {
                                buffer.set_Value(fi, aFea.get_Value(srcIdx));
                            }
                            catch { }
                        }
                       
                    }
                    insertCursor.InsertFeature(buffer);
                }
                insertCursor.Flush();

            }
            catch (Exception ex)
            {
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(insertCursor);
                GC.Collect();
                GC.WaitForPendingFinalizers(); 
            }

        }



    }
}
