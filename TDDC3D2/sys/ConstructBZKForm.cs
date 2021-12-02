using System;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using RCIS.GISCommon;
using System.Collections.Generic;

namespace TDDC3D.sys
{
    public partial class ConstructBZKForm : Form
    {
        public Boolean _isOK = false;
        public string _sourGDBPath = "";
        public string _GDBPath = "";
        List<int> geographicList = new List<int>();
        public ConstructBZKForm()
        {
            InitializeComponent();
        }


        //public double dTolerance
        //{
        //    get
        //    {
        //        double d = 0.001;
        //        double.TryParse(this.txtTolerance.Text, out d);
        //        return d;
        //    }
        //}



        ISpatialReference currSR = null;

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (currSR == null)
            {
                MessageBox.Show("请选择空间参考！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (this.beStandFile.Text.Trim() == "")
            {
                MessageBox.Show("输入项不可为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtTolerance.Text))
            {
                MessageBox.Show("容差项不可为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            double dTolerance = double.Parse(txtTolerance.Text);
            if (!(dTolerance > 0 && dTolerance < 1))
            {
                MessageBox.Show("容差值不在合理范围！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string destDir = this.beStandFile.Text;
            if (System.IO.Directory.Exists(destDir))
            {
                MessageBox.Show("目标数据库已经存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            this.Cursor = Cursors.WaitCursor;
            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("正在创建数据库，请稍后。", "提示");
            try
            {
                wait.Show();
                IWorkspaceFactory pWorkspaceFactory = new ESRI.ArcGIS.DataSourcesGDB.FileGDBWorkspaceFactoryClass();
                pWorkspaceFactory.Create(System.IO.Path.GetDirectoryName(destDir), System.IO.Path.GetFileName(destDir), null, 0);
                ISpatialReferenceTolerance pSRT = currSR as ISpatialReferenceTolerance;
                pSRT.XYTolerance = dTolerance;
                pSRT.ZTolerance = dTolerance;
                pSRT.MTolerance = dTolerance;
                ISpatialReferenceResolution pSRR = currSR as ISpatialReferenceResolution;
                pSRR.XYResolution[true] = dTolerance / 10;
                pSRR.ZResolution[true] = dTolerance / 10;
                pSRR.MResolution = dTolerance / 10;
                IWorkspace pTarWorkspace = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(destDir);
                //IFeatureDataset pTDDCFeatureDataset = (pTarWorkspace as IFeatureWorkspace).CreateFeatureDataset("TDDC", currSR);

                string sourceDir = RCIS.Global.AppParameters.ConfPath + @"\standard.gdb";
                IWorkspace pSourWorkspace = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(sourceDir);
                IEnumDataset pEnumDataset = pSourWorkspace.get_Datasets(esriDatasetType.esriDTAny);
                pEnumDataset.Reset();
                IDataset pDataset = pEnumDataset.Next();
                while (pDataset != null)
                {
                    switch (pDataset.Type)
                    {
                        case esriDatasetType.esriDTFeatureClass:
                            RCIS.GISCommon.DatabaseHelper.CopyPasteGeodatabaseData(pSourWorkspace, pTarWorkspace, pDataset.Name, pDataset.Type);
                            IClassSchemaEdit2 pClassSchemaEdit = (pTarWorkspace as IFeatureWorkspace).OpenFeatureClass(pDataset.Name) as IClassSchemaEdit2;
                            pClassSchemaEdit.AlterAliasName((pDataset as IFeatureClass).AliasName);
                            break;
                        case esriDatasetType.esriDTFeatureDataset:
                            if (pDataset.Name.ToLower() != "tf")
                            {
                                IFeatureDataset pTDDCFeatureDataset = (pTarWorkspace as IFeatureWorkspace).CreateFeatureDataset(pDataset.Name, currSR);
                                IFeatureClassContainer pFCC = pDataset as IFeatureClassContainer;
                                IEnumFeatureClass pEnumFC = pFCC.Classes;
                                pEnumFC.Reset();
                                IFeatureClass pFeatureClass = pEnumFC.Next();
                                while (pFeatureClass != null)
                                {
                                    RCIS.GISCommon.EsriDatabaseHelper.ConvertFeatureClass2FeatureDataset(pSourWorkspace, pTarWorkspace, (pFeatureClass as IDataset).Name, (pFeatureClass as IDataset).Name, pTDDCFeatureDataset, null);
                                    IClassSchemaEdit2 pClassSchemaEdit2 = (pTarWorkspace as IFeatureWorkspace).OpenFeatureClass((pFeatureClass as IDataset).Name) as IClassSchemaEdit2;
                                    pClassSchemaEdit2.AlterAliasName(pFeatureClass.AliasName);
                                    pFeatureClass = pEnumFC.Next();
                                }
                            }
                            else
                            {
                                RCIS.GISCommon.DatabaseHelper.CopyPasteGeodatabaseData(pSourWorkspace, pTarWorkspace, pDataset.Name, pDataset.Type);
                            }
                            break;
                        default:
                            RCIS.GISCommon.DatabaseHelper.CopyPasteGeodatabaseData(pSourWorkspace, pTarWorkspace, pDataset.Name, pDataset.Type);
                            break;
                    }
                    pDataset = pEnumDataset.Next();
                }
                //System.IO.Directory.CreateDirectory(destDir);
                //RCIS.Utility.File_DirManipulate.FolderCopy(sourceDir, destDir);
                //IWorkspace standWs = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(destDir);
                ////获取原来的空间参考，及容差等

                //double oldd = 0.001;
                //double oldr = 0.0001;
                ////修改第一个数据集
                //IFeatureWorkspace pFeaWs = standWs as IFeatureWorkspace;
                //IFeatureDataset pFeaDS = pFeaWs.OpenFeatureDataset(RCIS.Global.AppParameters.DATASET_DEFAULT_NAME);
                //if (pFeaDS != null)
                //{
                //    //IFeatureClassContainer pFeaClassContainer = pFeaDS as IFeatureClassContainer;
                //    //for (int i = 0; i < pFeaClassContainer.ClassCount; i++)
                //    //{
                //    //    IFeatureClass aFeaClass = pFeaClassContainer.get_Class(i);
                //    //    RCIS.GISCommon.GeometryHelper.AlterSpatialReference(aFeaClass, currSR,this.dTolerance);
                //    //}
                //    ISpatialReference oldSR = (pFeaDS as IGeoDataset).SpatialReference;
                //    ISpatialReferenceTolerance oldSrTolerance = oldSR as ISpatialReferenceTolerance;
                //    oldd = oldSrTolerance.XYTolerance;
                //    ISpatialReferenceResolution oldResolution = oldSR as ISpatialReferenceResolution;
                //    oldr = oldResolution.get_XYResolution(true);


                //    ISpatialReferenceResolution newResolution = currSR as ISpatialReferenceResolution;
                //    newResolution.set_XYResolution(true,oldr);
                //    newResolution.set_ZResolution(true,oldr);


                //    ISpatialReferenceTolerance newTolerance = currSR as ISpatialReferenceTolerance;
                //    newTolerance.XYTolerance = oldd;
                //    newTolerance.ZTolerance = oldd;
                //    newTolerance.MTolerance = oldd;
                //    currSR.SetDomain(-9999999999999, 9999999999999, -9999999999999, 9999999999999);

                //    IFeatureClassContainer pClassContainer = pFeaDS as IFeatureClassContainer;
                //    for (int kk = 0; kk < pClassContainer.ClassCount; kk++)
                //    {
                //        IFeatureClass aFeaClass = pClassContainer.get_Class(kk);
                //        RCIS.GISCommon.GeometryHelper.AlterSpatialReference(aFeaClass, currSR);

                //    }


                //    RCIS.GISCommon.GeometryHelper.AlterSpatialReference(pFeaDS, currSR);

                //    //double txmin, txmax, tymin, tymax = 0;
                //    //currSR.GetDomain(out txmin, out txmax, out tymin, out tymax);

                //    //ISpatialReference pSR = (pFeaDS as IGeoDataset).SpatialReference;
                //    //ISpatialReferenceTolerance spatialReferenceTolerance = pSR as ISpatialReferenceTolerance;
                //    //double newd = spatialReferenceTolerance.XYTolerance;



                //}

                ////IEnumDataset pEnumDs = standWs.get_Datasets(esriDatasetType.esriDTFeatureDataset);
                ////IDataset pDataset = pEnumDs.Next();
                ////while (pDataset != null)
                ////{
                ////    //IGeoDataset pGeoDs = pDataset as IGeoDataset;
                ////    //先 对其下所有要素类 进行空间参考修改
                ////    IFeatureClassContainer pFeaClassContainer = pDataset as IFeatureClassContainer;
                ////    for (int i = 0; i < pFeaClassContainer.ClassCount; i++)
                ////    {
                ////        IFeatureClass aFeaClass = pFeaClassContainer.get_Class(i);
                ////        RCIS.GISCommon.GeometryHelper.AlterSpatialReference(aFeaClass, currSR);
                ////    }

                ////    RCIS.GISCommon.GeometryHelper.AlterSpatialReference(pDataset as IFeatureDataset, currSR);
                ////    pDataset = pEnumDs.Next();
                ////}
                wait.Close();
                this.Cursor = Cursors.Default;
                MessageBox.Show("创建完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (chkOpen.Checked)
                {
                    _isOK = true;
                    _GDBPath = destDir;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                wait.Close();
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message);
            }

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            //导入空间参考
            AddDataForm frm = new AddDataForm();
            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                return;
            ILayer currLyr = frm.resultLyr;
            IFeatureLayer currFeaLyr = currLyr as IFeatureLayer;
            if (currFeaLyr == null)
                return;
            try
            {
                IFeatureClass srcClass = currFeaLyr.FeatureClass;
                this.currSR = (srcClass as IGeoDataset).SpatialReference;
                //ISpatialReferenceResolution resolutionTolerance = currSR as ISpatialReferenceResolution;
                //resolutionTolerance.set_XYResolution(true, this.dTolerance / 10);
                //resolutionTolerance.set_ZResolution(true, this.dTolerance / 10);

                //ISpatialReferenceTolerance spatialReferenceTolerance = currSR as ISpatialReferenceTolerance;
                //spatialReferenceTolerance.XYTolerance = this.dTolerance;
                //spatialReferenceTolerance.ZTolerance = this.dTolerance;
                //spatialReferenceTolerance.MTolerance = this.dTolerance;

                this.memoEdit1.Text = this.currSR.Name + "\r\n" + this.currSR.Alias + "\r\n" + this.currSR.Remarks.ToString();

            }
            catch (Exception ex)
            {
            }

        }

        private void btnSelectCoord_Click(object sender, EventArgs e)
        {
            try
            {
                FrmCoordinateSystem frm = new FrmCoordinateSystem();
                if (frm.ShowDialog() == DialogResult.Cancel)
                    return;
                currSR = frm.currSR;
                if (currSR != null)
                {
                    if (currSR.FactoryCode == 4326)
                    {

                        this.cmbCoordSelect.SelectedIndex = 0;
                        return;
                    }

                    string addText = "EPSG:" + currSR.FactoryCode + " - " + currSR.Name;

                    if (!this.cmbCoordSelect.Properties.Items.Contains(addText))
                    {
                        this.cmbCoordSelect.Properties.Items.Add(addText);
                    }
                    this.cmbCoordSelect.SelectedIndex = this.cmbCoordSelect.Properties.Items.IndexOf(addText);
                }
            }
            catch
            {
                MessageBox.Show("选择坐标出错。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void cmbCoordSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            string currText = cmbCoordSelect.Text.ToString();
            if (string.IsNullOrEmpty(currText))
                return;

            try
            {
                int wkid;
                if (currText.Contains("默认坐标系参考"))
                {
                    wkid = 4326;
                }
                else
                {
                    wkid = int.Parse(currText.Split('-')[0].Trim().Split(':')[1]);
                }

                ISpatialReferenceFactory spatialReferenceFactory = new SpatialReferenceEnvironmentClass();
                ISpatialReference spatialReference;
                if (geographicList.Contains(wkid))
                {
                    spatialReference = spatialReferenceFactory.CreateGeographicCoordinateSystem(wkid);
                }
                else
                {
                    spatialReference = spatialReferenceFactory.CreateProjectedCoordinateSystem(wkid);
                }

                this.currSR = spatialReference;
                this.memoEdit1.Text = this.currSR.Name + "\r\n" + this.currSR.Alias + "\r\n" + this.currSR.Remarks.ToString();
            }
            catch
            {
                MessageBox.Show("初始化参考系出错。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }

        private void ConstructBZKForm_Load(object sender, EventArgs e)
        {

            //初始化默认参考系
            geographicList.Add(4214);
            geographicList.Add(4490);
            geographicList.Add(4326);
            geographicList.Add(4610);

            cmbCoordSelect.Properties.Items.Add("默认坐标系参考：EPSG:4326 - GCS_WGS_1984");
            cmbCoordSelect.SelectedIndex = 0;

            btnSelectCoord.ImageIndex = 0;
        }

        private void beStandFile_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            string sFile = dlg.FileName;
            if (!sFile.ToUpper().EndsWith(".GDB"))
            {
                sFile += ".gdb";
            }
            this.beStandFile.Text = sFile;
        }

       
    }
}
