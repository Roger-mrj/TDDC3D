using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;
using System.Windows.Forms;
using RCIS.GISCommon;
using System.IO;
using System.Collections;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Geoprocessing;
using ESRI.ArcGIS.Geoprocessor;
using System.Diagnostics;


namespace RCIS.DataInterface.VCTOut
{
    /// <summary>
    /// 到处临时面的时候，用字典表记录 线面关系，能够快速导出，不过耗费内存大，数据大的时候会溢出
    /// 2019-4-25日修改，不再用字典表记录，线输出序号采用对应独立面的层代码*一千万+序号，不会重复，面序号采用 *一万万+序号，
    /// 在通过polygon2polyline中获取线面关系的时候，就用简单的换算关系就能得到
    /// </summary>
    public class VCTOut11
    {

        //public Stopwatch sortTime = new Stopwatch();
        //public Stopwatch findTime = new Stopwatch();       

        private string tmpRootPath;

        public bool DoByAXzq = false;  //是否按行政区分开处理地类图斑

        #region 基本成员
        public int dh = 38;        
        // 独立面线对象要素代码输出 1099000000
        public IFeatureWorkspace gdbWorkspace;
        public IFeatureDataset gdbDataset; //获取控件参考用
        public List<TableStruct> allTableStruct = new List<TableStruct>();

        public IFeatureWorkspace shpWorkspace;//生成的临时的shp文件夹所在地
        private string mSeparator = ",";
       

        /// <summary>
        /// 层代码，序号用层代码+oid
        /// </summary>
        private Dictionary<string, long > dicCDM = new Dictionary<string, long >();
        private Dictionary<string, long> getCDM()
        {
            string sql = "select CLASSNAME,LayerDM from SYS_YSDM  where type in ('POINT','LINE','POLYGON') ";
            System.Data.DataTable dt = RCIS.Database.LS_SetupMDBHelper.GetDataTable(sql, "tmp");
            Dictionary<string, long> dic = new Dictionary<string, long>();
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                string sCdm=dr["LayerDM"].ToString().Trim();
                long ldm = 0;
                if (sCdm == "29A1")
                {
                    ldm =9999;
                }
                else if (sCdm == "29B1")
                {
                    ldm = 9998;
                }
                else
                {
                    long.TryParse(sCdm, out ldm);
                }
                ldm *= 1000000;  //乘以，可不重复
                dic.Add(dr["CLASSNAME"].ToString().Trim().ToUpper(), ldm);
            }
            return dic;

        }

        public VCTOut11(string tmpPath)
        {
            dicCDM = this.getCDM();
            tmpRootPath = tmpPath;
            this.shpWorkspace = WorkspaceHelper2.GetShapefileWorkspace(tmpPath) as IFeatureWorkspace;
        }


        ///// <summary>
        ///// 从标准库看是否有些没有数据，没有数据则不导出了
        ///// </summary>
        //public void ModifyAllTabs()
        //{
        //    IWorkspace2 ws2 = this.shpWorkspace as IWorkspace2;
        //    for (int i = allTableStruct.Count - 1; i >= 0; i--)
        //    {
        //        TableStruct ts = allTableStruct[i];
        //        if (!ws2.get_NameExists(esriDatasetType.esriDTFeatureClass, ts.className))
        //        {
        //            allTableStruct.RemoveAt(i);
        //        }
        //    }
        //}

        #endregion

        #region 导出文件头
        //导出图层定义
        private void ExptLayerDefine3(StreamWriter sw)
        {
            
            sw.WriteLine("FeatureCodeBegin");
            foreach (TableStruct ts in allTableStruct)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(ts.feaCode).Append(mSeparator);
                sb.Append(ts.feaName).Append(mSeparator);
                sb.Append(ts.type).Append(mSeparator);
                sb.Append(ts.className);

                sw.WriteLine(sb.ToString());

            }
            ////注记单独处理,先去掉
            Dictionary<string, string> dicZj = new Dictionary<string, string>();
            dicZj.Add("1000119000","测量控制点注记");
            dicZj.Add("1000609000", "行政区注记");
            dicZj.Add("1000608000","村级调查区注记");
            dicZj.Add("2001010200", "地类图斑注记");
            dicZj.Add("2005010900","永久基本农田注记");
            dicZj.Add("2099010200","临时用地注记");
            dicZj.Add("2099020200","批准未建设土地注记");
            dicZj.Add("2099030200","城镇村等用地注记");
            dicZj.Add("2099040200","耕地等别注记");
            dicZj.Add("2099050200","重要项目用地注记");
            dicZj.Add("2099060200","开发园区注记");
            dicZj.Add("2099070200","光伏板区注记");
            dicZj.Add("2099080200","推土区注记");
            dicZj.Add("2099090200","拆除未尽区注记");
            dicZj.Add("2099100200","路面范围注记");
            dicZj.Add("2099110200","无居民海岛注记");
            dicZj.Add("3001010200","国家公园注记");
            dicZj.Add("3001020200","自然保护区注记");
            dicZj.Add("3001030200","森林公园注记");
            dicZj.Add("3001040200","风景名胜区注记");
            dicZj.Add("3001050200","地质公园注记");
            dicZj.Add("3001060200","世界自然遗产保护区注记");
            dicZj.Add("3001070200","湿地公园注记");
            dicZj.Add("3001080200","饮用水水源地注记");
            dicZj.Add("3001090200","水产种植资源保护区注记");
            dicZj.Add("3001990200","其他类型禁止开发区注记");
            dicZj.Add("3002020000","城市开发边界注记");
            dicZj.Add("3003020000","生态保护红线注记");

