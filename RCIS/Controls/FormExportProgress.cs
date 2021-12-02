using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.Collections;

using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Carto;

namespace RCIS.Controls
{
    public partial class FormExportProgress : Form
    {
        private ILayer m_SelectedLayer;
        private IEnvelope m_ActiveViewExtent;
        private int m_FeatureClass;
        private string m_ExportShapefilePath;
        private IWorkspace m_SourceWorkspace;

        

        public FormExportProgress()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 待导出图层
        /// </summary>
        public ILayer SelectedLayer
        {
            get
            {
                return this.m_SelectedLayer;
            }
            set
            {
                this.m_SelectedLayer = value;
            }
        }

        /// <summary>
        /// 当前视图范围
        /// </summary>
        public IEnvelope ActiveViewExtent
        {
            get
            {
                return this.m_ActiveViewExtent;
            }
            set
            {
                this.m_ActiveViewExtent = value;
            }
        }

        /// <summary>
        /// 导出类型
        /// </summary>
        public int FeatureClass
        {
            get
            {
                return this.m_FeatureClass;
            }
            set
            {
                this.m_FeatureClass = value;
            }
        }

        /// <summary>
        /// 导出Shapefile路径
        /// </summary>
        public string ExportShapefilePath
        {
            get
            {
                return this.m_ExportShapefilePath;
            }
            set
            {
                this.m_ExportShapefilePath = value;
            }
        }

        /// <summary>
        /// 源工作空间
        /// </summary>
        public IWorkspace SourceWorkspace
        {
            get
            {
                return this.m_SourceWorkspace;
            }
            set
            {
                this.m_SourceWorkspace = value;
            }
        }

        

        private void FormExportProgress_Load(object sender, EventArgs e)
        {
            this.Show();
            Application.DoEvents();
            labelFeatureName.Text = "正在导出" + m_SelectedLayer.Name + "图层";
            Application.DoEvents();
            LayerToShapefile(SelectedLayer, ActiveViewExtent, FeatureClass, ExportShapefilePath);

            //this.DialogResult = DialogResult.OK;
            //this.Close();
        }

        /// <summary>
        /// 取消按钮，取消导出操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private string getLayerObjId(ILayer pLayer)
        {
            IFeatureClass pSelectionClass = (pLayer as IFeatureLayer).FeatureClass;
            for (int i = 0; i < pSelectionClass.Fields.FieldCount; i++)
            {
                IField pFld= pSelectionClass.Fields.get_Field(i);
                if (pFld.Type == esriFieldType.esriFieldTypeOID)
                {
                    return pFld.Name.ToUpper();
                }
            }
                return "OBJECTID";
        }

        private void LayerToShapefile(ILayer pLayer, IEnvelope pEnvelope, int exportClass, string shapefileFullPath)
        {
            try
            {
                if (exportClass == 0)
                {
                    //没有选择导出的类型
                    MessageBox.Show("不能确定导出类型");
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                }

                IQueryFilter queryFilter = new QueryFilterClass();
                   

                if (exportClass == 1)
                {
                    //导出选中
                    try
                    {
                        //RCIS.GISCommon.EsriDatabaseHelper.ExportFeature2((pLayer as IFeatureLayer).FeatureClass, shapefileFullPath, pLayer as IFeatureSelection);
                        RCIS.GISCommon.EsriDatabaseHelper.ExportFeature3((pLayer as IFeatureLayer).FeatureClass, shapefileFullPath, pLayer as IFeatureSelection);
                        this.DialogResult = DialogResult.OK;

                    }
                    catch (Exception ex)
                    {
                    }
                    //string tmpClause = "";
                    //if (selectFeatureIDs.Count > 0)
                    //{
                    //    tmpClause = ojbFldName + " IN (";
                    //    for (int i = 0; i < selectFeatureIDs.Count; i++)
                    //    {
                    //        tmpClause += selectFeatureIDs[i].ToString() + ",";
                    //    }
                    //    if (tmpClause.EndsWith(","))
                    //    {
                    //        tmpClause = tmpClause.Remove(tmpClause.Length - 1);
                    //    }
                    //    tmpClause += " )";                        

                    //}
                    //if (tmpClause.Trim()!="")
                    //    queryFilter.WhereClause = tmpClause;
                }
                if (exportClass == 2)
                {
                    ISelectionSet pSelectionSet = (pLayer as IFeatureSelection).SelectionSet;
                    ArrayList selectFeatureIDs = new ArrayList();

                    IEnumIDs pEnumIDs = pSelectionSet.IDs;
                    int count = pEnumIDs.Next();
                    while (count > 0)
                    {
                        selectFeatureIDs.Add(count);
                        count = pEnumIDs.Next();
                    }
                    string ojbFldName = getLayerObjId(pLayer);
                    //string tmpClause = "BSM NOT IN ('";
                    string tmpClause = ojbFldName + " in (";
                    //导出未选择的要素
                    for (int i = 0; i < selectFeatureIDs.Count; i++)
                    {
                        tmpClause += selectFeatureIDs[i].ToString() + ",";
                    }
                    tmpClause = tmpClause.Substring(0, tmpClause.LastIndexOf(",")) + ")";
                    queryFilter.WhereClause = tmpClause;
                    if (Export(pLayer, queryFilter, shapefileFullPath))
                    {
                        this.DialogResult = DialogResult.OK;
                    }
                }
                if (exportClass == 3)
                {
                    //导出全部要素
                    
                    if (Export(pLayer, null, shapefileFullPath))
                    {
                        this.DialogResult = DialogResult.OK;
                    }
                }

                if (exportClass == 4)
                {
                    //导出视图内的要素
                    ISpatialFilter pSpatialFilter = new SpatialFilterClass();
                    pSpatialFilter.Geometry = pEnvelope as IGeometry;
                    //pSpatialFilter.GeometryField = (pLayer as IFeatureLayer).FeatureClass.ShapeFieldName;
                    pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                    queryFilter = pSpatialFilter as IQueryFilter;
                    if (Export(pLayer, queryFilter, shapefileFullPath))
                    {
                        this.DialogResult = DialogResult.OK;
                    }
                }

               
            }
            catch (Exception ex)
            {

            }
            finally
            {
                this.Close();
            }
        }



