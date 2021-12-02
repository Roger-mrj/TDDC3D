using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using RCIS.GISCommon;
using System.Collections;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geoprocessor;
using ESRI.ArcGIS.Geoprocessing;

using ESRI.ArcGIS.ADF;

namespace TDDC3D.sys
{
    public partial class InAFeatureClassForm : Form
    {
        public InAFeatureClassForm()
        {
            InitializeComponent();
        }


        IWorkspace OriginWS;//源数据的WS
        IWorkspace m_DestinationWS;//目标数据的WS

        string OriginFCName;
        string DestinationFCName;


        ArrayList sOriginList = new ArrayList();
        //源要素类的类型

        string OriginFCShapeType = "";
        //目标要素类的类型
        string DestinationFCShapeType = "";


        public IWorkspace DestinationWS
        {
            get { return m_DestinationWS; }
            set { m_DestinationWS = value; }
        }
        //加载本地数据的WS，到时候把WS赋值给DestinationWS

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
        private string GetFeatureLayerTableName(IFeatureClass fc)
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

                MessageBox.Show("" + ee.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return "";
            }
        }

        private void LoadData()
        {
            if (this.DestinationWS == null)
                return;
            this.cmbLocalFeatureclasses.Properties.Items.Clear();

            IFeatureWorkspace pFWS = m_DestinationWS as IFeatureWorkspace;
            IEnumDataset pEnumDataset = m_DestinationWS.get_Datasets(esriDatasetType.esriDTFeatureDataset);
            IDataset pDateset = pEnumDataset.Next();

            //IDataset pDateset = pFWS.OpenFeatureDataset("KFQ");

            while (pDateset != null)
            {
                IFeatureClassContainer pFeatureClsConner = (IFeatureClassContainer)pDateset;
                for (int i = 0; i < pFeatureClsConner.ClassCount; i++)
                {
                    IFeatureClass pFeauteClass = pFeatureClsConner.get_Class(i);


                    this.cmbLocalFeatureclasses.Properties.Items.AddRange(new object[]{
														  
                        this.GetFeatureLayerTableName(pFeauteClass)+"|"+pFeauteClass.AliasName.ToString()
														  });



                }
                pDateset = pEnumDataset.Next();

            }


        }


        private void InAFeatureClassForm_Load(object sender, EventArgs e)
        {
            this.gridDestFields.ClearSelection();
            this.gridDestFields.AllowUserToAddRows = false;
            this.gridDestFields.ReadOnly = true;
            this.gridSrcFields.ClearSelection();
            this.gridSrcFields.AllowUserToAddRows = false;
            this.gridDestFields.RowHeadersVisible = false;
            this.gridSrcFields.RowHeadersVisible = false;


            LoadData();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //加载目标要素类
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            string selPath = dlg.SelectedPath;
            this.DestinationWS = WorkspaceHelper2.GetFileGdbWorkspace(dlg.SelectedPath);
            LoadData();

        }

        private void btSounceFC_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            AddDataForm frm = new AddDataForm();
            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                return;
            ILayer currLyr = frm.resultLyr;
            IFeatureLayer currFeaLyr = currLyr as IFeatureLayer;
            if (currFeaLyr == null)
                return;
            IFeatureClass srcClass = currFeaLyr.FeatureClass;

            OriginWS = (srcClass as IDataset).Workspace;
            OriginFCName = LayerHelper.GetClassShortName(srcClass as IDataset);
            LoadOriginFCToComboBox(GetOriginFeatureClass(srcClass));