            foreach(KeyValuePair<string,string> aItem in dicZj)
            {
                sw.WriteLine(aItem.Key.ToString() + mSeparator + aItem.Value.ToString() + mSeparator + "Annotation" + mSeparator + "ZJ");
            }

           
            sw.WriteLine("FeatureCodeEnd");
            sw.WriteLine();
        }
        private void ExpTableStruct3(StreamWriter sw)
        {
            int RealFieldCount = 0;  //字段数量           

            //导出表结构
            sw.WriteLine("TableStructureBegin");
            foreach (TableStruct tabs in allTableStruct)
            {
                string sTableName = tabs.className;  //表名                
                string sGraph = tabs.type;               

                IFeatureClass pFc = this.gdbWorkspace.OpenFeatureClass(sTableName);
                ArrayList arFields = new ArrayList();
                RealFieldCount = 0; //计算真实输出的字段个数
                for (int i = 0; i < pFc.Fields.FieldCount; i++)
                {
                    IField field = pFc.Fields.get_Field(i);
                    if (field.Type == esriFieldType.esriFieldTypeGlobalID ||
                        field.Type == esriFieldType.esriFieldTypeOID ||
                        field.Type == esriFieldType.esriFieldTypeGeometry ||
                        field.Type == esriFieldType.esriFieldTypeGUID 
                        )
                        continue;

                    string fieldName = field.Name.ToUpper();
                    if ((fieldName == "OBJECTID") || (fieldName=="FID") || (fieldName.StartsWith("SHAPE")))
                    {
                        continue;
                    }
                    RealFieldCount++;

                    StringBuilder sb = new StringBuilder();
                    sb.Append(fieldName).Append(mSeparator);//+字段名
                    #region Field Type
                    switch (field.Type)
                    {
                        case esriFieldType.esriFieldTypeDate:
                            sb.Append("Date").Append(mSeparator);
                            sb.Append(field.Length);
                            break;

                        case esriFieldType.esriFieldTypeDouble:
                            if (fieldName.ToUpper() == "KCXS")
                            {
                                sb.Append("Float").Append(mSeparator);
                                sb.Append(6).Append(mSeparator);
                                sb.Append(4);
                            }
                            else
                            {
                                sb.Append("Float").Append(mSeparator);
                                sb.Append(15).Append(mSeparator);
                                sb.Append(2);
                            }
                            break;
                        case
                            esriFieldType.esriFieldTypeInteger:
                            sb.Append("Int").Append(field.Length);
                            //sb.Append(field.Length.ToString());
                            break;
                        case esriFieldType.esriFieldTypeSmallInteger:
                            sb.Append("Int").Append(field.Length);
                            //sb.Append(field.Length.ToString());
                            break;
                        case esriFieldType.esriFieldTypeString:
                            sb.Append("Char").Append(mSeparator);
                            sb.Append(field.Length).ToString();
                            break;
                        case esriFieldType.esriFieldTypeSingle:
                            sb.Append("Float").Append(mSeparator);
                            sb.Append(15).Append(mSeparator);
                            sb.Append(2);
                            break;
                        case esriFieldType.esriFieldTypeBlob:
                            sb.Append("Varbin").Append(mSeparator);
                            break;
                        default:
                            break;
                    }
                    #endregion

                    arFields.Add(sb.ToString());
                }
                //表名 ，字段个数
                sw.WriteLine(new StringBuilder(sTableName).Append(mSeparator).Append(RealFieldCount.ToString()));
                foreach (string str in arFields)
                {
                    sw.WriteLine(str);
                }
                sw.WriteLine(0); //以0 结束
                sw.WriteLine();

                RCIS.Utility.OtherHelper.ReleaseComObject(pFc);
            }

            

            //注记单独处理，先略过
            sw.WriteLine("ZJ,15");
            sw.WriteLine("BSM,Char,18");
            sw.WriteLine("YSDM,Char,10");
            sw.WriteLine("ZJNR,Char,60");
            sw.WriteLine("ZT,Char,4");
            sw.WriteLine("YS,Char,12");
            sw.WriteLine("BS,Int4");
            sw.WriteLine("XZ,Char,1");
            sw.WriteLine("XHX,Char,1");
            sw.WriteLine("KD,Float,15");
            sw.WriteLine("GD,Float,15");
            sw.WriteLine("JG,Float,6");
            sw.WriteLine("ZJDZXJXZB,Float,15");
            sw.WriteLine("ZJDZXJYZB,Float,15");
            sw.WriteLine("ZJFX,Float,10");
            sw.WriteLine("BZ,VarChar");
            sw.WriteLine(0);
            
            sw.WriteLine("TableStructureEnd");
            sw.WriteLine();
        }
        public void ExportFileHead3()
        {
            string fileName = RCIS.Global.AppParameters.VCTOut_TMP + "\\0BEGIN.VCT";  //Application.StartupPath + "\\VCTEX\\0BEGIN.VCT";
            StreamWriter sw = new StreamWriter(fileName, false, Encoding.GetEncoding("GB2312"));
            try
            {
                #region 输出文件头
                double maxX, maxY, minX, minY;

                ISpatialReference sr = (this.gdbDataset as IGeoDataset).SpatialReference;
                IProjectedCoordinateSystem prjSR = sr as IProjectedCoordinateSystem;
                IGeographicCoordinateSystem gcs = prjSR.GeographicCoordinateSystem;

                sr.GetDomain(out minX, out maxX, out minY, out maxY);

                sw.WriteLine("HeadBegin");
                sw.WriteLine("Datamark: LANDUSE-VCT");
                sw.WriteLine("Version:3.0");
                sw.WriteLine("CoordinateSystemType:P");
                sw.WriteLine("Dim:2");
                sw.WriteLine("XAxisDirection:E");
                sw.WriteLine("YAxisDirection:N");
                sw.WriteLine("XYUnit:M");
                sw.WriteLine("ZUnit:M");
                sw.WriteLine("Spheroid:CGCS2000,6378137.0,298.257222101");
                sw.WriteLine("PrimeMeridian:Greenwich");
                sw.WriteLine("Projection:高斯-克吕格投影");
                sw.WriteLine("Parameters:"+(3*dh).ToString()+",1," + prjSR.FalseEasting + "," + prjSR.FalseNorthing + ",3," + dh);
                sw.WriteLine("VerticalDatum:1985国家高程基准");
                sw.WriteLine("TemporalReferenceSystem:北京时间");
                sw.WriteLine("ExtentMin:" + minX.ToString() + "," + minY.ToString());
                sw.WriteLine("ExtentMax:" + maxX.ToString() + "," + minY.ToString());
                sw.WriteLine("MapScale:2000");
                sw.WriteLine("Offset:0,0");
                DateTime now = DateTime.Now;
                string aYear = now.Year.ToString();
                string aMonth = now.Month.ToString().PadLeft(2, '0');
                string aDay = now.Day.ToString().PadLeft(2, '0');
                sw.WriteLine("Date:" + aYear + aMonth + aDay);
                sw.WriteLine("Separator:,");
                sw.WriteLine("HeadEnd");
                sw.WriteLine();
                #endregion
                ExptLayerDefine3(sw);

                ExpTableStruct3(sw);

            }
            finally
            {
                sw.Flush();
                sw.Close();
            }
        }
        #endregion

        #region 输出点数据
        private void ExportPointClass(TableStruct ts, StreamWriter writer)
        {
            string className = ts.className;
            long start = this.dicCDM[className];

            //还是从数据库中读取
            IFeatureClass aPointClass = null;
            try
            {
                aPointClass=this.shpWorkspace.OpenFeatureClass(className.ToUpper());
            }
            catch { }
            if (aPointClass == null) return;
            if (aPointClass.FeatureCount(null) == 0) return;

            //开始导出
            //标识码 ， 要素代码，图形样式编码，点特征类型，点数 ，
            IFeatureCursor pCusor = aPointClass.Search(null, true);
            IFeature aPoint = null;
            try
            {                
                while ((aPoint=pCusor.NextFeature())!=null)
                {
                    IPoint aPtGeom = aPoint.ShapeCopy as IPoint;
                    if (aPtGeom != null && !aPtGeom.IsEmpty)
                    {
                        #region 输出到文件中
                       
                        double px = aPtGeom.X;
                        double py = aPtGeom.Y;
                        long gid = start*10 + aPoint.OID;

                        writer.WriteLine(gid);
                        writer.WriteLine(ts.feaCode);
                        writer.WriteLine("unknown");//图行样式编码
                        writer.WriteLine(1);  //点特征类型
                        writer.WriteLine(1);  //点数
                        writer.WriteLine(px + this.mSeparator + py);

                        writer.WriteLine(0); //输出一个 0 
                        writer.WriteLine();
                        #endregion 
                    }
                }
            }
            catch (Exception e)
            {
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCusor);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(aPointClass);
            }



        }
        public void ExportPoint3()
        {
            string fileName = RCIS.Global.AppParameters.VCTOut_TMP + "\\1POINT.vct";

            #region 输出点数据
            StreamWriter sw = new StreamWriter(fileName, false, Encoding.GetEncoding("GB2312"));
            try
            {
                sw.WriteLine("PointBegin");
                foreach (TableStruct ts in allTableStruct)
                {
                    string type = ts.type.ToUpper();
                    if (type.Equals("POINT"))
                    {
                        this.ExportPointClass(ts, sw);
                    }
                }
                sw.WriteLine("PointEnd");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sw.Flush();
                sw.Close();
            }
            #endregion
        }
        #endregion

        private void ExportAnnotationTable(StreamWriter mWriter2)
        {
            IFeatureClass aClass = null;
            try
            {
                aClass = this.shpWorkspace.OpenFeatureClass("ZJ");
            }
            catch (Exception ex) { }
            if (aClass == null)
                return;
            int count = aClass.FeatureCount(null);
            if (count <= 0)
                return;

            ICursor aCursor = aClass.Search(null, true) as ICursor;
            try
            {
                #region 导出属性表内容
                IRow aRow = aCursor.NextRow();
                int irow = 0;
                while (aRow != null)
                {
                    irow++;
                    mWriter2.WriteLine(FeatureHelper.GetRowValue(aRow, "BSM").ToString());
                    mWriter2.WriteLine(FeatureHelper.GetRowValue(aRow, "YSDM").ToString());
                    mWriter2.WriteLine("Unknown");
                    mWriter2.WriteLine("1");//单点注记
                    mWriter2.WriteLine(FeatureHelper.GetRowValue(aRow, "ZJNR").ToString());
                    IPoint thisPt = (aRow as IFeature).Shape as IPoint;
                    mWriter2.WriteLine(thisPt.X + "," + thisPt.Y + "," + FeatureHelper.GetRowValue(aRow, "ZJFX").ToString());
                    mWriter2.WriteLine(0);

                    aRow = aCursor.NextRow();
                }
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                RCIS.Utility.OtherHelper.ReleaseComObject(aCursor);
                RCIS.Utility.OtherHelper.ReleaseComObject(aClass);
            }
        }

        //导出Anno
        public void ExportAnotation3()
        {
            string fileName = Application.StartupPath + @"\VCTEX\4ANNO.vct";  //lstFiles[4].ToString();
            StreamWriter sw = new StreamWriter(fileName, false, Encoding.GetEncoding("GB2312"));
            try
            {
                #region 输出注记

                sw.WriteLine("AnnotationBegin");
                this.ExportAnnotationTable(sw);

                sw.WriteLine("AnnotationEnd");
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sw.Flush();
                sw.Close();
            }
        }

        #region 导出属性表
        public void ExportAttribute3()
        {
            string fileName = RCIS.Global.AppParameters.VCTOut_TMP + "\\5attribute.vct";
            StreamWriter sw = new StreamWriter(fileName, false, Encoding.GetEncoding("GB2312"));
            try
            {
                sw.WriteLine("AttributeBegin");
                foreach (TableStruct ts in allTableStruct)
                {
                    ExportAttributeTable(ts, sw);
                }
              
                sw.Write("AttributeEnd");
                sw.WriteLine();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sw.Flush();
                sw.Close();
            }
        }
        private void ExportAttributeTable(IFeatureClass aClass, string pTableName, ISpatialFilter pFilter, StreamWriter mWriter2)
        {
            long start = this.dicCDM[pTableName];

            ArrayList fields = VCTOutPublic.getAttrFields(gdbWorkspace, pTableName);
            int icount = aClass.FeatureCount(pFilter);
            IFeatureCursor aCursor = aClass.Search(pFilter, true) ;
            try
            {
                #region 导出属性表内容
                IFeature aFeature = null;
                          
                while ((aFeature=aCursor.NextFeature())!=null)
                {                    
                   
                    StringBuilder aBuilder = new StringBuilder();   
                    long xh = start * 10 + aFeature.OID;
                    //先输出序号
                    aBuilder.Append(xh).Append(this.mSeparator); //输出序号                  
                    foreach (string sField in fields)
                    {
                       
                        int idx = aCursor.FindField(sField);
                        if (idx >= 0)
                        {
                            //郭杰++ 090825,替换掉内容里面的,
                            string content = aFeature.get_Value(idx).ToString();                          

                            if (content.EndsWith("0:00:00"))
                            {
                                content = content.Substring(0, content.Length - 7);
                                try
                                {
                                    content = DateTime.Parse(content).ToString("yyyyMMdd");
                                }
                                catch
                                {
                                }
                            }
                            content = content.Replace(',', '，');
                            content = content.Replace("\r\n", " ");
                            aBuilder.Append(content).Append(this.mSeparator);
                            //guojie++++++++++++++++++++
                        }
                        else
                        {
                            aBuilder.Append(this.mSeparator);
                        }
                    }
                    if (aBuilder.Length > 0) aBuilder.Remove(aBuilder.Length - 1, 1);
                    String aText = aBuilder.ToString();
                    mWriter2.WriteLine(aText);
                    
                }
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                RCIS.Utility.OtherHelper.ReleaseComObject(aCursor);
            }
        }

        private void ExportAttributeTable(TableStruct pLayer, StreamWriter mWriter2)
        {            

            IFeatureClass aClass = null;
            try
            {
                aClass = this.shpWorkspace.OpenFeatureClass(pLayer.className);
              
            }
            catch (Exception ex) { }
            if (aClass == null)
                return;

            #region 输出主表       

            int count = aClass.FeatureCount(null);
            if (count <= 0)
                return;
            mWriter2.WriteLine(pLayer.className);
            this.ExportAttributeTable(aClass, pLayer.className, null, mWriter2);

            #endregion

            mWriter2.WriteLine("TableEnd");




        }
           


        #endregion

        #region 导出线

        //输出真实线 
        private void ExportLineClass(TableStruct pLayer, StreamWriter writer)
        {
            string className = pLayer.className;
            //还是采用从数据库中读取,这样快一点
            IFeatureClass aClass = null;
            try
            {
                aClass = this.shpWorkspace.OpenFeatureClass(pLayer.className); 
            }
            catch (Exception cex)
            {
            }
            if (aClass == null) return;
            if (aClass.FeatureCount(null) == 0) return;

            long lStart = dicCDM[pLayer.className];
            //开始导出数据
            IFeature aFeature = null;
            IFeatureCursor pCursor = aClass.Search(null, true);

            try
            {
                while ((aFeature=pCursor.NextFeature())!=null)    
                {
                    IPolyline aLineGeom = aFeature.ShapeCopy as IPolyline;
                    #region writelin
                    long gid = lStart*10 + aFeature.OID;
                    
                    IPointCollection aPtCol = aLineGeom as IPointCollection;
                    writer.WriteLine(gid);
                    writer.WriteLine(pLayer.feaCode);  //要素代码
                    writer.WriteLine("unknown"); //图形样式编码
                    writer.WriteLine(1);  //特征类型                   
                    writer.WriteLine(1); //线段个数
                    writer.WriteLine(11); //折线
                    writer.WriteLine(aPtCol.PointCount); //点数
                    for (int pi = 0; pi < aPtCol.PointCount; pi++)
                    {
                        IPoint aPt = aPtCol.get_Point(pi);
                        double px = aPt.X;
                        double py = aPt.Y;
                        writer.WriteLine(px + this.mSeparator + py);
                    }
                    writer.WriteLine(0);  //输出一个 0 ，结束
                    writer.WriteLine();
                    #endregion 
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(aClass);
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }


        }


       ///// <summary>
       // /// 通过dltbline与CJDCQ的空间关系输出宗地
       ///// </summary>
       ///// <param name="pLayer"></param>
       ///// <param name="dltbStart">此处 还是采用dltb的层代码，因为dltbline 输出的时候就是以dltb层代码*1000万开头的，</param>
       ///// <param name="plineClass"></param>
       // private void ExportTmpLineByPolygon2(TableStruct pLayer,long dltbStart, IFeatureClass plineClass)
       // {
            
       //     string fillClassName = pLayer.className.ToUpper();
       //     long currLStart = this.dicCDM[pLayer.className.ToUpper()];  //村级调查区起始id
           
       //     IFeatureClass aFillClass = null;
       //     string ZdShpFile = this.tmpRootPath + "\\" + pLayer.className.ToUpper() + ".shp";
       //     if (!System.IO.File.Exists(ZdShpFile))
       //     {
       //         return;
       //     }
       //     try
       //     {
       //         aFillClass = this.shpWorkspace.OpenFeatureClass(pLayer.className.ToUpper());
       //     }
       //     catch { }
       //     if (aFillClass==null) return;
       //     if (aFillClass.FeatureCount(null) == 0) return;

       //     #region 独立面得数据要输出到面文件里面去
       //     Encoding gb2312 = Encoding.GetEncoding("GB2312");
       //     StreamWriter mWriter2 = null;
       //     string pFilePath = Application.StartupPath + "\\VCTEX\\3Fill.VCT";
       //     System.IO.FileStream fs = null;
       //     if (File.Exists(pFilePath))
       //     {
       //         //如果存在，则追加
       //         mWriter2 = new StreamWriter(pFilePath, true, gb2312);
       //     }
       //     else
       //     {
       //         //没有，则创建
       //         fs = File.Create(pFilePath, 64 * 1024);
       //         mWriter2 = new StreamWriter(fs, gb2312);
       //         mWriter2.WriteLine("PolygonBegin");
       //     }
       //     mWriter2.WriteLine("");
       //     #endregion
            
       //     IQueryFilter pQF = new QueryFilterClass();
       //     IFeature aFill = null;
       //     IFeatureCursor pCusor = aFillClass.Search(pQF, true);
       //     try
       //     {
       //         while ((aFill = pCusor.NextFeature()) != null)
       //         {
                   
       //             List<LineObject> lineObjList = new List<LineObject>();
       //             lineObjList = VCToutTmpLineHelper.getJxObjByPolygon3(aFill.Shape, dltbStart, plineClass, 0);                                
       //             if (lineObjList.Count == 0)
       //             {
       //                 continue;
       //             }

       //             List<long> lineOidList = VCToutTmpLineHelper.OrderEdges(lineObjList);
                  
       //             //面的id *10
       //             mWriter2.WriteLine(currLStart*10 + aFill.OID);  //bsm
       //             mWriter2.WriteLine(pLayer.feaCode);  //要素代码
       //             mWriter2.WriteLine("unkown"); //图形表现编码
       //             mWriter2.WriteLine(100); // 面的特征类型
       //             IPoint label = (aFill.ShapeCopy as IArea).Centroid;
       //             double px = Math.Round(label.X, 6);
       //             double py = Math.Round(label.Y, 6);
       //             mWriter2.WriteLine(px.ToString("F6") + "," + py.ToString("F6"));    //表示点坐标
       //             mWriter2.WriteLine(21); //表示间接坐标面
       //             mWriter2.WriteLine(Convert.ToString(lineOidList.Count));  //输出线个数
       //             VCTOutPublic.ExportLineIDs(lineOidList, mWriter2); //输出线的id
       //             mWriter2.WriteLine(0);  //输出1个 0 ，标识结束
       //             mWriter2.WriteLine();
       //             lineObjList.Clear();
       //         }

       //         GC.Collect();
       //     }
       //     catch (Exception ex)
       //     {
       //     }
       //     finally
       //     {
       //         System.Runtime.InteropServices.Marshal.ReleaseComObject(pCusor);
       //         System.Runtime.InteropServices.Marshal.ReleaseComObject(aFillClass);
       //         mWriter2.Close();
       //         GC.Collect();
       //         GC.WaitForPendingFinalizers();
       //     }

       // }

      
        /// <summary>
        /// 输出临时线
        /// </summary>
        /// <param name="pLayer"></param>
        /// <param name="writer"></param>
        private void ExportTmpLineClass(TableStruct pLayer, StreamWriter writer)
        {
            string className = pLayer.className; 
            string lineFile = this.tmpRootPath + "\\" + className.ToUpper() + "line.shp";
            if (!System.IO.File.Exists(lineFile))
            {
                //不存在
                return;
            }

            long lStart = this.dicCDM[className];  //临时线 用dltb图层开头的 号            
            IFeatureClass tmpLineClass = null;
            try
            {
                tmpLineClass = (shpWorkspace as IFeatureWorkspace).OpenFeatureClass(className.ToUpper() + "line");
            }
            catch (Exception ex)
            {
            }
            if (tmpLineClass == null) return;
            if (tmpLineClass.FeatureCount(null) == 0) return;
            //输出临时线          
            IFeatureCursor pCursor = tmpLineClass.Search(null, true);
            IFeature aLine = null;         
            try
            {
                while ((aLine = pCursor.NextFeature()) != null)
                {
                    long gid = lStart + aLine.OID;  //输出线的时候用地类图斑的层代码+id，应该不重复

                    #region 输出线
                    writer.WriteLine(gid);
                    writer.WriteLine("1099000000");
                    writer.WriteLine("unknown");
                    writer.WriteLine(1);  //特征类型
                    writer.WriteLine(1); //线段个数
                    writer.WriteLine(11); //折线
                    IPointCollection aPtCol = aLine.Shape as IPointCollection;
                    int aPtCount = aPtCol.PointCount;
                    writer.WriteLine(aPtCount);
                    for (int pi = 0; pi < aPtCount; pi++)
                    {
                        IPoint aPt = aPtCol.get_Point(pi);
                        double px = aPt.X;
                        double py = aPt.Y;
                        writer.WriteLine(px + this.mSeparator + py);
                    }

                    writer.WriteLine(0);  //输出一个 0 ，结束
                    writer.WriteLine();
                    #endregion
                 
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
            }
            

            //紧跟着输出面
            try
            {
                //if (pLayer.className == "DLTB")
                //{
                //    ExportTmpLineByPolygon(pLayer, tmpLineClass);
                //    //同时把 ZD的输出出来
                //    foreach (TableStruct zdTs in this.allTableStruct)
                //    {
                //        if (zdTs.className.ToUpper() == "CJDCQ")
                //        {                            
                //            ExportTmpLineByPolygon2(zdTs, lStart, tmpLineClass);
                //        }
                //    }
                //}
                //else
                //{
                    ExportTmpLineByPolygon(pLayer, tmpLineClass);
                //}
            }
            catch (Exception ex)
            {
            }
          
            System.Runtime.InteropServices.Marshal.ReleaseComObject(tmpLineClass);           
            GC.Collect();
        }


        private void ExportTmpLinePDT(TableStruct pLayer, IFeatureClass aLineClass)
        {
            #region 控制条件
            IFeatureClass aFillClass = null;
            string dltbFile = this.tmpRootPath + "\\" + pLayer.className.ToUpper() + ".shp";
            if (!System.IO.File.Exists(dltbFile))
            {
                return;
            }
            try
            {
                aFillClass = shpWorkspace.OpenFeatureClass(pLayer.className.ToUpper());
            }
            catch { }
            if (aFillClass == null) return;
            if (aFillClass.FeatureCount(null) == 0) return;


            long lstart = this.dicCDM[pLayer.className]; //层代码
            #endregion

            //只要把dicDltblines中的对象输出即可
            #region 独立面得数据要输出到面文件里面去
            Encoding gb2312 = Encoding.GetEncoding("GB2312");
            StreamWriter mWriter2 = null;
            string pFilePath = Application.StartupPath + "\\VCTEX\\3Fill.VCT";
            System.IO.FileStream fs = null;

            if (File.Exists(pFilePath))
            {
                //如果存在，则追加
                //mWriter2 = File.AppendText(pFilePath);
                mWriter2 = new StreamWriter(pFilePath, true, gb2312);
            }
            else
            {
                //没有，则创建
                fs = File.Create(pFilePath, 64 * 1024);
                mWriter2 = new StreamWriter(fs, gb2312);
                mWriter2.WriteLine("PolygonBegin");
            }
            mWriter2.WriteLine("");
            #endregion
            

            #region 整个处理
            //全部            
            IFeature aFill = null;
            IFeatureCursor pCursor = aFillClass.Search(null , true);
            try
            {
                while ((aFill = pCursor.NextFeature()) != null)
                {
                    //面的id *10，
                    long fid = aFill.OID;

                    List<LineObject> lineObjList = VCToutTmpLineHelper.getPolygonJxBySx(aLineClass, lstart, fid);
                    fid += lstart * 10;

                    IGeometryCollection pGeoCols = aFill.Shape as IGeometryCollection;
                    List<long> lineOidList = new List<long>();
                    if (pGeoCols.GeometryCount == 1)
                    {

                        lineOidList = VCToutTmpLineHelper.OrderEdges(lineObjList,aFill.Shape);
                    }
                    else if (pGeoCols.GeometryCount > 1)
                    {
                        lineOidList = VCToutTmpLineHelper.OrderEdges(lineObjList, aFill.Shape);
                    }


                    #region 输出

                    mWriter2.WriteLine(fid.ToString());  //bsm
                    mWriter2.WriteLine(pLayer.feaCode);  //要素代码

                    mWriter2.WriteLine("unkown"); //图形表现编码
                    mWriter2.WriteLine(100); // 面的特征类型

                    IPoint label = (aFill.ShapeCopy as IArea).Centroid;
                    double px = Math.Round(label.X, 6);
                    double py = Math.Round(label.Y, 6);
                    mWriter2.WriteLine(px.ToString("F6") + "," + py.ToString("F6"));    //表示点坐标
                    mWriter2.WriteLine(21); //表示间接坐标面
                    mWriter2.WriteLine(Convert.ToString(lineOidList.Count));  //输出线个数
                    VCTOutPublic.ExportLineIDs(lineOidList, mWriter2);
                    mWriter2.WriteLine(0);  //输出1个 0 ，标识结束
                    mWriter2.WriteLine();

                    #endregion
                    lineObjList.Clear();
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            #endregion
            

            mWriter2.Close();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(aFillClass);
        }
             

        private void ExportTmpLineByPolygon(TableStruct pLayer, IFeatureClass aLineClass)
        {
            #region 控制条件
            IFeatureClass aFillClass = null;
            string dltbFile = this.tmpRootPath + "\\" + pLayer.className.ToUpper() + ".shp";
            if (!System.IO.File.Exists(dltbFile))
            {
                return;
            }
            try
            {
                aFillClass = shpWorkspace.OpenFeatureClass(pLayer.className.ToUpper());
            }
            catch { }
            if (aFillClass == null) return;
            if (aFillClass.FeatureCount(null) == 0) return;

            //是否需要分乡处理
            IFeatureClass XZQClass = null;
            try
            {
                XZQClass = this.shpWorkspace.OpenFeatureClass("XZQ");
            }
            catch { }


            long lstart = this.dicCDM[pLayer.className]; //层代码
           // lstart *= 100;
            #endregion 

            //只要把dicDltblines中的对象输出即可
            #region 独立面得数据要输出到面文件里面去
            Encoding gb2312 = Encoding.GetEncoding("GB2312");
            StreamWriter mWriter2 = null;
            string pFilePath = Application.StartupPath + "\\VCTEX\\3Fill.VCT";
            System.IO.FileStream fs = null;

            if (File.Exists(pFilePath))
            {
                //如果存在，则追加
                //mWriter2 = File.AppendText(pFilePath);
                mWriter2 = new StreamWriter(pFilePath, true, gb2312);
            }
            else
            {
                //没有，则创建
                fs = File.Create(pFilePath, 64 * 1024);
                mWriter2 = new StreamWriter(fs, gb2312);
                mWriter2.WriteLine("PolygonBegin");
            }
            mWriter2.WriteLine("");
            #endregion


            try
            {
                if ((pLayer.className.ToUpper() == "DLTB") && (XZQClass != null) && (XZQClass.FeatureCount(null) > 0) && (this.DoByAXzq))
                {
                    #region 分xzq处理
                    //下面要以乡 为单位 输出 dltb，因为一次加载会溢出
                    IFeatureLayer xzqLayer = new FeatureLayerClass();
                    xzqLayer.FeatureClass = XZQClass;
                    IIdentify identifyXzq = xzqLayer as IIdentify;
                    IArray arXzqs = identifyXzq.Identify((XZQClass as IGeoDataset).Extent);
                    for (int i = 0; i < arXzqs.Count; i++)
                    {
                        //获取每个乡范围对应的图形，进行空间查询
                        IFeatureIdentifyObj obj = arXzqs.get_Element(i) as IFeatureIdentifyObj;
                        IRowIdentifyObject aRow = obj as IRowIdentifyObject;
                        IFeature aXiang = aRow.Row as IFeature;
                        IGeometry aXiangGeo = aXiang.Shape;

                        //获取 乡 下所有 临时界线 层  数据
                       // findTime.Start();
                        Dictionary<long, List<LineObject>> dicDltbLineObj = VCToutTmpLineHelper.getDltbLineObjs(aLineClass, lstart, aXiangGeo);
                      //  findTime.Stop();

                        #region   //输出乡下面所有地类图斑
                        ISpatialFilter pSF = new SpatialFilterClass();
                        pSF.Geometry = aXiangGeo;
                        pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;                        
                        pSF.SubFields = "BSM," + aFillClass.ShapeFieldName;
                        IFeature aFill = null;

                        IFeatureCursor pCursor = aFillClass.Search(pSF as IQueryFilter, true);
                        try
                        {
                            while ((aFill = pCursor.NextFeature()) != null)
                            {
                                try
                                {
                                    string fillbsm = FeatureHelper.GetFeatureStringValue(aFill, "BSM");

                                    if (fillbsm == "140322211000006557" || fillbsm == "140322211000020621" || fillbsm == "140322211000019217" || fillbsm == "140322211000059480")
                                    {
                                    }
                                   
                                    //面的id *10，
                                    long fid = aFill.OID;
                                    fid += lstart * 10;
                                    if (!dicDltbLineObj.ContainsKey(fid))
                                    {
                                        continue;
                                    }

                                    List<LineObject> lineObjList = dicDltbLineObj[fid];
                                    IGeometryCollection pGeoCols = aFill.Shape as IGeometryCollection;
                                    List<long> lineOidList = new List<long>();

                                   // sortTime.Start();
                                    if (pGeoCols.GeometryCount == 1)
                                    {
                                        lineOidList = VCToutTmpLineHelper.OrderEdges(lineObjList,aFill.Shape);
                                    }
                                    else if (pGeoCols.GeometryCount > 1)
                                    {
                                        lineOidList = VCToutTmpLineHelper.OrderEdgesRing(lineObjList, aFill.Shape);
                                    }
                                   // sortTime.Stop();

                                    #region 输出
                                    mWriter2.WriteLine(fid.ToString());  //bsm
                                    mWriter2.WriteLine(pLayer.feaCode);  //要素代码
                                    mWriter2.WriteLine("unkown"); //图形表现编码
                                    mWriter2.WriteLine(100); // 面的特征类型

                                    IPoint label = (aFill.ShapeCopy as IArea).Centroid;
                                    double px = Math.Round(label.X, 6);
                                    double py = Math.Round(label.Y, 6);
                                    mWriter2.WriteLine(px.ToString("F6") + "," + py.ToString("F6"));    //表示点坐标
                                    mWriter2.WriteLine(21); //表示间接坐标面
                                    mWriter2.WriteLine(Convert.ToString(lineOidList.Count));  //输出线个数

                                    VCTOutPublic.ExportLineIDs(lineOidList, mWriter2);
                                    mWriter2.WriteLine(0);  //输出1个 0 ，标识结束

                                    #endregion
                                    lineObjList.Clear();
                                    dicDltbLineObj.Remove(fid);
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                        finally
                        {
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(pSF);
                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                        }
                        #endregion

                    }
                    #endregion
                }
                else
                {
                    #region 整个处理
                    //全部
                   
                    Dictionary<long, List<LineObject>> dicDltbLineObj = VCToutTmpLineHelper.getDltbLineObjs(aLineClass, lstart, null);
                    IFeature aFill = null;
                    IFeatureCursor pCursor = aFillClass.Search(null, true);
                    try
                    {
                        while ((aFill = pCursor.NextFeature()) != null)
                        {

                            string fillBsm = FeatureHelper.GetFeatureStringValue(aFill, "BSM");
                            

                            //面的id *10，
                            long fid = aFill.OID;
                            fid += lstart * 10;

                            if (!dicDltbLineObj.ContainsKey(fid))
                            {
                                continue;
                            }
                            List<LineObject> lineObjList = dicDltbLineObj[fid];

                            IGeometryCollection pGeoCols = aFill.Shape as IGeometryCollection;
                            List<long> lineOidList = new List<long>();
                            if (pGeoCols.GeometryCount == 1)
                            {

                                lineOidList = VCToutTmpLineHelper.OrderEdges(lineObjList, aFill.Shape);
                            }
                            else if (pGeoCols.GeometryCount > 1)
                            {
                                lineOidList = VCToutTmpLineHelper.OrderEdgesRing(lineObjList, aFill.Shape);
                            }


                            #region 输出

                            mWriter2.WriteLine(fid.ToString());  //bsm
                            mWriter2.WriteLine(pLayer.feaCode);  //要素代码

                            mWriter2.WriteLine("unkown"); //图形表现编码
                            mWriter2.WriteLine(100); // 面的特征类型

                            IPoint label = (aFill.ShapeCopy as IArea).Centroid;
                            double px = Math.Round(label.X, 6);
                            double py = Math.Round(label.Y, 6);
                            mWriter2.WriteLine(px.ToString("F6") + "," + py.ToString("F6"));    //表示点坐标
                            mWriter2.WriteLine(21); //表示间接坐标面
                            mWriter2.WriteLine(Convert.ToString(lineOidList.Count));  //输出线个数
                            VCTOutPublic.ExportLineIDs(lineOidList, mWriter2);
                            mWriter2.WriteLine(0);  //输出1个 0 ，标识结束
                            mWriter2.WriteLine();

                            #endregion
                            lineObjList.Clear();
                            dicDltbLineObj.Remove(fid);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    finally
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                    }
                    #endregion
                }                            

            }
            catch (Exception cex)
            {
            }
            finally
            {
                mWriter2.Close();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(aFillClass);
            }
                       
            
           
        }
       
        public void ExportLine3()
        {
           

            string fileName = RCIS.Global.AppParameters.VCTOut_TMP + "\\2LINE.vct"; //lstFiles[2].ToString();
            StreamWriter sw = new StreamWriter(fileName, false, Encoding.GetEncoding("GB2312"));
            #region 输出线数据
            try
            {
                sw.WriteLine("LineBegin");
                foreach (TableStruct ts in allTableStruct)
                {
                    string gt = ts.type.ToUpper();
                    if (gt.Equals("LINE"))
                    {
                        this.ExportLineClass(ts, sw);
                    }
                    else if (gt.Equals("POLYGON"))
                    {
                        //输出其他 面转换来的线层，XZQ不需要
                       // if (ts.className.ToUpper() != "PDT") 
                       // {                            
                            //输出临时线层
                            ExportTmpLineClass(ts, sw);

                       // }                       
                    }
                }
                
                sw.WriteLine("LineEnd");
                sw.WriteLine();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sw.Flush();
                sw.Close();
            }

            #endregion
        }

        #endregion 


        #region 导出面数据       

        //private void ExportXZQ(TableStruct pLayer, StreamWriter mWriter2)
        //{


        //    IFeatureClass xzqClass = null;
        //    IFeatureClass xzqJxClass = null;
        //    try
        //    {
        //        xzqClass = this.shpWorkspace.OpenFeatureClass("XZQ");
        //        xzqJxClass = this.shpWorkspace.OpenFeatureClass("XZQJX");
        //    }
        //    catch
        //    {
        //        return;
        //    }
        //    if (xzqClass.FeatureCount(null) == 0) return;

        //    long jxStart = dicCDM["XZQJX"];
        //    long xzqStart = dicCDM["XZQ"];
            
        //    IFeatureCursor pFeaCursor = xzqClass.Search(null, true);
        //    IFeature aFill = null;
        //    try
        //    {
        //        while ((aFill = pFeaCursor.NextFeature()) != null)
        //        {

        //            List<LineObject> lineObjList = VCToutTmpLineHelper.getJxObjByPolygon3(aFill.Shape, xzqJxClass, jxStart);
        //            List<long> lineBsmList = VCToutTmpLineHelper.OrderEdges(lineObjList);

        //            #region 输出到文件中
        //            mWriter2.WriteLine(xzqStart * 10 + aFill.OID);  //序号

        //            mWriter2.WriteLine(pLayer.feaCode);  //要素代码
        //            mWriter2.WriteLine("unkown"); //图形表现编码
        //            mWriter2.WriteLine(100); // 面的特征类型
        //            IPoint label = (aFill.ShapeCopy as IArea).Centroid;
        //            double px = Math.Round(label.X, 6);
        //            double py = Math.Round(label.Y, 6);
        //            mWriter2.WriteLine(px.ToString("F6") + "," + py.ToString("F6"));    //表示点坐标
        //            mWriter2.WriteLine(21); //表示间接坐标面
        //            mWriter2.WriteLine(Convert.ToString(lineBsmList.Count));  //输出线个数
        //            VCTOutPublic.ExportLineIDs(lineBsmList, mWriter2);
        //            mWriter2.WriteLine(0);  //输出1个 0 ，标识结束
        //            mWriter2.WriteLine();
        //            #endregion 

        //            lineObjList.Clear();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    finally
        //    {
        //        System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeaCursor);
        //        System.Runtime.InteropServices.Marshal.ReleaseComObject(xzqClass);
        //        System.Runtime.InteropServices.Marshal.ReleaseComObject(xzqJxClass);
        //    }



        //}


        public void ExportFill3()
        {
            string fileName = Application.StartupPath + @"\VCTEX\3Fill.vct";    //lstFiles[3].ToString();
            bool isHave = File.Exists(fileName);
            StreamWriter sw = new StreamWriter(fileName, true, Encoding.GetEncoding("GB2312"));
            try
            {

                if (!isHave)
                {
                    sw.WriteLine("PolygonBegin");
                }
                //foreach (TableStruct ts in allTableStruct)
                //{
                //    string gt = ts.type.ToUpper();
                //    string tabName = ts.className;

                //    //去掉XZQ和XZQJX拓扑关系
                //    //if (gt.Equals("POLYGON") && ts.className.ToUpper() == "XZQ")
                //    //{
                //    //    ExportXZQ(ts, sw);
                //    //}
                //}
                sw.WriteLine("PolygonEnd");
                sw.WriteLine();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sw.Flush();
                sw.Close();
            }
        }
        #endregion


    }
}
