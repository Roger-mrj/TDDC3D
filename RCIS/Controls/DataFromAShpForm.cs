using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Collections;

using System.Runtime.InteropServices;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using RCIS.GISCommon;

namespace RCIS.Controls
{
    public partial class DataFromAShpForm : Form
    {
        public DataFromAShpForm()
        {
            InitializeComponent();
        }

        IWorkspace OriginWS;//Դ���ݵ�WS
        IWorkspace m_DestinationWS;//Ŀ�����ݵ�WS

        string OriginFCName;
        string DestinationFCName;

       


        ArrayList sOriginList = new ArrayList();

        //ԴҪ���������

        string OriginFCShapeType = "";
        //Ŀ��Ҫ���������
        string DestinationFCShapeType = "";


        public IWorkspace DestinationWS
        {
            get { return m_DestinationWS; }
            set { m_DestinationWS = value; }
        }
        //���ر������ݵ�WS����ʱ���WS��ֵ��DestinationWS

        #region ���ر�������
        private void LoadData()
        {
            this.cmbLocalFeatureclasses.Properties.Items.Clear();

            IFeatureWorkspace pFWS = m_DestinationWS as IFeatureWorkspace;
            IEnumDataset pEnumDataset = m_DestinationWS.get_Datasets(esriDatasetType.esriDTFeatureDataset);
            IDataset pDateset = pEnumDataset.Next();

            //IDataset pDateset = pFWS.OpenFeatureDataset("KFQ");

            while  (pDateset != null)
            {
                IFeatureClassContainer pFeatureClsConner = (IFeatureClassContainer)pDateset;
                for (int i = 0; i < pFeatureClsConner.ClassCount; i++)
                {
                    IFeatureClass pFeauteClass = pFeatureClsConner.get_Class(i);


                    this.cmbLocalFeatureclasses.Properties.Items.AddRange(new object[]{
														  pFeauteClass.AliasName.ToString()+"|"+
                        this.GetFeatureLayerTableName(pFeauteClass)
														  });



                }
                pDateset = pEnumDataset.Next();

            }


        }

        #endregion


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

                MessageBox.Show("" + ee.Message, "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return "";
            }
        }

