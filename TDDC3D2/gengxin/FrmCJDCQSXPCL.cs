using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using System.IO;
using ESRI.ArcGIS.DataSourcesFile;
using System.Collections;

namespace TDDC3D.gengxin
{
    public partial class FrmCJDCQSXPCL : Form
    {
        public FrmCJDCQSXPCL()
        {
            InitializeComponent();
        }

        public IWorkspace currWs = null;
        private IFeatureClass featureClass = null;

        private void btnJGJC_Click(object sender, EventArgs e)
        {
            if (txtCJDCQBH.Text == "")
            {
                MessageBox.Show("请先选择统一时点更新村级调查区！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string[] arr = { "bsm", "ysdm", "zldwdm","zldwmc", "mssm", "hdmc", "dcmj", "jsmj" };
            //string[] arr = { };
            List<string> fields = new List<string>();
            string shpFile = txtCJDCQBH.Text;
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
            List<string> missFields = new List<string>();
            for (int i = 0; i < arr.Length; i++)
            {
                if (!fields.Contains(arr[i]))
                {
                    missFields.Add(arr[i]);
                }
            }

            if (missFields.Count > 0)
                MessageBox.Show("缺少" + string.Join(",", missFields.ToArray()) + "必要字段！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                MessageBox.Show("检查完毕，村级调查区数据结构符合要求！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void txtCJDCQBH_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "shapefile(*.shp)|*.shp|All files(*.*)|*.*";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                txtCJDCQBH.Text = openFile.FileName;
                featureClass = null;
            }
        }

        private void btnEditBGXW_Click(object sender, EventArgs e)
        {
            if (featureClass == null)
            {
                MessageBox.Show("请先进行结构检查！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //获取三调标准库shp数据
            IFeatureClass sdFeatureclass = null;
            IFeatureWorkspace pFeatureWorkspace = currWs as IFeatureWorkspace;
            try
            {
                sdFeatureclass = pFeatureWorkspace.OpenFeatureClass("CJDCQ");
            }
            catch
            {
                MessageBox.Show("未找到标准库，无法对比！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            UpdateStatus("正在填写变更行为");
            long maxNum = RCIS.GISCommon.FeatureHelper.GetMaxStringNumberOrderBy(sdFeatureclass, "BSM");
            
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
                    List<string> sdzldwdm = GetUniqueValuesByFeatureClass(sdFeatureclass, "ZLDWDM");
                    if (!sdzldwdm.Contains(pFeature.get_Value(pFeature.Fields.FindField("ZLDWDM")).ToString()))
                    {
                        pFeature.set_Value(pFieldNum, "1");//属性变更
                        pFeature.set_Value(pFeature.Fields.FindField("BSM"), txFeture.get_Value(txFeture.Fields.FindField("BSM")));
                    }
                    else
                    {
                        pFeature.set_Value(pFieldNum, "2");//图形变更
                        maxNum += 1;
                        pFeature.set_Value(pFeature.Fields.FindField("BSM"), maxNum);
                    }
                    //string zldwdm = txFeture.get_Value(txFeture.Fields.FindField("ZLDWDM")).ToString();
                    //IQueryFilter pQueryFilter = new QueryFilter();
                    //pQueryFilter.WhereClause = "ZLDWDM='"+zldwdm+"'";
                    //IFeatureCursor pFeatureCursor = sdFeatureclass.Search(pQueryFilter,true);
                    //IFeature feature;
                    //int temp = 0;
                    //while ((feature = pFeatureCursor.NextFeature())!=null)
                    //{
                    //    temp += 1;
                    //}
                    //if (temp > 1)
                    //{
                    //    pFeature.set_Value(pFieldNum, "2");//图形变更
                    //    maxNum += 1;
                    //    pFeature.set_Value(pFeature.Fields.FindField("BSM"), maxNum);
                    //}
                    //else
                    //{
                    //    pFeature.set_Value(pFieldNum, "1");//属性变更
                    //    pFeature.set_Value(pFeature.Fields.FindField("BSM"), txFeture.get_Value(txFeture.Fields.FindField("BSM")));
                    //}
                    
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
            UpdateStatus("变更行为填写完毕。");
            MessageBox.Show("村级调查区变更行为填写完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void UpdateStatus(string txt)
        {
            info.Text = DateTime.Now.ToString() + ":" + txt + "\r\n" + info.Text;
            Application.DoEvents();
        }
    }
}
