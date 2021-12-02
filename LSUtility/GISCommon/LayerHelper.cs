using System;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;

using ESRI.ArcGIS.Geodatabase;
using System.Collections;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Controls;
using System.Collections.Generic;

namespace RCIS.GISCommon
{
    public class LayerHelper
    {
        public static void SetDataSource(ILayer pLayer, string fileName)
        {
            IDataLayer2 pDataLayer = pLayer as IDataLayer2;
            try
            {
                IDatasetName pDataName = pDataLayer.DataSourceName as IDatasetName;
                try
                {
                    pDataLayer.Disconnect();
                }
                catch { }
                pDataName = pDataLayer.DataSourceName as IDatasetName;
                IWorkspaceName pWorkspaceName = WorkspaceHelper2.GetWorkspaceName(fileName);

                pDataLayer.DataSourceName = pWorkspaceName as IName;
                pDataName.WorkspaceName = pWorkspaceName;
                pDataLayer.Connect(pDataName as IName);
                pDataName = pDataLayer.DataSourceName as IDatasetName;
            }
            catch (Exception ex) { }
        }

        /// <summary>
        /// 获取 要素类objectid字段名称
        /// </summary>
        /// <param name="pLayer"></param>
        /// <returns></returns>
        public static string getLayerObjId(IFeatureClass pSelectionClass)
        {
            for (int i = 0; i < pSelectionClass.Fields.FieldCount; i++)
            {
                IField pFld = pSelectionClass.Fields.get_Field(i);
                if (pFld.Type == esriFieldType.esriFieldTypeOID)
                {
                    return pFld.Name.ToUpper();
                }
            }
            return "OBJECTID";
        }


