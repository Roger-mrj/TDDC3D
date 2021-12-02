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

using RCIS.Utility;
using RCIS.GISCommon;

namespace TDDC3D.sys
{
    public partial class AltSpatialRefForm : Form
    {
        public AltSpatialRefForm()
        {
            InitializeComponent();
        }

        public  IWorkspace currWs = null;

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }
        ISpatialReference currSR = null;
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
                this.memoEdit1.Text = this.currSR.Name + "\r\n" + this.currSR.Alias + "\r\n" + this.currSR.Remarks.ToString();

            }
            catch (Exception ex)
            {
            }
        }
        

        private ISpatialReference getSRFromTxt()
        {
            string sSrprjFile = this.bePrjFile.Text.Trim();
            ISpatialReferenceFactory spatialReferenceFactory = new SpatialReferenceEnvironmentClass();
            ISpatialReference pSR = SpatialRefHelper.ConstructCoordinateSystem(true, sSrprjFile);
            //修改容差范围
            double dtolerance = 0.0001;
            ISpatialReferenceResolution resolutionTolerance = pSR as ISpatialReferenceResolution;
            resolutionTolerance.set_XYResolution(true, dtolerance);
            resolutionTolerance.set_ZResolution(true, dtolerance);

           
            ISpatialReferenceTolerance spatialReferenceTolerance = pSR as ISpatialReferenceTolerance;
            spatialReferenceTolerance.XYTolerance = dtolerance;


            
            pSR.SetDomain(-999999999999, 999999999999, -999999999999, 999999999999);
            return pSR;
        }
        private void bePrjFile_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "坐标参考文件|*.prj";
            dlg.InitialDirectory = Application.StartupPath + @"\srprj";
            if (dlg.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            this.bePrjFile.Text = dlg.FileName;
            this.currSR = getSRFromTxt();
            System.IO.StreamReader stream = new System.IO.StreamReader(dlg.FileName, System.Text.Encoding.Default);
            this.memoEdit1.Text = stream.ReadToEnd();
            stream.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (currSR == null)
            {
                MessageBox.Show("请选择空间参考！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
           

            try
            {
                IFeatureDataset pDataset = (this.currWs as IFeatureWorkspace).OpenFeatureDataset(RCIS.Global.AppParameters.DATASET_DEFAULT_NAME);
                if  (pDataset != null)
                {
                    //IGeoDataset pGeoDs = pDataset as IGeoDataset;
                    //先 对其下所有要素类 进行空间参考修改
                    IFeatureClassContainer pFeaClassContainer = pDataset as IFeatureClassContainer;
                    for (int i = 0; i < pFeaClassContainer.ClassCount; i++)
                    {
                        IFeatureClass aFeaClass = pFeaClassContainer.get_Class(i);
                        RCIS.GISCommon.GeometryHelper.AlterSpatialReference(aFeaClass, currSR);
                    }

                    RCIS.GISCommon.GeometryHelper.AlterSpatialReference(pDataset as IFeatureDataset, currSR);
                    
                }

                MessageBox.Show("修改完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            this.currSR =SpatialRefHelper.CreateUnKnownSpatialReference();
            try
            {


                IEnumDataset pEnumDs = this.currWs.get_Datasets(esriDatasetType.esriDTFeatureDataset);
                IDataset pDataset = pEnumDs.Next();
                while (pDataset != null)
                {
                    //IGeoDataset pGeoDs = pDataset as IGeoDataset;
                    //先 对其下所有要素类 进行空间参考修改
                    IFeatureClassContainer pFeaClassContainer = pDataset as IFeatureClassContainer;
                    for (int i = 0; i < pFeaClassContainer.ClassCount; i++)
                    {
                        IFeatureClass aFeaClass = pFeaClassContainer.get_Class(i);
                        RCIS.GISCommon.GeometryHelper.AlterSpatialReference(aFeaClass, currSR);
                    }

                    RCIS.GISCommon.GeometryHelper.AlterSpatialReference(pDataset as IFeatureDataset, currSR);
                    pDataset = pEnumDs.Next();
                }

                MessageBox.Show("修改完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