        private bool Export(
            ILayer pLayer,IQueryFilter queryFilter, string shapefileName )
        {
            try{

                IFeatureClass fc = (pLayer as IFeatureLayer).FeatureClass;
                IDataset inDataSet = fc as IDataset;
                string inClassName=inDataSet.Name;
                IWorkspace inWorkspace = inDataSet.Workspace;

                string folderPath = System.IO.Path.GetDirectoryName(shapefileName);
                string shortName = System.IO.Path.GetFileNameWithoutExtension(shapefileName); //shapefileName.Substring(idxStart + 1, idxEnd - idxStart - 1);
                IWorkspaceFactory targetWorkspaceFactory = new ShapefileWorkspaceFactoryClass();
                IWorkspace targetWorkspace = targetWorkspaceFactory.OpenFromFile(folderPath, 0);

                RCIS.GISCommon.EsriDatabaseHelper.ConvertFeatureClass(inWorkspace, targetWorkspace, inClassName, shortName, queryFilter);

                
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }


        ///// <summary>
        ///// 输出图层全部要素
        ///// </summary>
        ///// <param name="pLayer"></param>
        ///// <param name="shapefileName"></param>
        ///// <returns></returns>
        //private bool Export(ILayer pLayer,IQueryFilter queryFilter, string shapefileName)
        //{
            

        //    int idxStart=shapefileName.LastIndexOf("\\");
        //    int idxEnd=shapefileName.LastIndexOf(".");
        //    string folderPath=shapefileName.Substring(0,idxStart);
        //    string shortName = shapefileName.Substring(idxStart + 1, idxEnd - idxStart - 1);

        //    try
        //    {
        //        //IWorkspaceFactory sourceWorkspaceFactory = new WorkspaceFactoryClass();
        //        IWorkspaceFactory targetWorkspaceFactory = new ShapefileWorkspaceFactoryClass();
        //        IWorkspace sourceWorkspace = this.SourceWorkspace;
        //        IWorkspace targetWorkspace = targetWorkspaceFactory.OpenFromFile(folderPath, 0);
                
        //        IDataset sourceWorkspaceDataset = sourceWorkspace as IDataset;
        //        IDataset targetWorkspaceDataset = targetWorkspace as IDataset;
        //        IName sourceWorkspaceDatasetName = sourceWorkspaceDataset.FullName;
        //        IName targetWorkspaceDatasetName = targetWorkspaceDataset.FullName;
        //        IWorkspaceName sourceWorkspaceName = sourceWorkspaceDatasetName as IWorkspaceName;
        //        IWorkspaceName targetWorkspaceName = targetWorkspaceDatasetName as IWorkspaceName;

        //        IFeatureClassName targetFeatureClassName = new FeatureClassNameClass();
        //        IDatasetName targetDatasetName = targetFeatureClassName as IDatasetName;
        //        targetDatasetName.Name = shortName;
        //        targetDatasetName.WorkspaceName = targetWorkspaceName;

        //        //string sLayerName = pLayer.Name.Substring(pLayer.Name.IndexOf("|") + 1);
        //        IFeatureLayer pFeatureLayer = pLayer as IFeatureLayer;
        //        string sLayerName = (pFeatureLayer.FeatureClass as IDataset).Name;
        //        IFeatureClassName sourceFeatureClassName = new FeatureClassNameClass();
        //        IDatasetName sourceDatasetName = sourceFeatureClassName as IDatasetName;
        //        //sourceDatasetName.Name = prefix + sLayerName;
        //        sourceDatasetName.Name = sLayerName;
        //        sourceDatasetName.WorkspaceName = sourceWorkspaceName;

        //        IName sourceName = sourceFeatureClassName as IName;
        //        IFeatureClass sourceFeatureClass = sourceName.Open() as IFeatureClass;

        //        IFieldChecker fieldChecker = new FieldCheckerClass();
        //        IFields sourceFields = sourceFeatureClass.Fields;
        //        IFields targetFields = null;
        //        IEnumFieldError enumFieldError = null;

        //        fieldChecker.InputWorkspace = sourceWorkspace;
        //        fieldChecker.ValidateWorkspace = targetWorkspace;
        //        fieldChecker.Validate(sourceFields, out enumFieldError, out targetFields);

        //        //IFieldsEdit pFE = targetFields as IFieldsEdit;

        //        //for (int i = 0; i < targetFields.FieldCount; )
        //        //{
        //        //    IField pField = targetFields.get_Field(i);
        //        //    if (pField.Name == "OBJECTID_1")
        //        //    {
        //        //        pFE.DeleteField(pField);
        //        //        break;
        //        //    }
        //        //    i++;
        //        //}

        //        if (enumFieldError != null)
        //        {
        //            //MessageBox.Show("出错！");
        //            //return false;

        //        }

        //        string shapeFieldName = sourceFeatureClass.ShapeFieldName;
        //        int shapeFieldIndex = sourceFeatureClass.FindField(shapeFieldName);
        //        IField shapeField = sourceFields.get_Field(shapeFieldIndex);

        //        IGeometryDef geometryDef = shapeField.GeometryDef;
        //        IClone geometryDefClone = geometryDef as IClone;
        //        IClone targetGeometryDefClone = geometryDefClone.Clone();
        //        IGeometryDef targetGeometryDef = targetGeometryDefClone as IGeometryDef;

        //        IFeatureDataConverter featureDataConverter = new FeatureDataConverterClass();
        //        IEnumInvalidObject enumInvalidObject = featureDataConverter.ConvertFeatureClass(sourceFeatureClassName,
        //            queryFilter, null, targetFeatureClassName, targetGeometryDef, targetFields, "", 1000, 0);

        //        IInvalidObjectInfo invalidObjectInfo = null;
        //        enumInvalidObject.Reset();
        //        while ((invalidObjectInfo = enumInvalidObject.Next()) != null)
        //        {
        //            //MessageBox.Show("错~~~");
        //        }

        //        #region 删除Shapefile中的 OBJECT_1 字段
        //        //try
        //        //{
        //        //    IWorkspaceFactory pWSF = new ShapefileWorkspaceFactoryClass();
        //        //    IWorkspace pWS = pWSF.OpenFromFile(folderPath, 0);
        //        //    IFeatureWorkspace pFWS = pWS as IFeatureWorkspace;
        //        //    IFeatureClass pFC = pFWS.OpenFeatureClass(shortName);

        //        //    IFields pFields = pFC.Fields;



        //        //    for (int i = 0; i < targetFields.FieldCount; )
        //        //    {
        //        //        IField pField = targetFields.get_Field(i);
        //        //        if (pField.Name == "BSM")
        //        //        {
        //        //            //IFieldEdit pFE = pField as IFieldEdit;
        //        //            //pFE.Name_2 = pField.Name;

        //        //            pFC.DeleteField(pField);
        //        //            break;
        //        //        }
        //        //        i++;
        //        //    }
        //        //}
        //        //catch(Exception ex)
        //        //{}
        //        //finally
        //        //{

        //        //}
        //        #endregion

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //    finally
        //    {
 
        //    }
        //}
    }
}