        /// <summary>
        /// 创建属性索引
        /// </summary>
        /// <param name="pFC"></param>
        public static bool CreatePropIndex(IFeatureClass pFeatureClass)
        {
            IIndex pIndex = new IndexClass();
            IIndexEdit pIndexEdit = pIndex as IIndexEdit;
            IFields pFields = new FieldsClass();
            IFieldsEdit pFieldsEdit = pFields as IFieldsEdit;
            int feildindex = pFeatureClass.Fields.FindField("BSM");
            if (feildindex == -1)
                return true;
            try
            {
                IField pField = pFeatureClass.Fields.Field[feildindex];
                pFieldsEdit.FieldCount_2 = 1;
                pFieldsEdit.set_Field(0, pField);
                pIndexEdit.Fields_2 = pFields;
                pIndexEdit.Name_2 = "BSM";
                pIndexEdit.IsAscending_2 = true;
                pFeatureClass.AddIndex(pIndex);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        /// <summary>
        /// 图层加载到combox中
        /// </summary>
        /// <param name="cmbLayer"></param>
        /// <param name="currMap"></param>
        public static void LoadLayer2Combox(DevExpress.XtraEditors.ComboBoxEdit cmbLayer, IMap currMap)
        {
            cmbLayer.Properties.Items.Clear();
            for (int i = 0; i < currMap.LayerCount; i++)
            {
                ILayer currLyr = currMap.get_Layer(i);
                if (currLyr is IFeatureLayer)
                {

                    IFeatureLayer currFeaLyr = currLyr as IFeatureLayer;
                    IFeatureClass currClass = currFeaLyr.FeatureClass;
                    string clsName = (currClass as IDataset).Name.ToUpper();
                    cmbLayer.Properties.Items.Add(clsName + "|" + currClass.AliasName);
                }
                else if (currLyr is IGroupLayer)
                {
                    ICompositeLayer pCLyr = currLyr as ICompositeLayer;
                    for (int j = 0; j < pCLyr.Count; j++)
                    {
                        ILayer childLyr = pCLyr.get_Layer(j);
                        if (childLyr is IFeatureLayer)
                        {
                            IFeatureLayer currFeaLyr = childLyr as IFeatureLayer;
                            IFeatureClass currClass = currFeaLyr.FeatureClass;

                            string clsName = (currClass as IDataset).Name.ToUpper();
                            cmbLayer.Properties.Items.Add(clsName + "|" + currClass.AliasName);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 图层加载到combox中
        /// </summary>
        /// <param name="cmbLayer"></param>
        /// <param name="currMap"></param>
        public static void LoadLayer2Combox(DevExpress.XtraEditors.ComboBoxEdit cmbLayer, IMap currMap,esriGeometryType geoType)
        {
            cmbLayer.Properties.Items.Clear();
            for (int i = 0; i < currMap.LayerCount; i++)
            {
                ILayer currLyr = currMap.get_Layer(i);
                if (currLyr is IFeatureLayer)
                {
                    IFeatureLayer currFeaLyr = currLyr as IFeatureLayer;
                    IFeatureClass currClass = currFeaLyr.FeatureClass;
                    if (currClass.ShapeType != geoType)
                        continue;
                    string clsName = (currClass as IDataset).Name.ToUpper();
                    cmbLayer.Properties.Items.Add(clsName + "|" + currClass.AliasName);
                }
                else if (currLyr is IGroupLayer)
                {
                    ICompositeLayer pCLyr = currLyr as ICompositeLayer;
                    for (int j = 0; j < pCLyr.Count; j++)
                    {
                        ILayer childLyr = pCLyr.get_Layer(j);
                        if (childLyr is IFeatureLayer)
                        {
                            IFeatureLayer currFeaLyr = childLyr as IFeatureLayer;
                            IFeatureClass currClass = currFeaLyr.FeatureClass;
                            if (currClass.ShapeType != geoType)
                                continue;
                            string clsName = (currClass as IDataset).Name.ToUpper();
                            cmbLayer.Properties.Items.Add(clsName + "|" + currClass.AliasName);
                        }
                    }
                }

            }
        }


      
        /// <summary>
        /// 图层加载到combox中
        /// </summary>
        /// <param name="cmbLayer"></param>
        /// <param name="currMap"></param>
        /// <param name="isVisible">是否只添加可见</param>
        public static void LoadLayer2Combox(DevExpress.XtraEditors.ComboBoxEdit cmbLayer, IMap currMap, bool isVisible)
        {
            cmbLayer.Properties.Items.Clear();
            for (int i = 0; i < currMap.LayerCount; i++)
            {
                ILayer currLyr = currMap.get_Layer(i);
                if (isVisible)
                {
                    if (currLyr.Visible)
                    {
                        if (currLyr is IFeatureLayer)
                        {
                            IFeatureLayer currFeaLyr = currLyr as IFeatureLayer;
                            IFeatureClass currClass = currFeaLyr.FeatureClass;
                            string clsName = (currClass as IDataset).Name.ToUpper();
                            cmbLayer.Properties.Items.Add(clsName + "|" + currClass.AliasName);
                        }
                    }
                    else if (currLyr is IGroupLayer)
                    {
                        ICompositeLayer pCLyr = currLyr as ICompositeLayer;
                        for (int j = 0; j < pCLyr.Count; j++)
                        {
                            ILayer childLyr = pCLyr.get_Layer(j);
                            if (childLyr.Visible)
                            {
                                IFeatureLayer currFeaLyr = childLyr as IFeatureLayer;
                                IFeatureClass currClass = currFeaLyr.FeatureClass;
                                string clsName = (currClass as IDataset).Name.ToUpper();
                                cmbLayer.Properties.Items.Add(clsName + "|" + currClass.AliasName);
                            }
                        }
                    }

                }
                

            }
        }

        /// <summary>
        /// 图层加载到combox中
        /// </summary>
        /// <param name="cmbLayer"></param>
        /// <param name="currMap"></param>
        /// <param name="isVisible">是否只添加可见</param>
        /// <param name="geoType">只添加指定几何类型的要素类图层</param>
        public static void LoadLayer2Combox(DevExpress.XtraEditors.ComboBoxEdit cmbLayer, IMap currMap, bool isVisible, esriGeometryType geoType)
        {
            cmbLayer.Properties.Items.Clear();
            for (int i = 0; i < currMap.LayerCount; i++)
            {
                ILayer currLyr = currMap.get_Layer(i);
                if (isVisible)
                {
                    if (currLyr.Visible)
                    {
                        if (currLyr is IFeatureLayer)
                        {
                            IFeatureLayer currFeaLyr = currLyr as IFeatureLayer;
                            IFeatureClass currClass = currFeaLyr.FeatureClass;
                            if (currClass.ShapeType == geoType)
                            {
                                string clsName = (currClass as IDataset).Name.ToUpper();
                                cmbLayer.Properties.Items.Add(clsName + "|" + currClass.AliasName);
                            }

                        }
                        else if (currLyr is IGroupLayer)
                        {
                            ICompositeLayer pCLyr = currLyr as ICompositeLayer;
                            for (int j = 0; j < pCLyr.Count; j++)
                            {
                                ILayer childLyr = pCLyr.get_Layer(j);
                                if (currLyr.Visible)
                                {
                                    if (childLyr is IFeatureLayer)
                                    {
                                        IFeatureLayer currFeaLyr = childLyr as IFeatureLayer;
                                        IFeatureClass currClass = currFeaLyr.FeatureClass;
                                        if (currClass.ShapeType != geoType)
                                            continue;
                                        string clsName = (currClass as IDataset).Name.ToUpper();
                                        cmbLayer.Properties.Items.Add(clsName + "|" + currClass.AliasName);
                                    }
                                }
                            }
                        }
                       
                    }

                }


            }
        }


        /// <summary>
        /// 获取相交图层
        /// </summary>
        /// <param name="pClass"></param>
        /// <param name="pGeom"></param>
        /// <returns></returns>
        public static List<IFeature> GetIntersectFeature(IFeatureClass pClass, IGeometry pGeom)
        {
            List<IFeature> rList = new List<IFeature>();
            if (pClass == null) return rList;
            if (pGeom == null || pGeom.IsEmpty) return rList;
            ISpatialReference sr = (pClass as IGeoDataset).SpatialReference;
            if (sr != null)
            {
                pGeom.SpatialReference = sr;
                pGeom.SnapToSpatialReference();
            }
            ISpatialFilter spFilter = new SpatialFilterClass();
            spFilter.Geometry = pGeom;
            spFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            IFeatureCursor aCursor = pClass.Search(spFilter, false);
            IFeature aFea = aCursor.NextFeature();
            while (aFea != null)
            {
                rList.Add(aFea);
                aFea = aCursor.NextFeature();
            }
            System.Runtime.InteropServices.Marshal.ReleaseComObject(aCursor);
            return rList;
        }

        /// <summary>
        /// 移除图层
        /// </summary>
        /// <param name="pMapControl"></param>
        public static  void removeAllLayers(IMapControl3 pMapControl)
        {
            //清除所有图层，为下一个县的图层加载做准备
            //获取图层数，释放所有workspce
            for (int i = pMapControl.LayerCount - 1; i >= 0; i--)
            {
                ILayer pLyr = pMapControl.get_Layer(i);
                pMapControl.DeleteLayer(i);
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(pLyr);
                
            }
            pMapControl.ClearLayers();

        }

        /// <summary>
        /// 获取特定图层上被选中的要素。
        /// </summary>
        /// <param name="pMap"></param>
        /// <param name="pLayer"></param>
        /// <param name="pGT"></param>
        /// <returns></returns>
        public static ArrayList GetSelectedFeature(IMap pMap, IGeoFeatureLayer pLayer, esriGeometryType pGT)
        {
            ArrayList resultList = new ArrayList();
            IFeatureClass aClass = null;
            if (pLayer != null) aClass = pLayer.FeatureClass;
            ISelection selection = pMap.FeatureSelection;
            IEnumFeatureSetup enumFeatureSetup = (IEnumFeatureSetup)selection;
            enumFeatureSetup.AllFields = true;
            IEnumFeature enumFeature = enumFeatureSetup as IEnumFeature;
            enumFeature.Reset();
            IFeature feature = enumFeature.Next();
            while (feature != null)
            {
                IFeatureClass aFeaClass = feature.Table as IFeatureClass;
                if (aClass == null
                    || aFeaClass == aClass)
                {
                    resultList.Add(feature);
                }
                feature = enumFeature.Next();
            }

            return resultList;
        }

        /// <summary>
        /// 获取特定图层上被选中的要素。
        /// </summary>
        /// <param name="pMap"></param>
        /// <param name="pLayer"></param>
        /// <param name="pGT"></param>
        /// <returns></returns>
        public static List<IFeature> GetSelectedFeature(IMap pMap, IGeoFeatureLayer pLayer)
        {
            List<IFeature> resultList = new List<IFeature>();
            IFeatureClass aClass = null;
            if (pLayer != null) aClass = pLayer.FeatureClass;
            ISelection selection = pMap.FeatureSelection;
            IEnumFeatureSetup enumFeatureSetup = (IEnumFeatureSetup)selection;
            enumFeatureSetup.AllFields = true;
            IEnumFeature enumFeature = enumFeatureSetup as IEnumFeature;
            enumFeature.Reset();
            IFeature feature = enumFeature.Next();
            while (feature != null)
            {
                IFeatureClass aFeaClass = feature.Table as IFeatureClass;
                if (aClass == null
                    || aFeaClass == aClass)
                {
                    resultList.Add(feature);
                }
                feature = enumFeature.Next();
            }

           
            return resultList;
        }

        /// <summary>
        /// 获取特定图层上被选中的要素。
        /// </summary>
        /// <param name="pMap"></param>
        /// <param name="pLayer"></param>
        /// <param name="pGT"></param>
        /// <returns></returns>
        public static ArrayList GetSelectedFeatureOID(IMap pMap, IGeoFeatureLayer pLayer, esriGeometryType pGT)
        {
            ArrayList resultList = new ArrayList();
            IFeatureClass aClass = null;
            if (pLayer != null) aClass = pLayer.FeatureClass;
            IEnumFeature aEnumFea = pMap.FeatureSelection as IEnumFeature;
            IFeature aFea = aEnumFea.Next();
            while (aFea != null)
            {
                IFeatureClass aFeaClass = aFea.Table as IFeatureClass;
                if (aClass == null
                    || aFeaClass == aClass)
                {

                    if (pGT == esriGeometryType.esriGeometryAny
                        || aFeaClass.ShapeType == pGT)
                    {
                        resultList.Add(aFea.OID);
                    }
                }
                aFea = aEnumFea.Next();
            }
            return resultList;
        }

        /// <summary>
        /// 根据地类图斑－－－获得“DLTB”
        /// </summary>
        /// <param name="featLayer"></param>
        /// <returns></returns>
        public static string GetFeatureLayerTableName(IFeatureClass fc)
        {
            try
            {

                string shortName = "";
                shortName = (fc as IDataset).Name;
                int index = shortName.LastIndexOf(".");
                if (index >= 0)
                {
                    shortName = shortName.Substring(index + 1);
                }
                return shortName;
            }
            catch (Exception ee)
            {

                System.Windows.Forms.MessageBox.Show(ee.ToString());
                return "";
            }
        }

        /// <summary>
        /// 找到分组图层
        /// </summary>
        /// <param name="paramMap"></param>
        /// <param name="paramModelName"></param>
        /// <returns></returns>
        public static IGroupLayer QueryGroupLyrByName(IMap paramMap, String paramModelName)
        {
            if (paramMap == null) return null;
            if (paramModelName == null) return null;
            int layerCount = paramMap.LayerCount;
            paramModelName = paramModelName.ToUpper();
            for (int i = 0; i < layerCount; i++)
            {
                ILayer curLayer = paramMap.get_Layer(i);
                if (curLayer is IGroupLayer)
                {
                    if (curLayer.Name.ToUpper() == paramModelName)
                    {
                        return curLayer as IGroupLayer;
                    }
                }
            }
            return null;
        }


        /// <summary>
        /// 按照名称删除图层
        /// </summary>
        /// <param name="IN_Name"></param>
        public static void DeleteLyersByName(IMap paramMap,string IN_Name)
        {
            ILayer pL = null;
            for (int i = 0; i < paramMap.LayerCount; i++)
            {
                pL = paramMap.get_Layer(i);
                if (pL is IFeatureLayer)
                {
                    IFeatureClass curClass = (pL as IGeoFeatureLayer).FeatureClass;
                    string curModelName = GetClassShortName(curClass as IDataset).ToUpper();
                    if (curModelName.Equals(IN_Name))
                    {
                        paramMap.DeleteLayer(pL);
                    }
                }
               
            }
        }

        public static IGeoFeatureLayer QueryLayerByModelName(IMap paramMap, String paramModelName)
        {
            if (paramMap == null) return null;
            if (paramModelName == null) return null;
            int layerCount = paramMap.LayerCount;
            paramModelName = paramModelName.ToUpper();
            for (int i = 0; i < layerCount; i++)
            {
                ILayer curLayer = paramMap.get_Layer(i);
                if (curLayer is IGeoFeatureLayer)
                {
                    IFeatureClass curClass = (curLayer as IGeoFeatureLayer).FeatureClass;
                    string curModelName = GetClassShortName(curClass as IDataset).ToUpper();
                    if (curModelName.Equals(paramModelName))
                    {
                        return curLayer as IGeoFeatureLayer;
                    }
                }
                else if (curLayer is IGroupLayer)
                {
                    //如果是组合图层
                    ICompositeLayer pCLyr = curLayer as ICompositeLayer;
                    for (int j = 0; j < pCLyr.Count; j++)
                    {
                        ILayer childLyr = pCLyr.get_Layer(j);
                        if (childLyr is IFeatureLayer)
                        {
                            IFeatureClass curClass = (childLyr as IGeoFeatureLayer).FeatureClass;
                            string curModelName = GetClassShortName(curClass as IDataset).ToUpper();
                            if (curModelName.Equals(paramModelName))
                            {
                                return childLyr as IGeoFeatureLayer;
                            }
                        }
                    }
                }

                
            }
            return null;
        }

        /// <summary>
        /// 获取图层 索引
        /// </summary>
        /// <param name="paramMap"></param>
        /// <param name="paramLayer"></param>
        /// <returns></returns>
        public static int IndexOfLayer(IMap paramMap, ILayer paramLayer)
        {
            int resultIndex = -1;
            int layerCount = paramMap.LayerCount;
            for (int li = 0; li < layerCount; li++)
            {
                ILayer curLayer = paramMap.get_Layer(li);
                if (curLayer is IGroupLayer)
                {
                    ICompositeLayer pCLyr = curLayer as ICompositeLayer;
                    for (int j = 0; j < pCLyr.Count; j++)
                    {
                        ILayer childLyr = pCLyr.get_Layer(j);
                        if (childLyr == paramLayer)
                        {
                            resultIndex = li;
                            break;
                        }
                    }
                    
                }
                else
                {
                    if (curLayer == paramLayer)
                    {
                        resultIndex = li;
                        break;
                    }
                }
            }
            return resultIndex;
        }

        /// <summary>
        /// 返回图层类型的中文描述
        /// </summary>
        /// <param name="paramLayer"></param>
        /// <returns></returns>
        public static string LayerTypeName(ILayer paramLayer)
        {
            string resultName = "";
            if (paramLayer is IGeoFeatureLayer)
            {
                IFeatureClass curClass = (paramLayer as IGeoFeatureLayer).FeatureClass;
                if (curClass != null)
                {
                    resultName = GeometryHelper.ShapeTypeName(curClass.ShapeType);
                }
            }
            else if (paramLayer is IRasterLayer)
            {
                resultName = "影像";
            }
            else if (paramLayer is ITopologyLayer)
            {
                resultName = "拓扑";
            }
            else if (paramLayer is IAnnotationLayer)
            {
                resultName = "注记";
            }
            return resultName;
        }

        public static String GetClassOwnerName(String pDSName)
        {
            int index = pDSName.IndexOf(".");
            if (index >= 0)
                pDSName = pDSName.Substring(0, index);
            else pDSName = "";
            pDSName = pDSName.ToUpper();
            return pDSName;
        }



        public static byte[] SaveLayerToStream(ILayer paramLayer)
        {
            byte[] resultBT = null;
            if (paramLayer is IPersistStream)
            {
                IPersistStream ps = paramLayer as IPersistStream;
                XMLStreamClass stream = new XMLStreamClass();
                ps.Save(stream, 0);
                resultBT = stream.SaveToBytes();
            }
            return resultBT;
        }
        public static void LoadLayerFromStream(ILayer paramLayer, byte[] paramLayerContent)
        {
            if (paramLayer == null
                || paramLayerContent == null
                || paramLayerContent.Length == 0)
            {
                return;
            }
            IPersistStream ps = paramLayer as IPersistStream;
            XMLStreamClass stream = new XMLStreamClass();
            stream.LoadFromBytes(ref paramLayerContent);
            ps.Load(stream);
        }

      

        /// <summary>
        /// 获取所有图层名
        /// </summary>
        /// <param name="pWS"></param>
        /// <returns></returns>
        public static List<string> GetAllLayerName(IWorkspace pWS)
        {
            List<string> lstNames = new List<string>();

            //获取第一个Dataset
            IEnumDataset pEnumDs = pWS.get_Datasets(esriDatasetType.esriDTAny);
            pEnumDs.Reset();
            IDataset ds = pEnumDs.Next();
            if (ds != null)
            {

                IEnumDataset EnumSubClass = ds.Subsets;
                IDataset subDs = EnumSubClass.Next();
                while (subDs != null)
                {
                    lstNames.Add(subDs.Name);
                    subDs = EnumSubClass.Next();
                }
            }

            //IEnumDatasetName pEnumDs = pWS.get_DatasetNames(esriDatasetType.esriDTFeatureClass);
            //IDatasetName dsName = pEnumDs.Next();
            //while (dsName != null)
            //{
            //    lstNames.Add(dsName.Name);
            //    dsName = pEnumDs.Next();
            //}
            lstNames.Sort();
            return lstNames;
        }


        public static String GetClassShortName(IDataset paramDS)
        {
            if (paramDS == null) return "";
            String dsName = paramDS.Name.ToUpper();
            return GetClassShortName(dsName);

        }
        public static String GetClassShortName(String paramName)
        {
            String shortName = paramName;
            int splitIndex = paramName.LastIndexOf(".");
            if (splitIndex >= 0)
                shortName = paramName.Substring(splitIndex + 1);
            return shortName;
        }

       
    
    }


    

}
