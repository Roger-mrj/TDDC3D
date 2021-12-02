using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using RCIS.GISCommon;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using System.Collections;
using RCIS.Utility;
using ESRI.ArcGIS.Geometry;

namespace TDDC3D.gengxin
{
    public partial class FrmTBBHOpt2 : Form
    {
        public IMap currMap = null;

        public FrmTBBHOpt2()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmTBBHOpt2_Load(object sender, EventArgs e)
        {
            LayerHelper.LoadLayer2Combox(cboDLTBGX, currMap);
            for (int i = 0; i < cboDLTBGX.Properties.Items.Count; i++)
            {
                string layerName = cboDLTBGX.Properties.Items[i].ToString();
                if (layerName.Contains("DLTBGX"))
                {
                    cboDLTBGX.Text = layerName;
                    break;
                }
            }
        }

        private void cboDLTBGX_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(cboDLTBGX.Text))
            {
                IFeatureLayer pLayer = LayerHelper.QueryLayerByModelName(currMap, "DLTB");
                if (pLayer != null)
                {
                    IFeatureClass pFeatureClass = pLayer.FeatureClass;
                    if (pFeatureClass.Fields.FindField("TBBH") == -1)
                    {
                        MessageBox.Show("该图层没有找到图斑编号字段。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        txtTBBH.Text = (FeatureHelper.GetMaxStringNumberEveryOne(pFeatureClass, "TBBH") + 1).ToString();
                    }
                }
                else
                {
                    MessageBox.Show("没有找到该图层。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            txtBeforeTBH.Enabled = checkEdit1.Checked;
        }

        private void btnCompute_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cboDLTBGX.Text) ||
                string.IsNullOrWhiteSpace(txtTBBH.Text) ||
                (checkEdit1.Checked && string.IsNullOrWhiteSpace(txtBeforeTBH.Text)))
            {
                MessageBox.Show("输入信息不完整。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("正在执行图斑编号，请稍等...", "处理中");
            wait.Show();

            long iStartH = 1; //起始点号
            long.TryParse(txtTBBH.Text, out iStartH);
            IFeatureLayer pLayer = LayerHelper.QueryLayerByModelName(currMap, cboDLTBGX.Text.Split('|')[0]);
            if (pLayer == null)
            {
                wait.Close();
                return;
            }
            //IPolygon extent = RCIS.GISCommon.GeometryHelper.UnionPolygon(lstXzq);
            //找到所有地类图斑
            ArrayList arAllTbs = getAllTb(pLayer.FeatureClass); //getAllTb(aXzqFea.ShapeCopy);

            //IComparer comparer = new tbbhCompare();
            //arAllTbs.Sort(comparer);

            IWorkspaceEdit pWSEdit = (pLayer.FeatureClass as IDataset).Workspace as IWorkspaceEdit;
            pWSEdit.StartEditing(true);
            pWSEdit.StartEditOperation();
            try
            {
                foreach (IFeature aTb in arAllTbs)
                {
                    //string tbbh1 = xzqdm + (iStartH).ToString().PadLeft(6, '0');
                    string tbbh = checkEdit1.Checked ? txtBeforeTBH.Text + iStartH.ToString() : iStartH.ToString();
                    //FeatureHelper.SetFeatureValue(aTb, "TBYBH", tbbh1);
                    RCIS.GISCommon.FeatureHelper.SetFeatureValue(aTb, "TBBH", tbbh);
                    aTb.Store();
                    iStartH++;
                }
                pWSEdit.StopEditOperation();
                pWSEdit.StopEditing(true);
            }
            catch (Exception ex)
            {
                pWSEdit.AbortEditOperation();
                pWSEdit.StopEditing(false);
            }
            wait.Close();
            MessageBox.Show("编号完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public class tbbhCompare : IComparer
        {
            int IComparer.Compare(object x, object y)
            {
                IFeature fea1 = x as IFeature;
                IFeature fea2 = y as IFeature;

                IPoint pt1 = (fea1.Shape as IArea).Centroid as IPoint;
                IPoint pt2 = (fea2.Shape as IArea).Centroid as IPoint;

                if (pt1.Y > pt2.Y)
                {
                    return -1;
                }
                else
                    if (pt1.Y == pt2.Y)
                    {
                        if (pt1.X < pt2.X)
                            return -1;
                        else
                            if (pt1.X == pt2.X)
                                return 0;
                            else
                                return 1;
                    }
                    else
                    {
                        return 1;
                    }


            }
        }

        private ArrayList getAllTb(IFeatureClass dltbClass)
        {

            ArrayList arTbs = new ArrayList();
            //ISpatialFilter pSF = new SpatialFilterClass();
            //pSF.WhereClause = "ZLDWDM='" + dwdm + "'";
            //pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
            //pSF.Geometry = xzqGeo;
            //pSF.set_OutputSpatialReference(dltbClass.ShapeFieldName, (dltbClass as IGeoDataset).SpatialReference);
            IFeatureCursor pCursor = dltbClass.Search(null, false);
            IFeature aFeature = null;
            while ((aFeature = pCursor.NextFeature()) != null)
            {
                arTbs.Add(aFeature);
            }
            OtherHelper.ReleaseComObject(pCursor);
            return arTbs;

        }
    }
}
