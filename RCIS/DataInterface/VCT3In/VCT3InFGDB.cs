using System;
using System.Collections.Generic;
using System.Collections;

using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;

namespace RCIS.DataInterface.VCT3In
{
    public partial class VCT3InFGDB : Form
    {
        public VCT3InFGDB()
        {
            InitializeComponent();
        }
        private void UpdateStatus(string txt)
        {
            memoLog.Text += "\r\n" + DateTime.Now.ToString() + ":" + txt;
            Application.DoEvents();
        }
        private void beVCTFile_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            string sFilter = "VCT文件(*.vct)|*.vct";
            dlg.Filter = sFilter;
            dlg.InitialDirectory = Application.StartupPath;
            if (dlg.ShowDialog() != DialogResult.OK)
                return;
            this.beVCTFile.Text = dlg.FileName;
        }
        private void Allshp2Fgdb(string sTmpFilePath,IFeatureDataset destDS)
        {
            IWorkspace destWS=destDS.Workspace;
            string[] shpfiles = System.IO.Directory.GetFiles(sTmpFilePath, "*.SHP", System.IO.SearchOption.AllDirectories);
            foreach (string aShp in shpfiles)
            {
                string shpName = System.IO.Path.GetFileNameWithoutExtension(aShp);
                IWorkspace srcWS = RCIS.GISCommon.WorkspaceHelper2.GetShapefileWorkspace(System.IO.Path.GetDirectoryName(aShp));
                if (srcWS != null)
                {
                    RCIS.GISCommon.EsriDatabaseHelper.ConvertFeatureClass2FeatureDataset(srcWS, destWS, shpName, shpName, destDS, null);
                    IFeatureClass srcClass = (srcWS as IFeatureWorkspace).OpenFeatureClass(shpName);
                    IDataset pDS = srcClass as IDataset;
                    if (pDS.CanDelete())
                    {
                        pDS.Delete();
                    }
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(srcWS);
                }
            }
        }
        private void beDestDir_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            this.beDestDir.Text = dlg.SelectedPath;
        }

        private bool CreateAttrTable(IWorkspace gpWS, string sTableName, TableStructBeginEnd3 curItem)
        {
            //gpWS是全局IWorkspace;
            try
            {

                // 在gpWS中建立属性表:sTableName
                IFeatureWorkspace pWS = gpWS as IFeatureWorkspace;

                //检查当前的属性表是否已经存在[有时VCT文件中几个要素共用一个表]:
                IEnumDataset pEnumDS = gpWS.get_Datasets(esriDatasetType.esriDTAny);
                pEnumDS.Reset();
                IDataset pCurDS = pEnumDS.Next();
                bool bExist = false;
                while (pCurDS != null)
                {
                    string sName = pCurDS.Name.Trim().ToUpper();
                    string ss = sTableName.Trim().ToUpper();
                    if (sName.Equals(ss) == true)
                    {
                        bExist = true;
                        break;
                    }
                    pCurDS = pEnumDS.Next();
                }
                if (bExist)
                    return true;

                //建立字段集合:
                IFields pFields = new ESRI.ArcGIS.Geodatabase.FieldsClass();
                IFieldsEdit pFieldsEdit = pFields as IFieldsEdit;

                //建立字段:ObjectID
                IField pField = new ESRI.ArcGIS.Geodatabase.FieldClass();
                IFieldEdit pFieldEdit = pField as IFieldEdit;
                pFieldEdit.Name_2 = "OBJECTID";
                pFieldEdit.Type_2 = esriFieldType.esriFieldTypeOID;
                pFieldsEdit.AddField(pField);

                //加其他的字段...
                for (int i = 0; i < curItem.aZDMCs.Count; i++)
                {
                    pField = new ESRI.ArcGIS.Geodatabase.FieldClass();
                    pFieldEdit = pField as IFieldEdit;
                    pFieldEdit.Name_2 = (string)curItem.aZDMCs[i];
                    pFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;   //缺省

                    string sZDName = (string)curItem.aZDMCs[i];
                    string sType = (string)curItem.aZDLXs[i];
                    if (sType.Equals("CHAR"))
                    {
                        pFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                        pFieldEdit.Length_2 = (int)curItem.aZDJD[i];
                    }
                    else if (sType.Equals("DATE"))
                    {
                        pFieldEdit.Type_2 = esriFieldType.esriFieldTypeDate;
                    }
                    else if (sType.Equals("DOUBLE"))
                    {
                        pFieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
                        pFieldEdit.Precision_2 = (int)curItem.aZDJD[i];
                        pFieldEdit.Scale_2 = (int)curItem.aZDJD2[i];
                    }
                    else if (sType.Equals("INT"))
                    {
                        pFieldEdit.Type_2 = esriFieldType.esriFieldTypeInteger;
                        
                    }

                    pFieldsEdit.AddField(pField);
                }

                ITable createdTable = pWS.CreateTable(sTableName, pFields, null, null, "");
                return true;
            }
            catch (Exception E)
            {
                throw E;
            }
            //... ...
        }
    

        private  bool CreateSHPGdal(string destDir, string type, string sSxbm, TableStructBeginEnd3 curItem)
        {


            if (!System.IO.Directory.Exists(destDir))
            {
                System.IO.Directory.CreateDirectory(destDir);
            }
            //再目标目录下创建shp文件
            string shpFile = destDir + "\\" + sSxbm + ".SHP";
            //OSGeo.GDAL.Gdal.SetConfigOption("GDAL_FILENAME_IS_UTF8", "YES");
            //// 为了使属性表字段支持中文，请添加下面这句
            //OSGeo.GDAL.Gdal.SetConfigOption("SHAPE_ENCODING", "CP936");
            // 注册所有的驱动
            OSGeo.OGR.Ogr.RegisterAll();

            //创建数据，这里以创建ESRI的shp文件为例
            string strDriverName = "ESRI Shapefile";
            int count = OSGeo.OGR.Ogr.GetDriverCount();
            OSGeo.OGR.Driver oDriver = OSGeo.OGR.Ogr.GetDriverByName(strDriverName);
            if (oDriver == null)
            {
                return false;
            }

            // 创建数据源
            OSGeo.OGR.DataSource oDS = oDriver.CreateDataSource(shpFile, null);
            if (oDS == null)
            {
                return false;
            }

            // 创建图层，创建一个多边形图层，这里没有指定空间参考，如果需要的话，需要在这里进行指定
            OSGeo.OGR.Layer oLayer = null;
            if (type.ToUpper() == "POINT")
            {
                oLayer = oDS.CreateLayer(sSxbm, null, OSGeo.OGR.wkbGeometryType.wkbPoint, null);
            }
            else if (type.ToUpper() == "LINE")
            {
                oLayer = oDS.CreateLayer(sSxbm, null, OSGeo.OGR.wkbGeometryType.wkbLineString, null);
            }
            else if (type.ToUpper() == "POLYGON")
            {
                oLayer = oDS.CreateLayer(sSxbm, null, OSGeo.OGR.wkbGeometryType.wkbPolygon, null);
            }

            if (oLayer == null)
            {
                return false;
            }

            // 下面创建属性表

            for (int i = 0; i < curItem.aZDMCs.Count; i++)
            {
                OSGeo.OGR.FieldDefn oField = null;
                string sZDName = (string)curItem.aZDMCs[i];
                string sType = (string)curItem.aZDLXs[i];
                if (sType.Equals("CHAR"))
                {
                    oField = new OSGeo.OGR.FieldDefn(sZDName, OSGeo.OGR.FieldType.OFTString);
                    oField.SetWidth((int)curItem.aZDJD[i]);
                }
                else if (sType.Equals("DATE"))
                {
                    oField = new OSGeo.OGR.FieldDefn(sZDName, OSGeo.OGR.FieldType.OFTDate);
                }
                else if (sType.Equals("DOUBLE"))
                {
                    oField = new OSGeo.OGR.FieldDefn(sZDName, OSGeo.OGR.FieldType.OFTReal);
                    oField.SetPrecision((int)curItem.aZDJD[i]);

                }
                else if (sType.Equals("INT"))
                {
                    oField = new OSGeo.OGR.FieldDefn(sZDName, OSGeo.OGR.FieldType.OFTInteger);

                }
                oLayer.CreateField(oField, 1);
            }
            if (oLayer != null) oLayer.Dispose();
            if (oDS != null) oDS.Dispose();
            return true;

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //转化为shp
            if (this.beVCTFile.Text.Trim() == "") return;
            if (this.beDestDir.Text.Trim() == "") return;
            string destFile = this.beDestDir.Text + "\\" + System.IO.Path.GetFileNameWithoutExtension(this.beVCTFile.Text) + ".gdb";
            if (System.IO.Directory.Exists(destFile))
            {
                MessageBox.Show("该目录已经存在！");
                return;
            }
            
            memoLog.Text = "";

            Hashtable ghasTableStructure = null;
            Hashtable ghasFeatuerCode = null;
            this.Cursor = Cursors.WaitCursor;
            string vctFile = this.beVCTFile.Text;
            Encoding gb2312 = Encoding.GetEncoding("GB2312");

            VCTReader3 reader = new VCTReader3();

            string sTmpFilePath = Application.StartupPath + "\\VCTProcess";
            if (Directory.Exists(sTmpFilePath))
            {
                RCIS.Utility.FileHelper.DelectDir(sTmpFilePath);
            }

            try
            {
                
                #region 拆分文件
                using (StreamReader sr = new StreamReader(vctFile, gb2312))
                {

                    //以'属性表名'为Key,存放TableStructClass信息
                    reader.GetFeatureCodes(sr);
                    reader.GetAllTableStruct(sr);
                    ghasTableStructure = reader.ghasTableStructure;
                    ghasFeatuerCode = reader.ghasFeatuerCode;
                    UpdateStatus("读取VCT结构信息完毕！");
                    
                    Dictionary<string, ArrayList> dicPt = reader.getNewPointDataFile(sr);
                    UpdateStatus("拆分点文件结束！");
                    Dictionary<string, ArrayList> dicLine = reader.getNewLineDataFile(sr);
                    UpdateStatus("拆分线文件结束！");

                    Dictionary<string, ArrayList> dicPolygon = reader.getNewPolygonDataFile(sr);
                    UpdateStatus("拆分面文件结束！");

                    reader.getNewAnnotationFile(sr); //注记文件
                    
                    #region 合并为一个

                    foreach (KeyValuePair<string, ArrayList> aItem in dicLine)
                    {
                        if (!dicPt.ContainsKey(aItem.Key))
                        {
                            dicPt.Add(aItem.Key, aItem.Value);
                        }
                    }
                    dicLine.Clear();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    foreach (KeyValuePair<string, ArrayList> aItem in dicPolygon)
                    {
                        if (!dicPt.ContainsKey(aItem.Key))
                        {
                            dicPt.Add(aItem.Key, aItem.Value);
                        }
                    }
                    dicPolygon.Clear();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    #endregion

                    //整理属性文件
                    reader.GetNewAttrFile(sr, dicPt);
                    UpdateStatus("拆分属性文件结束！");
                    sr.Close();

                    dicPt.Clear();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }

                
                #endregion

            }
            catch (Exception ex)
            {
                UpdateStatus(ex.ToString());
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
            //创建shp
            foreach (DictionaryEntry de in ghasFeatuerCode)
            {
                FeatureCodeBeginEnd3 aFCCode = (FeatureCodeBeginEnd3)de.Value;
                string aTabName = aFCCode.sSXBM;
                string aJhlx = aFCCode.sJHLX;
                TableStructBeginEnd3 curItem = (TableStructBeginEnd3)ghasTableStructure[aTabName];

                CreateSHPGdal(sTmpFilePath + "\\" + aTabName, aJhlx.ToUpper(), aTabName, curItem);

            }
            //创建注记层
            if (ghasTableStructure.ContainsKey("ZJ"))
            {
                TableStructBeginEnd3 zjFld = (TableStructBeginEnd3)ghasTableStructure["ZJ"];
                CreateSHPGdal(sTmpFilePath + "\\ZJ", "POINT", "ZJ", zjFld);
            }
            UpdateStatus("结构创建完毕！");

            foreach (DictionaryEntry de in ghasFeatuerCode)
            {

                FeatureCodeBeginEnd3 aFCCode = (FeatureCodeBeginEnd3)de.Value;
                string aTabName = aFCCode.sSXBM;

                //if (aTabName.ToUpper() == "XZQ" || aTabName == "DLTB")
                //{
                //}
                string aJhlx = aFCCode.sJHLX;
                string shpFile = sTmpFilePath + "\\" + aTabName + "\\" + aTabName + ".shp";
                if (!System.IO.File.Exists(shpFile))
                {
                    continue;
                }

                if (aJhlx.ToUpper() == "POINT")
                {
                    VCT3InAPOINT2Shp inPt = new VCT3InAPOINT2Shp(aTabName, ghasTableStructure, shpFile);
                    inPt.InAPointClass();
                    UpdateStatus(aTabName + "导入完毕！");

                }
                else if (aJhlx.ToUpper() == "LINE")
                {
                    VCT3InALINE2SHP inLIne = new VCT3InALINE2SHP(aTabName, ghasTableStructure, shpFile);
                    inLIne.InALineClass();
                    UpdateStatus(aTabName + "导入完毕！");


                }
                else
                    if (aJhlx.ToUpper() == "POLYGON")
                    {

                        VCT3APolygon2Shp inAM = new VCT3APolygon2Shp(aTabName, ghasTableStructure, shpFile);
                        inAM.InAPolygonClass();
                        UpdateStatus(aTabName + "导入完毕！");
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                    }
            }
            //注记单独处理
            VCT3InAPOINT2Shp inAnno = new VCT3InAPOINT2Shp();
            inAnno.InAnnotationClass();
            UpdateStatus( "注记导入完毕！");      
            string sourceDir = RCIS.Global.AppParameters.TemplatePath + @"\VCTInput.gdb";
            RCIS.Utility.File_DirManipulate.FolderCopy(sourceDir, destFile);
            //创建数据集
            IWorkspace destWS = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(destFile);
            if (destWS == null)
            {
                memoLog.Text += "\r\n" + DateTime.Now.ToString() + "创建文件数据库失败，是否找不到模板文件...";
                MessageBox.Show("创建文件数据库失败！");
                return;
            }

            string sDSName = RCIS.Global.AppParameters.DATASET_DEFAULT_NAME; //数据集名称
            //创建数据集
          
            ISpatialReference spatialReference = null;
            IFeatureDataset destFDS = null;
            try
            {
                spatialReference = reader.getSpatialRef(vctFile);
                IFeatureWorkspace pFcWps = destWS as IFeatureWorkspace;
                destFDS = pFcWps.CreateFeatureDataset(sDSName, spatialReference);

            }
            catch (Exception ex)
            {
                MessageBox.Show("创建数据集失败！");
                memoLog.Text += "\r\n" + DateTime.Now.ToString() + ":" + ex.Message;
                return;
            }
            if (destFDS == null)
                return;
            Allshp2Fgdb(sTmpFilePath, destFDS);

            //查看有无扩展属性表
            foreach(DictionaryEntry de in ghasTableStructure)
            {
                TableStructBeginEnd3 aTable = (TableStructBeginEnd3)de.Value;
                if (aTable.isKzsxb)
                {
                    //纯扩展属性表
                    if (CreateAttrTable(destWS, de.Key.ToString(),aTable))
                    {
                        VCT3InAExtTable inExt = new VCT3InAExtTable(de.Key.ToString().Trim(), aTable, destWS);
                        inExt.WriteExtTable();
                    }
                }
            }
            UpdateStatus("扩展属性导入完毕！");

            System.Runtime.InteropServices.Marshal.ReleaseComObject(destFDS);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(destWS);
            GC.Collect();
            GC.WaitForPendingFinalizers();
            UpdateStatus("全部数据入库完毕！");
            this.Cursor = Cursors.Default;
            MessageBox.Show("全部导入完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void VCT3InFGDB_Load(object sender, EventArgs e)
        {

        }
    }
}
