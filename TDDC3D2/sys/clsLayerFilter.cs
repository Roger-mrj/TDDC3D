using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using System.Data;
using RCIS.Database;

namespace TDDC3D.sys
{
    public class clsLayerFilter
    {
        //用与图层过滤的过滤器
        private SortedList<string, List<clsFilterAttri>> listFilter = null;

        //执行过滤
        public void ExecuteFilt(AxMapControl MapControl)
        {
            string layerName = "";
            //IFeatureLayer featureLayer = null;
            double mapScale = 0;
            IFeatureLayerDefinition2 flDef = null;
            for (int i = 0; i < MapControl.LayerCount; i++)
            {
                try
                {
                    ILayer currentLyr = MapControl.get_Layer(i);
                    if (currentLyr.Visible == false )
                    {
                        continue;
                    }

                    if (currentLyr is IFeatureLayer)
                    {
                        IFeatureLayer pFeatLayer = currentLyr as IFeatureLayer;
                        IFeatureClass featCls = pFeatLayer.FeatureClass;
                        layerName = currentLyr.Name;
                        layerName = ((IDataset)featCls).Name;

                        if (listFilter == null)
                            listFilter = getFilter();

                        if (listFilter == null)
                            continue;

                        if (listFilter.Keys.Contains(layerName))
                        {
                            List<clsFilterAttri> listAttri = null;
                            if (listFilter.TryGetValue(layerName, out listAttri))
                            {
                                mapScale = MapControl.MapScale;
                                foreach (clsFilterAttri attri in listAttri)
                                {
                                    if (attri.minFilterScale > mapScale)
                                    {
                                        flDef = (IFeatureLayerDefinition2)currentLyr;
                                        flDef.DefinitionExpression = attri.whereClause;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else if (currentLyr is IGroupLayer)
                    {
                        ICompositeLayer compsiteLyr = currentLyr as ICompositeLayer;
                        for (int kk = 0; kk < compsiteLyr.Count; kk++)
                        {
                            ILayer childLyr = compsiteLyr.get_Layer(kk);
                            if (childLyr is IFeatureLayer)
                            {
                                IFeatureLayer pFeatLayer = childLyr as IFeatureLayer;
                                IFeatureClass featCls = pFeatLayer.FeatureClass;
                                layerName = currentLyr.Name;
                                layerName = ((IDataset)featCls).Name;

                                if (listFilter == null)
                                    listFilter = getFilter();

                                if (listFilter == null)
                                    continue;

                                if (listFilter.Keys.Contains(layerName))
                                {
                                    List<clsFilterAttri> listAttri = null;
                                    if (listFilter.TryGetValue(layerName, out listAttri))
                                    {
                                        mapScale = MapControl.MapScale;
                                        foreach (clsFilterAttri attri in listAttri)
                                        {
                                            if (attri.minFilterScale > mapScale)
                                            {
                                                flDef = (IFeatureLayerDefinition2)currentLyr;
                                                flDef.DefinitionExpression = attri.whereClause;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }


                    
                    
                }
                catch (Exception ex)
                {
                }

            }
        }

        //获取需要过滤的图层类型      
        private SortedList<string, List<clsFilterAttri>> getFilter()
        {
            listFilter = new SortedList<string, List<clsFilterAttri>>();

            try
            {
                string strSQL = " SELECT F_LayerType,F_FilterField  FROM  SYS_LAYERVISIBLESET  WHERE F_Filter='1'";                         
                DataTable dt = LS_SetupMDBHelper.GetDataTable(strSQL, "SYS_LAYERVISIBLESET");

                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0] != DBNull.Value)
                {
                    string layerType = "";
                    string filterField = "";
                    //单个图层对应的多个过滤比例尺
                    List<clsFilterAttri> listAttri = null;
                    foreach (DataRow dr in dt.Rows)
                    {
                        //图层类型
                        layerType = dr[0].ToString();
                        //过滤的字段
                        filterField = dr[1].ToString();

                        strSQL = " SELECT F_Value,F_FilterMinScale,F_FilterMaxScale  FROM  SYS_LAYERFILTERSET  WHERE F_LayerType ='" + layerType + "'"
                                    + " ORDER BY F_FilterMinScale  ASC";
                        DataTable dt2 = LS_SetupMDBHelper.GetDataTable(strSQL, "SYS_LAYERFILTERSET");

                        listAttri = new List<clsFilterAttri>();
                        double value = 0;
                        double minScale = 0;
                        double maxScale = 0;
                        if (dt2 != null && dt2.Rows.Count > 0 && dt2.Rows[0][0] != DBNull.Value)
                        {
                            clsFilterAttri attri = null;
                            foreach (DataRow dr2 in dt2.Rows)
                            {
                                value = Convert.ToDouble(dr2[0].ToString());
                                minScale = Convert.ToDouble(dr2[1].ToString());
                                maxScale = Convert.ToDouble(dr2[2].ToString());

                                attri = new clsFilterAttri(minScale, maxScale);
                                attri.whereClause = filterField + " > " + value;

                                listAttri.Add(attri);
                            }
                        }
                        listFilter.Add(layerType, listAttri);
                    }
                }
            }
            catch (Exception ex)
            { }
            return listFilter;
        }
    }

    /// <summary>
    /// 过滤器辅助属性类
    /// </summary>
    class clsFilterAttri
    {
        public clsFilterAttri(double minScale, double maxScale)
        {
            minFilterScale = minScale;
            maxFilterScale = maxScale;
        }
        public double minFilterScale = 0;
        public double maxFilterScale = 0;
        public double filterValue = 0;
        public string whereClause = "";
    }
}