        private void DataFromAShpForm_Load(object sender, EventArgs e)
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
        //ת��
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (this.btSounceFC.Text == "")
            {
                this.Cursor = Cursors.Arrow;
                MessageBox.Show("ԴҪ����·��Ϊ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
           
                if (this.cmbLocalFeatureclasses.Text == "")
                {
                    this.Cursor = Cursors.Arrow;
                    MessageBox.Show("����Ҫ����Ϊ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            

            if (this.OriginFCShapeType != this.DestinationFCShapeType)
            {
                this.Cursor = Cursors.Arrow;
                MessageBox.Show("ת����Ҫ���༸�����Ͳ�һ�£����飡", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (this.CheckType() == false)
            {
                this.Cursor = Cursors.Arrow;
                MessageBox.Show("ת���ֶ����Ͳ�һ�£����飡", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.tabControl1.SelectedIndex = 1;
                return;
            }

            
            this.Cursor = Cursors.WaitCursor;

            try
            {

                

                if (this.gridDestFields.Rows.Count == 0) return;


                AddFieldValue(OriginWS, DestinationWS, OriginFCName, DestinationFCName);

            
                //this.ConvertFeatureClass(OriginWS, DestinationWS, OriginFCName, "new_Shapefile");

                
                MessageBox.Show("ת���ɹ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Cursor = Cursors.Arrow;
            }
            catch (Exception ee)
            {

                MessageBox.Show("" + ee.Message, "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Cursor = Cursors.Arrow;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }


        #region ת��Ҫ����

        //public void ConvertFeatureClass(IWorkspace sourceWorkspace, IWorkspace targetWorkspace, string nameOfSourceFeatureClass, string nameOfTargetFeatureClass)
        //{
        //    try
        //    {
        //        //create source workspace name     
        //        IDataset sourceWorkspaceDataset = (IDataset)sourceWorkspace;
        //        IWorkspaceName sourceWorkspaceName = (IWorkspaceName)sourceWorkspaceDataset.FullName;
        //        //create source dataset name 
        //        IFeatureClassName sourceFeatureClassName = new FeatureClassNameClass();
        //        IDatasetName sourceDatasetName = (IDatasetName)sourceFeatureClassName;
        //        sourceDatasetName.WorkspaceName = sourceWorkspaceName;
        //        sourceDatasetName.Name = nameOfSourceFeatureClass;
        //        //create target workspace name    
        //        IDataset targetWorkspaceDataset = (IDataset)targetWorkspace;
        //        IWorkspaceName targetWorkspaceName = (IWorkspaceName)targetWorkspaceDataset.FullName;
        //        //create target dataset name     
        //        IFeatureClassName targetFeatureClassName = new FeatureClassNameClass();
        //        IDatasetName targetDatasetName = (IDatasetName)targetFeatureClassName;
        //        targetDatasetName.WorkspaceName = targetWorkspaceName;
        //        targetDatasetName.Name = nameOfTargetFeatureClass;
        //        //Open input Featureclass to get field definitions. 
        //        ESRI.ArcGIS.esriSystem.IName sourceName = (ESRI.ArcGIS.esriSystem.IName)sourceFeatureClassName;
        //        IFeatureClass sourceFeatureClass = (IFeatureClass)sourceName.Open();
        //        // IGeoDataset pGD = sourceFeatureClass as IGeoDataset;
        //        // pGD.SpatialReference = OriginSR;
        //        //Validate the field names because you are converting between different workspace types.       
        //        IFieldChecker fieldChecker = new FieldCheckerClass();
        //        IFields targetFeatureClassFields;
        //        IFields sourceFeatureClassFields = sourceFeatureClass.Fields;
        //        IEnumFieldError enumFieldError;
        //        // Most importantly set the input and validate workspaces!    
        //        fieldChecker.InputWorkspace = sourceWorkspace;
        //        fieldChecker.ValidateWorkspace = targetWorkspace;
        //        fieldChecker.Validate(sourceFeatureClassFields, out enumFieldError, out targetFeatureClassFields);
        //        // Loop through the output fields to find the geomerty field   
        //        IField geometryField;

        //        for (int i = 0; i < targetFeatureClassFields.FieldCount; i++)
        //        {
        //            if (targetFeatureClassFields.get_Field(i).Type == esriFieldType.esriFieldTypeGeometry)
        //            {
        //                geometryField = targetFeatureClassFields.get_Field(i);
        //                // Get the geometry field's geometry defenition           
        //                IGeometryDef geometryDef = geometryField.GeometryDef;
        //                //Give the geometry definition a spatial index grid count and grid size          
        //                IGeometryDefEdit targetFCGeoDefEdit = (IGeometryDefEdit)geometryDef;
        //                targetFCGeoDefEdit.GridCount_2 = 1;
        //                targetFCGeoDefEdit.set_GridSize(0, 0);
        //                //Allow ArcGIS to determine a valid grid size for the data loaded           
        //                targetFCGeoDefEdit.SpatialReference_2 = geometryField.GeometryDef.SpatialReference;
        //                // we want to convert all of the features      
        //                IQueryFilter queryFilter = new QueryFilterClass();
        //                queryFilter.WhereClause = "";
        //                // Load the feature class            
        //                IFeatureDataConverter fctofc = new FeatureDataConverterClass();
        //                IEnumInvalidObject enumErrors = fctofc.ConvertFeatureClass(sourceFeatureClassName, queryFilter, null, targetFeatureClassName, geometryDef, targetFeatureClassFields, "", 1000, 0);
        //                break;
        //            }
        //        }

        //    }
        //    catch (Exception ee)
        //    {

        //        MessageBox.Show("" + ee.Message, "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    }

        //}
        
        #endregion



        #region ת�����Ѿ����ڵ�FeatureClass

        private void AddFieldValue(IWorkspace sourceWorkspace,
            IWorkspace targetWorkspace,
            string nameOfSourceFeatureClass,
            string nameOfTargetFeatureClass
            )
        {
            try
            {
                if (sourceWorkspace == null) return;
                if (targetWorkspace == null) return;
                IFeatureWorkspace pSourceFWS = (IFeatureWorkspace)sourceWorkspace;
                IFeatureWorkspace pTargetFWS = (IFeatureWorkspace)targetWorkspace;
                IFeatureClass pSourceTable = pSourceFWS.OpenFeatureClass(nameOfSourceFeatureClass);
                IFeatureClass pTargetTable = pTargetFWS.OpenFeatureClass(nameOfTargetFeatureClass);

                ISpatialReference pSP = ((pTargetTable as IDataset) as IGeoDataset).SpatialReference;

                IFeatureCursor aCursor = pSourceTable.Search(null, false);
                string nameOfSourceFieldName = "";//ԴҪ�����ֶ���

                string nameOfTargetFieldName = "";//Ŀ��Ҫ�����ֶ���
                int SourceFieldIndex;//ԴҪ�����ֶ�������

                int TargetFieldIndex;//Ŀ��Ҫ�����ֶ�������
                //string sSourceValue = "";//ԴҪ�����ֶ�������Ӧ������ֵ

                IFeature row = aCursor.NextFeature();
                int aCount = pSourceTable.FeatureCount(null);
                if (aCount <= 0) return;

                string aText = "���ڵ���" + pSourceTable.AliasName + "��" + pTargetTable.AliasName;
                status.Text = aText;
                Application.DoEvents();

                IWorkspaceEdit aWKEdit = pTargetFWS as IWorkspaceEdit;
                aWKEdit.StartEditing(false);
                aWKEdit.StartEditOperation();
                int aStep = aCount / 10 + 1;
                int order = 0;

                #region   //��ȡ�ֶζ�Ӧ��ϵ.
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

                try
                {
                    while (row != null)
                    {
                        if (order++ % aStep == 0)
                        {
                            aWKEdit.StopEditOperation();
                            aWKEdit.StopEditing(true);
                            aWKEdit.StartEditing(false);
                            aWKEdit.StartEditOperation();
                           

                        }
                        try
                        {
                            #region //����Ҫ��
                            IFeature pTargetRow = pTargetTable.CreateFeature();
                            FeatureHelper.CopyRow(row, pTargetRow, aSrcFields, aDestFields);
                            IGeometry srcShp = row.ShapeCopy;
                            try
                            {
                                srcShp.Project(pSP);
                            }
                            catch { }
                            pTargetRow.Shape = srcShp;
                            pTargetRow.Store();

                            string aTaskCap = aText + "(" + order + "/" + aCount + ")";
                            status.Text = aTaskCap;
                            Application.DoEvents();

                            #endregion 

                        }
                        catch(Exception ex)
                        { }
                        row = aCursor.NextFeature();
                    }
                    aWKEdit.StopEditOperation();
                    aWKEdit.StopEditing(true);
                }
                catch { aWKEdit.StopEditing(false); }
                status.Text = "";

                Marshal.ReleaseComObject(aCursor);
                Marshal.ReleaseComObject(pSourceTable);
                Marshal.ReleaseComObject(pTargetTable);

            }
            catch (Exception ee)
            {
                MessageBox.Show("" + ee.Message, "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        #endregion

        #region  �ж�ƥ���ֶε������Ƿ�һ��



        private bool CheckType()
        {
            //if(this.dataGridView1.Rows.Count==0) return false;
            //if(this.dataGridView2.Rows.Count==0) return false;
            //string sFieldType = "";
            //for (int i = 0; i < this.dataGridView2.Rows.Count; i++)
            //{
            //    if (this.dataGridView2[0, i].Value != null)
            //    {//2��Դ 1��Ŀ��
            //        sFieldType = this.dataGridView2[0, i].Value.ToString();
            //        sFieldType = sFieldType.Substring(sFieldType.IndexOf("|") + 1, sFieldType.Length - sFieldType.IndexOf("|")-1);
            //        if (sFieldType != this.dataGridView1[1, i].Value.ToString())
            //        {
            //            return false;
            //        }
            //    }
            //}
            return true;
        }

        #endregion

        #region ����·������һ��WORKSPACE
        private IWorkspace GetWorkSpace(string strPath)
        {

            IWorkspaceFactory workspaceFactory = new ESRI.ArcGIS.DataSourcesFile.ShapefileWorkspaceFactoryClass();
            return workspaceFactory.OpenFromFile(strPath, 0);

        }

        #endregion
        //����Դ�����ֶ�����ComboBox
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

        #region ����ԴҪ���ൽArrayList
        //private ArrayList GetOriginFeatureClass(IWorkspace pWS)
        //{
        //    if (pWS == null) return null;


        //    IFeatureWorkspace pFWS = pWS as IFeatureWorkspace;

        //    if ((OriginFCName == "") || (OriginFCName == null)) return null;
        //    IFeatureClass pFC = pFWS.OpenFeatureClass(OriginFCName);

        //    this.OriginFCShapeType = pFC.ShapeType.ToString();

        //    ITable pTable = pFC as ITable;

        //    sOriginList.Clear();
        //    for (int i = 0; i < pTable.Fields.FieldCount; i++)
        //    {


        //        IField pField = pTable.Fields.get_Field(i);
        //        if ((pField.Type != esriFieldType.esriFieldTypeBlob) &&
        //             (pField.Type != esriFieldType.esriFieldTypeGeometry) &&
        //            (pField.Type != esriFieldType.esriFieldTypeOID))
        //        {
        //            sOriginList.Add(pField.AliasName.ToString() + "|" + pField.Type.ToString());
        //        }

        //    }
        //    return sOriginList;
        //}
        #endregion

        private void btSounceFC_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            //OpenFileDialog dlg = new OpenFileDialog();
            //dlg.Title = "��ѡ��ԴҪ�����ļ�";

            //dlg.Filter = "���ݿ��ļ�(*.shp)|*.shp";


            //dlg.RestoreDirectory = true;

            //if (dlg.ShowDialog() == DialogResult.OK)
            //{
            //    this.btSounceFC.Text = dlg.FileName.ToString();

            //    OriginWS = this.GetWorkSpace(System.IO.Path.GetDirectoryName(dlg.FileName.ToString()));
            //    OriginFCName = System.IO.Path.GetFileNameWithoutExtension(dlg.FileName.ToString());

            //    //��ʼ�� ԭʼҪ�����ֶ��б�

            //    LoadOriginFCToComboBox(GetOriginFeatureClass(OriginWS));
            //}
        }

        #region ����Ŀ��Ҫ������ֶ�����Datagridview



        private void AutoArange()
        {
            
        }


        //����Ŀ��Ҫ���� �ֶ�
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

                MessageBox.Show("" + ee.Message, "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion

        private void cmbLocalFeatureclasses_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DestinationFCName = this.cmbLocalFeatureclasses.Text;
                DestinationFCName = DestinationFCName.Substring(DestinationFCName.IndexOf("|") + 1, DestinationFCName.Length - DestinationFCName.IndexOf("|") - 1);

                LoadFeatureClassField(m_DestinationWS);

                // ����һ���Ե�ӳ�� ��һ��
            }
            catch (Exception ee)
            {

                MessageBox.Show("" + ee.Message, "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dataGridView1_Scroll(object sender, ScrollEventArgs e)
        {
            gridSrcFields.FirstDisplayedScrollingRowIndex = gridDestFields.FirstDisplayedScrollingRowIndex;
            gridSrcFields.HorizontalScrollingOffset = gridDestFields.HorizontalScrollingOffset;

            
        }

        private void dataGridView2_Scroll(object sender, ScrollEventArgs e)
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
                    (pField.Type != esriFieldType.esriFieldTypeOID))
                {
                    sOriginList.Add(pField.Name.ToString() + "|" + pField.Type.ToString());
                }

            }
            return sOriginList;
        }

        private void ѡ��Ҫ����_Click(object sender, EventArgs e)
        {
            RCIS.GISCommon.AddDataForm frm = new AddDataForm();
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



        }
    }
}