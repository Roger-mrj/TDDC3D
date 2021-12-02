using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;

namespace TDDC3D.gengxin
{
    public partial class FrmXZQGX : Form
    {
        public IWorkspace currWs = null;
        private long maxBSM;

        public FrmXZQGX()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void UpdateStatus(string txt)
        {
            info.Text = DateTime.Now.ToString() + ":" + txt + "\r\n" + info.Text;
            Application.DoEvents();
        }

        private void txtXZQBH_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OpenFileDialog openDia = new OpenFileDialog();
            openDia.Filter = "SHP文件（*.shp）|*.shp";
            if (openDia.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtXZQBH.Text = openDia.FileName;
            }
        }

        private void btnDLTBGX_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtXZQBH.Text))
            {
                MessageBox.Show("请选择变化行政区的数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            IFeatureClass pBHClass = RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(txtXZQBH.Text);
            if (pBHClass == null)
            {
                MessageBox.Show("变化行政区数据没有找到。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("原行政区更新层中的数据将被删除，然后重新生成，是否继续？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes) return;
            
            UpdateStatus("正在生成行政区更新层数据");
            List<string> bsms = new List<string>();
            IFeatureWorkspace pFeatureWorkspace = currWs as IFeatureWorkspace;
            IFeatureClass pGXGCClass = pFeatureWorkspace.OpenFeatureClass("XZQGXGC");
            IFeatureClass pGXClass = pFeatureWorkspace.OpenFeatureClass("XZQGX");
            IFeatureClass pXZQClass = pFeatureWorkspace.OpenFeatureClass("XZQ");
            (pGXClass as ITable).DeleteSearchedRows(null);
            using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
            {
                IFeatureCursor pInsert = pGXClass.Insert(true);
                comRel.ManageLifetime(pInsert);
                using (ESRI.ArcGIS.ADF.ComReleaser comRel2 = new ESRI.ArcGIS.ADF.ComReleaser())
                {
                    IFeatureCursor pBHCursor = pBHClass.Search(null, true);
                    comRel2.ManageLifetime(pBHCursor);
                    IFeature pBH;
                    while ((pBH = pBHCursor.NextFeature()) != null)
                    {
                        string bsm = pBH.get_Value(pBHClass.FindField("BSM")).ToString();
                        bsms.Add(bsm);
                        IFeatureBuffer pFeatureBuffer = pGXClass.CreateFeatureBuffer();
                        for (int i = 0; i < pFeatureBuffer.Fields.FieldCount; i++)
                        {
                            IField pField = pFeatureBuffer.Fields.Field[i];
                            int iFieldIndex = pBHClass.FindField(pField.Name);
                            if (pField.Type != esriFieldType.esriFieldTypeOID && iFieldIndex != -1 && pField.Name != "SHAPE_Area" && pField.Name != "SHAPE_Length")
                            {
                                pFeatureBuffer.set_Value(i, pBH.get_Value(iFieldIndex));
                            }
                        }
                        pFeatureBuffer.set_Value(pGXClass.FindField("GXSJ"), DateTime.ParseExact("20191231", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture));
                        pInsert.InsertFeature(pFeatureBuffer);
                    }
                }
                using (ESRI.ArcGIS.ADF.ComReleaser comRel3 = new ESRI.ArcGIS.ADF.ComReleaser())
                {
                    IQueryFilter pQueryFilter = new QueryFilterClass();
                    pQueryFilter.WhereClause = "BGHBSM = ''";
                    IFeatureCursor pGXGCCursor = pGXGCClass.Update(null, true);
                    comRel3.ManageLifetime(pGXGCCursor);
                    IFeature pGXGC;
                    while ((pGXGC = pGXGCCursor.NextFeature()) != null)
                    {
                        string bsm = pGXGC.get_Value(pGXGCClass.FindField("BGHBSM")).ToString();
                        if (bsms.Contains(bsm)) continue;
                        long newbsm = maxBSM++;
                        pGXGC.set_Value(pGXGCClass.FindField("BGHBSM"), newbsm);
                        pGXGCCursor.UpdateFeature(pGXGC);
                        IFeatureBuffer pFeatureBuffer = pGXClass.CreateFeatureBuffer();
                        pFeatureBuffer.Shape = pGXGC.ShapeCopy;
                        pFeatureBuffer.set_Value(pGXClass.FindField("BSM"), newbsm);
                        pFeatureBuffer.set_Value(pGXClass.FindField("XZQDM"), pGXGC.get_Value(pGXGCClass.FindField("BGQXZQDM")));
                        pFeatureBuffer.set_Value(pGXClass.FindField("XZQMC"), pGXGC.get_Value(pGXGCClass.FindField("BGQXZQMC")));
                        pFeatureBuffer.set_Value(pGXClass.FindField("DCMJ"), pGXGC.get_Value(pGXGCClass.FindField("BGQDCMJ")));
                        pFeatureBuffer.set_Value(pGXClass.FindField("JSMJ"), pGXGC.get_Value(pGXGCClass.FindField("BGQJSMJ")));
                        pFeatureBuffer.set_Value(pGXClass.FindField("MSSM"), pGXGC.get_Value(pGXGCClass.FindField("BGQMSSM")));
                        pFeatureBuffer.set_Value(pGXClass.FindField("HDMC"), pGXGC.get_Value(pGXGCClass.FindField("BGQHDMC")));
                        pFeatureBuffer.set_Value(pGXClass.FindField("GXSJ"), DateTime.ParseExact("20191231", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture));
                        using (ESRI.ArcGIS.ADF.ComReleaser comRel31 = new ESRI.ArcGIS.ADF.ComReleaser())
                        {
                            IQueryFilter pQueryFilter2 = new QueryFilterClass();
                            pQueryFilter2.WhereClause = "BSM = '" + pGXGC.get_Value(pGXGCClass.FindField("BGQBSM")).ToString() + "'";
                            IFeatureCursor pFeatureCursor = pXZQClass.Search(pQueryFilter2, true);
                            comRel3.ManageLifetime(pFeatureCursor);
                            IFeature pXZQ = pFeatureCursor.NextFeature();
                            pFeatureBuffer.set_Value(pGXClass.FindField("YSDM"), pXZQ.get_Value(pXZQClass.FindField("YSDM")));
                            pFeatureBuffer.set_Value(pGXClass.FindField("BZ"), pXZQ.get_Value(pXZQClass.FindField("BZ")));
                        }
                        pInsert.InsertFeature(pFeatureBuffer);
                    }
                }
                pInsert.Flush();
            }
            RCIS.Utility.OtherHelper.ReleaseComObject(pGXGCClass);
            RCIS.Utility.OtherHelper.ReleaseComObject(pBHClass);
            UpdateStatus("生成完成");
            MessageBox.Show("完成。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void FrmXZQGX_Load(object sender, EventArgs e)
        {
            IFeatureClass pDLTBGXGCClass = (currWs as IFeatureWorkspace).OpenFeatureClass("XZQGXGC");
            maxBSM = RCIS.GISCommon.FeatureHelper.GetMaxStringNumberOrderBy(pDLTBGXGCClass, "BSM"); maxBSM++;
        }
    }
}
