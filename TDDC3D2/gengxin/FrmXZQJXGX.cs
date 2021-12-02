using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geoprocessor;
using System.IO;
using ESRI.ArcGIS.Geometry;

namespace TDDC3D.gengxin
{
    public partial class FrmXZQJXGX : Form
    {
        public IWorkspace currWs = null;

        public FrmXZQJXGX()
        {
            InitializeComponent();
        }

        private void UpdateStatus(string txt)
        {
            info.Text = DateTime.Now.ToString() + ":" + txt + "\r\n" + info.Text;
            Application.DoEvents();
        }

        private void DeleteFile(string filePath)
        {
            if (Directory.Exists(filePath))
            {
                foreach (string str in Directory.GetFileSystemEntries(filePath))
                {
                    if (File.Exists(str))
                    {
                        File.Delete(str);
                    }
                }
            }
        }
        private void btnXZQJXGX_Click(object sender, EventArgs e)
        {
            
            //变化行政区
            IFeatureWorkspace pFeaWorkspace = currWs as IFeatureWorkspace;
            IFeatureClass pXZQJX = pFeaWorkspace.OpenFeatureClass("XZQJXGX");
            if (MessageBox.Show("原行政区界线更新层中的数据将被删除，然后重新生成，是否继续？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes) return;
            
            (pXZQJX as ITable).DeleteSearchedRows(null);
            string filePath = currWs.PathName;
            UpdateStatus("正在提取行政区界线");
            
            IFeatureClass pGXGCFeatureclass = GPPolygonToLineByFeatureclass(filePath + "\\TDGX\\XZQGXGC");
            IFeatureClass pGXFeatureclass = GPPolygonToLineByFeatureclass(filePath + "\\TDGX\\XZQGX");
            IFeatureClass pSDFeatureclass = GPPolygonToLineByFeatureclass(filePath + "\\TDDC\\XZQ");

            IFeatureClass mFeaClass = GPUpdate(filePath + "\\TDDC\\XZQ", filePath + "\\TDGX\\XZQGXGC");
            IFeatureClass allFeaClass = GPPolygonToLineByFeatureclass((mFeaClass as IDataset).Workspace.PathName + "\\" + (mFeaClass as IDataset).Name + ".shp");
            IFeatureCursor allFeaCursor = allFeaClass.Update(null,true);
            IFeature allFeature;
            while((allFeature=allFeaCursor.NextFeature())!=null)
            {
                ISpatialFilter SpaFil = new SpatialFilterClass();
                SpaFil = new SpatialFilterClass();
                SpaFil.Geometry = allFeature.ShapeCopy;
                SpaFil.SpatialRel = esriSpatialRelEnum.esriSpatialRelRelation;
                SpaFil.SpatialRelDescription = "T********";
                IFeatureCursor cursor = pGXGCFeatureclass.Search(SpaFil, true);
                IFeature feature = cursor.NextFeature();
                bool bl = false;
                while(feature!=null)
                {
                    bl = GetIntersectLength(allFeature,feature);
                    if (bl == true)
                        break;
                    feature = cursor.NextFeature();
                }
                if (bl == false)
                {
                    allFeaCursor.DeleteFeature();
                }
                RCIS.Utility.OtherHelper.ReleaseComObject(cursor);

            }
            allFeaCursor.Flush();
            RCIS.Utility.OtherHelper.ReleaseComObject(allFeaCursor);

            UpdateStatus("正在生成行政区界线更新层");
            using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
            {
                IFeatureCursor pInsert = pXZQJX.Insert(true);
                comRel.ManageLifetime(pInsert);
                using (ESRI.ArcGIS.ADF.ComReleaser comRel2 = new ESRI.ArcGIS.ADF.ComReleaser())
                {
                    IFeatureCursor pFeaCursor = pSDFeatureclass.Search(null, true);
                    comRel2.ManageLifetime(pFeaCursor);
                    IFeature pFeature;
                    ISpatialFilter pSpaFil = null;
                    while ((pFeature = pFeaCursor.NextFeature()) != null)
                    {
                        pSpaFil = new SpatialFilterClass();
                        pSpaFil.Geometry = pFeature.ShapeCopy;
                        pSpaFil.SpatialRel = esriSpatialRelEnum.esriSpatialRelRelation;
                        pSpaFil.SpatialRelDescription = "T********";
                        IFeatureCursor pGXGCFeatureCursor = allFeaClass.Search(pSpaFil, false);
                        IFeature pGXGCFeature;
                        IFeature tempFeature=null;
                        bool B = false;
                        int tmp = 0;
                        while ((pGXGCFeature=pGXGCFeatureCursor.NextFeature()) != null)
                        {
                            B = false;
                            B = GetIntersectLength(pFeature, pGXGCFeature);
                            if (B == true)
                            {
                                if (tmp == 0)
                                {
                                    tempFeature = pGXGCFeature;
                                    tmp += 1;
                                }
                                else
                                {
                                    pSpaFil = new SpatialFilterClass();
                                    pSpaFil.Geometry = pGXGCFeature.ShapeCopy;
                                    pSpaFil.SpatialRel = esriSpatialRelEnum.esriSpatialRelRelation;
                                    pSpaFil.SpatialRelDescription = "T********";
                                    IFeatureCursor pGXFeaCursor = pGXFeatureclass.Search(pSpaFil,true);
                                    IFeature pGXFeature;
                                    while ((pGXFeature = pGXFeaCursor.NextFeature()) != null)
                                    {
                                        if (GetIntersectLength(pGXGCFeature, pGXFeature) == true)
                                        {
                                            IFeatureBuffer pFeaBuffer = pXZQJX.CreateFeatureBuffer();
                                            int pFieldNum = pXZQJX.FindField("BGXW");
                                            pFeaBuffer.Shape = pGXGCFeature.ShapeCopy;
                                            pFeaBuffer.set_Value(pXZQJX.FindField("YSDM"), "1000600200");
                                            pFeaBuffer.set_Value(pXZQJX.FindField("JXLX"), "660200");
                                            pFeaBuffer.set_Value(pXZQJX.FindField("JXXZ"), "600001");
                                            pFeaBuffer.set_Value(pXZQJX.FindField("GXSJ"), DateTime.ParseExact("20191231", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture));
                                            pFeaBuffer.set_Value(pFieldNum, "1");//新增
                                            pInsert.InsertFeature(pFeaBuffer);
                                            break;
                                        }
                                    }
                                    RCIS.Utility.OtherHelper.ReleaseComObject(pGXFeaCursor);
                                    tmp += 1;
                                }
                            }
                        }
                        RCIS.Utility.OtherHelper.ReleaseComObject(pGXGCFeatureCursor);                                    

                        if (tmp==1)
                        {
                            IFeatureBuffer pFeaBuffer = pXZQJX.CreateFeatureBuffer();
                            int pFieldNum = pXZQJX.FindField("BGXW");
                            pFeaBuffer.Shape = pFeature.ShapeCopy;
                            pFeaBuffer.set_Value(pXZQJX.FindField("YSDM"), "1000600200");
                            pFeaBuffer.set_Value(pXZQJX.FindField("JXLX"), "660200");
                            pFeaBuffer.set_Value(pXZQJX.FindField("JXXZ"), "600001");
                            pFeaBuffer.set_Value(pXZQJX.FindField("GXSJ"), DateTime.ParseExact("20191231", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture));
                            pFeaBuffer.set_Value(pFieldNum, "3");//无变化
                            pInsert.InsertFeature(pFeaBuffer);
                        }
                        if (tmp > 1)
                        {
                            IFeatureBuffer pFeaBuffer = pXZQJX.CreateFeatureBuffer();
                            int pFieldNum = pXZQJX.FindField("BGXW");
                            pFeaBuffer.Shape = pFeature.ShapeCopy;
                            pFeaBuffer.set_Value(pXZQJX.FindField("YSDM"), "1000600200");
                            pFeaBuffer.set_Value(pXZQJX.FindField("JXLX"), "660200");
                            pFeaBuffer.set_Value(pXZQJX.FindField("JXXZ"), "600001");
                            pFeaBuffer.set_Value(pXZQJX.FindField("GXSJ"), DateTime.ParseExact("20191231", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture));
                            pFeaBuffer.set_Value(pFieldNum, "2");//灭失
                            pInsert.InsertFeature(pFeaBuffer);

                            pSpaFil = new SpatialFilterClass();
                            pSpaFil.Geometry = tempFeature.ShapeCopy;
                            pSpaFil.SpatialRel = esriSpatialRelEnum.esriSpatialRelRelation;
                            pSpaFil.SpatialRelDescription = "T********";
                            IFeatureCursor pGXFeatureCursor = pGXFeatureclass.Search(pSpaFil,true);
                            IFeature pGXFeature;
                            while ((pGXFeature = pGXFeatureCursor.NextFeature()) != null)
                            {
                                if (GetIntersectLength(tempFeature, pGXFeature) == true)
                                {
                                    IFeatureBuffer pFeatureBuffer = pXZQJX.CreateFeatureBuffer();
                                    pFeatureBuffer.Shape = tempFeature.ShapeCopy;
                                    pFeatureBuffer.set_Value(pXZQJX.FindField("YSDM"), "1000600200");
                                    pFeatureBuffer.set_Value(pXZQJX.FindField("JXLX"), "660200");
                                    pFeatureBuffer.set_Value(pXZQJX.FindField("JXXZ"), "600001");
                                    pFeatureBuffer.set_Value(pXZQJX.FindField("GXSJ"), DateTime.ParseExact("20191231", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture));
                                    pFeatureBuffer.set_Value(pFieldNum, "1");//新增
                                    pInsert.InsertFeature(pFeatureBuffer);
                                    break;
                                }
                            }
                            RCIS.Utility.OtherHelper.ReleaseComObject(pGXFeatureCursor);                                    
                                    
                        }
                    }
                    IFeatureCursor pCursor = pGXGCFeatureclass.Search(null, true);
                    IFeature pFea;
                    while ((pFea = pCursor.NextFeature()) != null)
                    {
                        pSpaFil = new SpatialFilterClass();
                        pSpaFil.Geometry = pFea.ShapeCopy;
                        pSpaFil.SpatialRel = esriSpatialRelEnum.esriSpatialRelRelation;
                        pSpaFil.SpatialRelDescription = "T********";
                        IFeatureCursor pSDCursor = pSDFeatureclass.Search(pSpaFil, true);
                        IFeature pSD = pSDCursor.NextFeature();
                        IFeatureCursor pGXCursor = pGXFeatureclass.Search(pSpaFil,true);
                        IFeature pGX = pGXCursor.NextFeature();
                        if (pSD == null && pGX != null)
                        {
                            IFeatureBuffer pFeatureBuffer = pXZQJX.CreateFeatureBuffer();
                            pFeatureBuffer.Shape = pFea.ShapeCopy;
                            pFeatureBuffer.set_Value(pXZQJX.FindField("YSDM"), "1000600200");
                            pFeatureBuffer.set_Value(pXZQJX.FindField("JXLX"), "660200");
                            pFeatureBuffer.set_Value(pXZQJX.FindField("JXXZ"), "600001");
                            pFeatureBuffer.set_Value(pXZQJX.FindField("GXSJ"), DateTime.ParseExact("20191231", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture));
                            int pFieldNum = pXZQJX.FindField("BGXW");
                            pFeatureBuffer.set_Value(pFieldNum, "1");//新增
                            pInsert.InsertFeature(pFeatureBuffer);
                        }
                        RCIS.Utility.OtherHelper.ReleaseComObject(pSDCursor);
                        RCIS.Utility.OtherHelper.ReleaseComObject(pGXCursor);                                    
                    }
                    RCIS.Utility.OtherHelper.ReleaseComObject(pCursor);                                    
                        
                }
                pInsert.Flush();
            }
            IDataset pGXGC = pGXGCFeatureclass as IDataset;
            pGXGC.Delete();
            IDataset GX = pGXFeatureclass as IDataset;
            GX.Delete();
            IDataset SD = pSDFeatureclass as IDataset;
            SD.Delete();
            IDataset mDataset = mFeaClass as IDataset;
            mDataset.Delete();
            IDataset all = allFeaClass as IDataset;
            all.Delete();
            UpdateStatus("行政区界线更新层生成完毕");
            MessageBox.Show("行政区界线更新层生成完毕。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool GetIntersectLength(IFeature pFeature, IFeature pFea) 
        {
            bool b = false;
            IGeometry pGeometry = pFeature.ShapeCopy;
            ITopologicalOperator pTop = pGeometry as ITopologicalOperator;
            //IFeatureCursor pFeaturecursor = pFeatureclass.Search(null, true);
            //IFeature pFea;
            //while ((pFea = pFeaturecursor.NextFeature()) != null)
            //{
                IGeometry pGeoIntersect = pTop.Intersect(pFea.ShapeCopy, esriGeometryDimension.esriGeometry1Dimension);
                if (pGeoIntersect != null)
                {
                    IPolyline pLine = pGeoIntersect as IPolyline;

                    //ILine pLine = pGeoIntersect as ILine;
                    if (pLine.Length > 0.0001)
                    {
                        b = true;
                        //break;
                    }
                }
            //}
            return b;
        }

        //private bool GetIntersectLength(IFeature pFeature, IFeatureClass pFeatureclass)
        //{
        //    bool b = false;
        //    IGeometry pGeometry = pFeature.ShapeCopy;
        //    ITopologicalOperator pTop = pGeometry as ITopologicalOperator;
        //    IFeatureCursor pFeaturecursor = pFeatureclass.Search(null, true);
        //    IFeature pFea;
        //    while ((pFea = pFeaturecursor.NextFeature()) != null)
        //    {
        //    IGeometry pGeoIntersect = pTop.Intersect(pFea.ShapeCopy, esriGeometryDimension.esriGeometry1Dimension);
        //    if (pGeoIntersect != null)
        //    {
        //        IPolyline pLine = pGeoIntersect as IPolyline;

        //        //ILine pLine = pGeoIntersect as ILine;
        //        if (pLine.Length > 0.0001)
        //        {
        //            b = true;
        //            break;
        //        }
        //    }
        //    }
        //    return b;
        //}

        private IFeatureClass GPPolygonToLineByFeatureclass(string inputFile)
        {
            IFeatureClass pFeaClass = null;
            string name = Guid.NewGuid().ToString().Replace("-","") + ".shp";
            string temp = Application.StartupPath + "\\tmp\\"+name;
            
            ESRI.ArcGIS.DataManagementTools.PolygonToLine PolygonToLine = new ESRI.ArcGIS.DataManagementTools.PolygonToLine();
            PolygonToLine.in_features = inputFile;
            PolygonToLine.neighbor_option = "IDENTIFY_NEIGHBORS";
            PolygonToLine.out_feature_class = temp;
            Geoprocessor geoProcessor = new Geoprocessor();
            try
            {
                geoProcessor.Execute(PolygonToLine, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return pFeaClass;
            }
            pFeaClass = RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(temp);
            return pFeaClass;
        }

        private IFeatureClass GPUpdate(string inputFea,string updateFea) 
        {
            IFeatureClass pFeaClass = null;
            string name = Guid.NewGuid().ToString().Replace("-", "") + ".shp";
            string temp = Application.StartupPath + "\\tmp\\" + name;
            
            ESRI.ArcGIS.AnalysisTools.Update update = new ESRI.ArcGIS.AnalysisTools.Update();
            update.in_features = inputFea;
            update.update_features = updateFea;
            update.out_feature_class = temp;
            Geoprocessor geoProcessor = new Geoprocessor();
            try
            {
                geoProcessor.Execute(update, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return pFeaClass;
            }
            pFeaClass = RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(temp);
            return pFeaClass;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnEditBSM_Click(object sender, EventArgs e)
        {
            IFeatureWorkspace pFeaWorkspace = currWs as IFeatureWorkspace;
            IFeatureClass pXZQJXClass = pFeaWorkspace.OpenFeatureClass("XZQJX");

            if (pXZQJXClass == null)
            {
                MessageBox.Show("行政区界线图层没有数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            UpdateStatus("正在生成标识码");
            IFeatureClass pGXClass = pFeaWorkspace.OpenFeatureClass("XZQJXGX");
            long maxBSM = RCIS.GISCommon.FeatureHelper.GetMaxStringNumberEveryOne(pXZQJXClass, "BSM");
            maxBSM = EditFieldFromMaxNum(maxBSM, pGXClass, "BSM", "");
            UpdateStatus("标识码生成完毕");
            MessageBox.Show("完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private long EditFieldFromMaxNum(long maxNum, IFeatureClass writeFeatureClass, string writeField, string writeWhere)
        {
            //写入标识码或图斑编号
            int writeFieldNum = writeFeatureClass.FindField(writeField);
            IQueryFilter queryFilter = new QueryFilterClass();
            queryFilter.WhereClause = writeWhere;
            IFeatureCursor featureCursor = writeFeatureClass.Update(queryFilter, true);
            IFeature feature = featureCursor.NextFeature();
            while (feature != null)
            {
                feature.set_Value(writeFieldNum, maxNum++);
                featureCursor.UpdateFeature(feature);
                feature = featureCursor.NextFeature();
            }
            RCIS.Utility.OtherHelper.ReleaseComObject(featureCursor);
            return maxNum;
        }
    }
}
