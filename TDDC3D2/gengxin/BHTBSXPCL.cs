using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;
using System.Collections;
using ESRI.ArcGIS.Geometry;

namespace TDDC3D.gengxin
{
    public partial class FrmBHTBSXPCL : Form
    {
        public FrmBHTBSXPCL()
        {
            InitializeComponent();
        }
        public IWorkspace currWs = null;
        private IFeatureClass featureClass = null;

        private void btnSelectBHTB_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "shapefile(*.shp)|*.shp|All files(*.*)|*.*";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                btnSelectBHTB.Text = openFile.FileName;
                featureClass = null;
            }
        }

        private void btnTBCheck_Click(object sender, EventArgs e)
        {

            

            if (btnSelectBHTB.Text == "")
            {
                MessageBox.Show("请先选择统一时点更新图斑！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string[] arr = { "bsm", "dlbm", "dlmc", "qsxz", "qsdwdm", "qsdwmc", "zldwdm", "zldwmc", "kcdlbm", "kcxs", "kcmj", "tbdlmj", "gdlx", "gdpdjb", "xzdwkd", "tbxhdm", "tbxhmc", "zzsxdm", "zzsxmc", "gddb", "frdbs", "czcsxm", "mssm", "hdmc" };
            //string[] arr = { };
            List<string> fields = new List<string>();
            string shpFile = btnSelectBHTB.Text;
            FileInfo fileInfo = new FileInfo(shpFile);
            IWorkspaceFactory pWorkspaceFactory = new ShapefileWorkspaceFactoryClass();
            IWorkspace pWorkspace = pWorkspaceFactory.OpenFromFile(fileInfo.DirectoryName, 0);
            IFeatureWorkspace pFeaWorkspace = pWorkspace as IFeatureWorkspace;
            IFeatureClass pFeatureclass = pFeaWorkspace.OpenFeatureClass(fileInfo.Name);
            IFields pFields = pFeatureclass.Fields;
            for (int i = 0; i < pFeatureclass.Fields.FieldCount; i++)
            {
                IField pField = pFields.get_Field(i);
                fields.Add(pField.Name.ToLower());
            }
            string missFields = "";
            for (int i = 0; i < arr.Length; i++)
            {
                if (!fields.Contains(arr[i]))
                {
                    missFields += arr[i] + ",";
                }
            }
            
            if (missFields.Length > 0)
                MessageBox.Show("缺少" + missFields + "必要字段！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                if (!fields.Contains("bgxw"))
                {
                    IField pField = new FieldClass();
                    IFieldEdit pFieldEdit = pField as IFieldEdit;
                    pFieldEdit.AliasName_2 = "变更行为";
                    pFieldEdit.Name_2 = "BGXW";
                    pFieldEdit.Length_2 = 1;
                    pFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                    IClass pClass = pFeatureclass as IClass;
                    pClass.AddField(pField);
                }
                featureClass = pFeatureclass;
                MessageBox.Show("检查完毕，变化图斑数据结构符合要求！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


        }

        private void btnEditBGXW_Click(object sender, EventArgs e)
        {
            if (featureClass == null)
            {
                MessageBox.Show("请先进行变化图斑检查！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            //获取三调标准库shp数据
            IFeatureClass sdFeatureclass = null;
            IFeatureWorkspace pFeatureWorkspace = currWs as IFeatureWorkspace;
            try
            {
                sdFeatureclass = pFeatureWorkspace.OpenFeatureClass("DLTB");
            }
            catch
            {
                MessageBox.Show("未找到标准库，无法对比！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("正在填写变更行为，请稍后。", "提示");
            wait.Show();
            long maxNum = RCIS.GISCommon.FeatureHelper.GetMaxStringNumberOrderBy(sdFeatureclass,"BSM");
            //变更图斑的featureclass
            IFeatureCursor pFeaCursor = featureClass.Update(null, true);
            
            IFeature pFeature = pFeaCursor.NextFeature();
            ISpatialFilter pSpaFil = null;
            while (pFeature != null)
            {
                int pFieldNum = featureClass.FindField("BGXW");
                if (!string.IsNullOrWhiteSpace(pFeature.get_Value(pFieldNum).ToString()))
                {
                    pFeature = pFeaCursor.NextFeature();
                    continue;
                }
                pSpaFil = new SpatialFilterClass();
                pSpaFil.Geometry = pFeature.ShapeCopy;
                pSpaFil.SpatialRel = esriSpatialRelEnum.esriSpatialRelRelation;
                pSpaFil.SpatialRelDescription = "TFFFTFFFT";
                IFeatureCursor txFeatureCursor = sdFeatureclass.Search(pSpaFil, false);
                IFeature txFeture = txFeatureCursor.NextFeature();
                if (txFeture != null)
                {
                    pFeature.set_Value(pFieldNum, "1");//属性变更
                    pFeature.set_Value(pFeature.Fields.FindField("BSM"),txFeture.get_Value(txFeture.Fields.FindField("BSM")));
                }
                else
                {
                    pFeature.set_Value(pFieldNum, "2");//图形变更
                    maxNum += 1;
                    pFeature.set_Value(pFeature.Fields.FindField("BSM"), maxNum);
                }
                RCIS.Utility.OtherHelper.ReleaseComObject(txFeatureCursor);
                pFeaCursor.UpdateFeature(pFeature);
                pFeature = pFeaCursor.NextFeature();
            }
            RCIS.Utility.OtherHelper.ReleaseComObject(pFeaCursor);
            wait.Close();
            MessageBox.Show("图斑变更行为填写完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private List<string> GetUniqueValuesByFeatureClass(IFeatureClass pFeatureClass, string FieldName)
        {
            List<string> arrValues = new List<string>();
            DataStatisticsClass pDataStatistics = new DataStatisticsClass();
            pDataStatistics.Cursor = pFeatureClass.Search(null, false) as ICursor;
            pDataStatistics.Field = FieldName;
            IEnumerator pEnum = pDataStatistics.UniqueValues;
            while (pEnum.MoveNext())
            {
                string temp = pEnum.Current.ToString();
                arrValues.Add(temp);
            }
            return arrValues;
        }
        
        private void btnEditTBH_Click(object sender, EventArgs e)
        {
            if (featureClass == null)
            {
                MessageBox.Show("请先进行变更图斑数据结构检查！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("正在编写图斑编号，请稍后。", "提示");
            wait.Show();
            IFeatureWorkspace pFeatureworkspace = currWs as IFeatureWorkspace;
            IFeatureClass pFeatureclass = pFeatureworkspace.OpenFeatureClass("DLTB");
            List<string> arr = new List<string>();
            arr = GetUniqueValuesByFeatureClass(featureClass, "ZLDWDM");
            for (int i = 0; i < arr.Count; i++)
            {
                string search = "ZLDWDM='" + arr[i] + "'";
                string write = "ZLDWDM='" + arr[i] + "'";
                long max = RCIS.GISCommon.FeatureHelper.GetMaxStringNumberEveryOne(pFeatureclass, "TBBH",search);
                if (max == 0)
                    max = 1;
                EditFieldFromMaxNum(max, featureClass, "TBBH", write);
            }
            wait.Close();
            MessageBox.Show("图斑编号编写完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        

        private void UpdateStatus(string txt)
        {
            info.Text = DateTime.Now.ToString() + ":" + txt + "\r\n" + info.Text;
            Application.DoEvents();
        }

        
    }
}