            this.btSounceFC.Text = OriginWS.PathName + "\\" + OriginFCName;
            this.cmbLocalFeatureclasses.Text = "";
        }

        




        #region 根据路径返回一个WORKSPACE
        private IWorkspace GetWorkSpace(string strPath)
        {

            IWorkspaceFactory workspaceFactory = new ESRI.ArcGIS.DataSourcesFile.ShapefileWorkspaceFactoryClass();
            return workspaceFactory.OpenFromFile(strPath, 0);

        }

        #endregion

        //加载源数据字段名到ComboBox
        private void LoadOriginFCToComboBox(ArrayList sList)
        {
            this.ColumnSrcFields.Items.Clear();
            if (sList.Count == 0) return;
            this.ColumnSrcFields.Items.AddRange(new object[]{
														  ""
														  });
            for (int i = 0; i < sList.Count; i++)
            {

                this.ColumnSrcFields.Items.AddRange(new object[]{
														  sList[i].ToString()
														  });
            }
        }

        //加载目标要素类 字段
        private void LoadFeatureClassField(IWorkspace pWS)
        {
            try
            {
                this.gridDestFields.Rows.Clear();
                this.gridSrcFields.Rows.Clear();
                if (pWS == null) return;


                IFeatureWorkspace pFWS = pWS as IFeatureWorkspace;

                if (DestinationFCName == "") return;
                if (DestinationFCName == null) return;
                IFeatureClass pFC = pFWS.OpenFeatureClass(DestinationFCName);

                this.DestinationFCShapeType = pFC.ShapeType.ToString();
                ITable pTable = pFC as ITable;
                int aOIDIdx = -1;
                try
                {
                    if (pTable.HasOID)
                    {
                        aOIDIdx = pTable.FindField(pTable.OIDFieldName);
                    }
                }
                catch { }
                int j = 0;
                for (int i = 0; i < pTable.Fields.FieldCount; i++)
                {
                    IField pField = pTable.Fields.get_Field(i);
                    if (i != aOIDIdx &&
                        (pField.Type != esriFieldType.esriFieldTypeBlob) &&
                         (pField.Type != esriFieldType.esriFieldTypeGeometry) &&
                        (pField.Type != esriFieldType.esriFieldTypeOID) &&
                        (pField.Type != esriFieldType.esriFieldTypeRaster))
                    {
                        gridDestFields.AllowUserToAddRows = true;
                        this.gridSrcFields.AllowUserToAddRows = true;
                        int iSrcRowIndex = this.gridSrcFields.Rows.Add(1);
                        this.gridDestFields.Rows.Add(1);
                        this.gridDestFields[0, j].Value = pField.Name.ToString().Trim();
                        this.gridDestFields[1, j].Value = pField.Type.ToString();


                        for (int idx = 0; idx < this.ColumnSrcFields.Items.Count; idx++)
                        {

                            string sValue = this.ColumnSrcFields.Items[idx].ToString().Trim();
                            string strSrc = sValue;
                            if (sValue.IndexOf('|') > 0)
                            {
                                strSrc = strSrc.Substring(0, strSrc.IndexOf('|'));
                            }

                            string strDest = pField.Name.ToString().Trim();
                           

                            if (strSrc.ToUpper() == strDest.ToUpper())
                            {

                                this.gridSrcFields[0, iSrcRowIndex].Value = sValue;
                                break;
                                // this.gridSrcFields[1, iSrcRowIndex].Value = pField.Type.ToString();
                            }
                        }


                        this.gridDestFields.AllowUserToAddRows = false;
                        this.gridSrcFields.AllowUserToAddRows = false;
                        j++;
                    }

                }



            }
            catch (Exception ee)
            {

                MessageBox.Show("" + ee.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private string  GpAppend(IWorkspace targetWorkspace, string nameOfTargetFeatureClass)
        {
            //获取图层
            string err = "";
            if (targetWorkspace == null) return "";
            IFeatureWorkspace pTargetFWS = (IFeatureWorkspace)targetWorkspace;
            IFeatureClass pTargetTable = pTargetFWS.OpenFeatureClass(nameOfTargetFeatureClass);

            //IFeatureLayer sourceLayer = ((ComboBoxItem)(this.cmbSourceLayer.Properties.Items[this.cmbSourceLayer.SelectedIndex])).ItemObject as IFeatureLayer;
            string sourcePath = this.btSounceFC.Text;   //((ComboBoxItem)(this.cmbSourceLayer.Properties.Items[this.cmbSourceLayer.SelectedIndex])).ItemText;



            IFeatureClass pSourceTable = (this.OriginWS as IFeatureWorkspace).OpenFeatureClass(this.OriginFCName);


            IPRJSpatialReferenceGEN sourceSrc = (IPRJSpatialReferenceGEN)(pSourceTable as IGeoDataset).SpatialReference;
            IPRJSpatialReferenceGEN targetSrc = (IPRJSpatialReferenceGEN)(pTargetTable as IGeoDataset).SpatialReference;

            //判断坐标是否一致
            string sourceStr, targetStr;
            int sourceInt, tagetInt;
            sourceSrc.ExportSpatialReferenceToPRJ(out sourceStr, out sourceInt);
            targetSrc.ExportSpatialReferenceToPRJ(out targetStr, out tagetInt);

            if (sourceStr.Trim() != targetStr.Trim())
            {
                if (MessageBox.Show("导入数据和系统坐标系不一致,是否继续？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel)
                {
                    return "导入数据和系统坐标系不一致";
                };
            }

            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("请稍等", "正在进行入库操作...");
            wait.Show();
            try
            {
                //需清除其中数据
                if (this.chkCover.Checked)
                {
                    ITable pTargetITable = (ITable)pTargetTable;
                    pTargetITable.DeleteSearchedRows(null);
                }


                //获取目标表的路径
                string targetPathName = sourcePath + "";

                if(!sourcePath.ToUpper().Contains(".GDB") && !sourcePath.ToUpper().Contains(".MDB"))
                    targetPathName = sourcePath + ".shp";

                //创建DETABLE
                IGPUtilities gputilities = new GPUtilitiesClass();
                IDETable inputTableA = (IDETable)gputilities.MakeDataElement(targetPathName, null, null);

                //创建输出表的Array
                IArray inputtables = new ArrayClass();
                inputtables.Add(inputTableA);

                //初始化GPFieldMapping
                IGPFieldMapping fieldmapping = new GPFieldMappingClass();
                fieldmapping.Initialize(inputtables, null);

                //根据对应关系创建fieldMapping

                for (int i = 0; i < this.gridSrcFields.Rows.Count; i++)
                {
                    if (this.gridSrcFields[0, i].Value != null)
                    {
                        string nameOfSourceFieldName = this.gridSrcFields[0, i].Value.ToString();
                        nameOfSourceFieldName = nameOfSourceFieldName.Substring(0, nameOfSourceFieldName.IndexOf("|"));
                        string nameOfTargetFieldName = this.gridDestFields[0, i].Value.ToString();
                        if (string.IsNullOrEmpty(nameOfSourceFieldName) || string.IsNullOrEmpty(nameOfTargetFieldName) || nameOfSourceFieldName == nameOfTargetFieldName)
                        {
                            continue;
                        }

                        IFieldEdit field = pTargetTable.Fields.get_Field(pTargetTable.FindField(nameOfTargetFieldName)) as IFieldEdit;
                        IGPFieldMap trackid = new GPFieldMapClass();
                        trackid.OutputField = field;

                        int fieldmap_index = fieldmapping.FindFieldMap(nameOfSourceFieldName);
                        IGPFieldMap stfid_fieldmap = fieldmapping.GetFieldMap(fieldmap_index);
                        int field_index = stfid_fieldmap.FindInputField(inputTableA, nameOfSourceFieldName);
                        IField inputField = stfid_fieldmap.GetField(field_index);
                        trackid.AddInputField(inputTableA, inputField, 0, inputField.Length);
                        fieldmapping.AddFieldMap(trackid);
                    }
                }

                Geoprocessor gp = new Geoprocessor();
                gp.OverwriteOutput = true;
                string s = null;
                try
                {
                    ESRI.ArcGIS.DataManagementTools.Append append = new ESRI.ArcGIS.DataManagementTools.Append();
                    append.inputs = pSourceTable;
                    append.target = pTargetTable;
                    append.schema_type = "NO_TEST";
                    append.field_mapping = fieldmapping;
                    append.subtype = "LAST";
                    gp.Execute(append, null);
                    s = GpToolHelper.ReturnMessages(gp);

                    wait.Close();
                    if ((s.ToUpper().Contains("ERROR")) || (s.Contains("失败")))
                    {
                        err = "执行失败。" + s;
                        //MessageBox.Show("执行失败。" + s, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        err = "";
                        //MessageBox.Show("执行成功。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch
                {
                    s = GpToolHelper.ReturnMessages(gp);
                    throw new Exception(s);

                }
            }
            catch (Exception ex)
            {
                err = ex.Message;
            }
            finally
            {
                if (wait != null)
                    wait.Close();
            }
            return err;

        }


        private void AddFieldValue(IWorkspace sourceWorkspace,
            IWorkspace targetWorkspace,
            string nameOfSourceFeatureClass,
            string nameOfTargetFeatureClass
            )
        {
            
                #region 准备工作
                if (sourceWorkspace == null) return;
                if (targetWorkspace == null) return;
                IFeatureWorkspace pSourceFWS = (IFeatureWorkspace)sourceWorkspace;
                IFeatureWorkspace pTargetFWS = (IFeatureWorkspace)targetWorkspace;
                IFeatureClass pSourceTable = pSourceFWS.OpenFeatureClass(nameOfSourceFeatureClass);
                IFeatureClass pTargetTable = pTargetFWS.OpenFeatureClass(nameOfTargetFeatureClass);

                ISpatialReference pSP = ((pTargetTable as IDataset) as IGeoDataset).SpatialReference;

                
                string nameOfSourceFieldName = "";//源要素类字段名

                string nameOfTargetFieldName = "";//目标要素类字段名
                int SourceFieldIndex;//源要素类字段名索引

                int TargetFieldIndex;//目标要素类字段名索引
                int aCount = pSourceTable.FeatureCount(null);
                if (aCount <= 0) return;

                string aText = "正在导入" + pSourceTable.AliasName + "到" + pTargetTable.AliasName;
                status.Text = aText;
                #endregion
                
                
                IWorkspaceEdit aWKEdit = pTargetFWS as IWorkspaceEdit;
                aWKEdit.StartEditing(false);
                aWKEdit.StartEditOperation();
                int aStep = aCount / 10 + 1;
                int order = 0;

                #region  //提取字段对应关系.
                List<int> aSrcFields = new List<int>();
                List<int> aDestFields = new List<int>();
                for (int i = 0; i < this.gridSrcFields.Rows.Count; i++)
                {
                    if (this.gridSrcFields[0, i].Value != null)
                    {
                        nameOfSourceFieldName = this.gridSrcFields[0, i].Value.ToString();
                        nameOfSourceFieldName = nameOfSourceFieldName.Substring(0, nameOfSourceFieldName.IndexOf("|"));                      

                        SourceFieldIndex = pSourceTable.Fields.FindField(nameOfSourceFieldName);
                        nameOfTargetFieldName = this.gridDestFields[0, i].Value.ToString();
                        
                        TargetFieldIndex = pTargetTable.Fields.FindField(nameOfTargetFieldName);
                        aSrcFields.Add(SourceFieldIndex);
                        aDestFields.Add(TargetFieldIndex);
                    }
                }
                #endregion 


                IFeatureCursor insertCursor = pTargetTable.Insert(true);

                try
                {
                    IFeatureLayer sourceLayer = new FeatureLayerClass();
                    sourceLayer.FeatureClass = pSourceTable;
                    IDataset pSourceDs = pSourceTable as IDataset;
                    IGeoDataset pGeoSourceDs = pSourceDs as IGeoDataset;
                    IIdentify dltbIndentify = sourceLayer as IIdentify;
                    IArray arrDltbIDs = dltbIndentify.Identify(pGeoSourceDs.Extent);
                    for (int i = 0; i < arrDltbIDs.Count; i++)
                    {
                        IFeatureIdentifyObj idObj = arrDltbIDs.get_Element(i) as IFeatureIdentifyObj;
                        IRowIdentifyObject pRow = idObj as IRowIdentifyObject;
                        IFeature pfea = pRow.Row as IFeature;

                        try
                        {
                            #region  //复制要素
                            using (ComReleaser comReleaser = new ComReleaser())
                            {
                                IFeatureBuffer pTargetBuffer = pTargetTable.CreateFeatureBuffer();
                                comReleaser.ManageLifetime(pTargetBuffer);

                                FeatureHelper.CopyRowFeatureBuffer(pfea, pTargetBuffer, aSrcFields, aDestFields);
                                IGeometry srcShp = pfea.ShapeCopy;
                                srcShp.Project(pSP);
                                //IZAware pZaware = srcShp as IZAware;
                                //pZaware.DropZs();
                                //pZaware.ZAware = false;
                                pTargetBuffer.Shape = srcShp;
                                insertCursor.InsertFeature(pTargetBuffer);

                                order++;
                                string aTaskCap = aText + "(" + (order) + "/" + aCount + ")";
                                status.Text = aTaskCap;
                                Application.DoEvents();
                                if (order % 2000 == 0)
                                {
                                    
                                    
                                    insertCursor.Flush();
                                }
                            }
                            #endregion


                        }
                        catch (Exception ex)
                        { }
                    }
                    
                    aWKEdit.StopEditOperation();
                    aWKEdit.StopEditing(true);
                }
                catch { aWKEdit.StopEditing(false); }
                finally
                {
                    
                    Marshal.ReleaseComObject(insertCursor);
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    status.Text = "";

                }
                

                
            
            
            
        }
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (this.btSounceFC.Text == "")
            {
                this.Cursor = Cursors.Arrow;
                MessageBox.Show("源要素类路径为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (this.cmbLocalFeatureclasses.Text == "")
            {
                this.Cursor = Cursors.Arrow;
                MessageBox.Show("本地要素类为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            if (this.OriginFCShapeType != this.DestinationFCShapeType)
            {
                this.Cursor = Cursors.Arrow;
                MessageBox.Show("转换的要素类几何类型不一致，请检查！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            this.Enabled = false;
            this.Cursor = Cursors.WaitCursor;

            try
            {


                if (this.gridDestFields.Rows.Count == 0) return;

                // AddFieldValue(OriginWS, DestinationWS, OriginFCName, DestinationFCName);

                string err = this.GpAppend(DestinationWS, DestinationFCName);
                if (err == "")
                {
                    MessageBox.Show("转换成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(err, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                
                btSounceFC.Text = "";
                cmbLocalFeatureclasses.Text = "";
                this.gridDestFields.Rows.Clear();
                this.ColumnSrcFields.Items.Clear();
                this.gridSrcFields.Rows.Clear();
                this.Cursor = Cursors.Arrow;
            }
            catch (Exception ee)
            {

                MessageBox.Show("" + ee.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Cursor = Cursors.Arrow;
            }
            finally
            {
                this.Cursor = Cursors.Default;
                this.Enabled = true;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        private void cmbLocalFeatureclasses_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DestinationFCName = this.cmbLocalFeatureclasses.Text;
                DestinationFCName = RCIS.Utility.OtherHelper.GetLeftName(DestinationFCName);  //DestinationFCName.Substring(DestinationFCName.IndexOf("|") + 1, DestinationFCName.Length - DestinationFCName.IndexOf("|") - 1);

                LoadFeatureClassField(m_DestinationWS);

                // 两个一致性的映射 加一下
            }
            catch (Exception ee)
            {

                MessageBox.Show("" + ee.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void gridDestFields_Scroll(object sender, ScrollEventArgs e)
        {
            gridSrcFields.FirstDisplayedScrollingRowIndex = gridDestFields.FirstDisplayedScrollingRowIndex;
            gridSrcFields.HorizontalScrollingOffset = gridDestFields.HorizontalScrollingOffset;
        }

        private void gridSrcFields_Scroll(object sender, ScrollEventArgs e)
        {
            gridDestFields.FirstDisplayedScrollingRowIndex = gridSrcFields.FirstDisplayedScrollingRowIndex;
            gridDestFields.HorizontalScrollingOffset = gridSrcFields.HorizontalScrollingOffset;
        }

        private ArrayList GetOriginFeatureClass(IFeatureClass pFC)
        {
            if (pFC == null) return null;

            this.OriginFCShapeType = pFC.ShapeType.ToString();

            ITable pTable = pFC as ITable;
            sOriginList.Clear();
            for (int i = 0; i < pTable.Fields.FieldCount; i++)
            {
                IField pField = pTable.Fields.get_Field(i);
                if ((pField.Type != esriFieldType.esriFieldTypeBlob) &&
                     (pField.Type != esriFieldType.esriFieldTypeGeometry) &&
                    (pField.Type != esriFieldType.esriFieldTypeOID) &&
                    (pField.Type!=esriFieldType.esriFieldTypeGlobalID) &&
                    (pField.Type!=esriFieldType.esriFieldTypeGUID) &&
                    (pField.Type!=esriFieldType.esriFieldTypeOID) &&
                    (pField.Type!=esriFieldType.esriFieldTypeRaster) 
                    )
                {
                    sOriginList.Add(pField.Name.ToString() + "|" + pField.Type.ToString());
                }

            }
            return sOriginList;
        }

        private void InAFeatureClassForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.OriginWS != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(this.OriginWS);
            }
            if (this.DestinationWS != null)
            {
                if (this.DestinationWS != RCIS.Global.GlobalEditObject.GlobalWorkspace)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(this.DestinationWS);
                }
            }
        }
    }

    

}
