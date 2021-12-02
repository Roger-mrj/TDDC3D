using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.ADF;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using RCIS.GISCommon;
using RCIS.Utility;
using ESRI.ArcGIS.esriSystem;
using System.Collections;

namespace TDDC3D.edit
{
    public partial class MapIntegrateForm : Form
    {
        public MapIntegrateForm()
        {
            InitializeComponent();
        }
        public IMap currMap = null;
        private void MapIntegrateForm_Load(object sender, EventArgs e)
        {
            LayerHelper.LoadLayer2Combox(cmbLayer, currMap, esriGeometryType.esriGeometryPolygon);
            List<string> sAllDlbm = sys.YWCommonHelper.getAllDlbm();     

            //加载所有地类
            foreach (string adl in sAllDlbm)
            {
                this.cmbExceptedDlbms.Properties.Items.Add(adl);
            }
        }



        

        private List<string>  exceptDLBM = new List<string>();

       
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string dltbClassName = OtherHelper.GetLeftName(this.cmbLayer.Text);
            if (dltbClassName.Trim().ToUpper() == "")
                return;
            IFeatureLayer pFeaLyr = LayerHelper.QueryLayerByModelName(this.currMap, dltbClassName);
            if (pFeaLyr == null) return;
            IFeatureClass dltbClass = pFeaLyr.FeatureClass;
            //阈值

            double dmaxTbmj = 0;
            double.TryParse(this.txtTbmj.Text, out dmaxTbmj);
            double dmaxWidth = 0;
            double.TryParse(this.txtScale.Text, out dmaxWidth); //最大面积除以周长，简略的理解为道路宽度

            //计算范围
            IEnvelope pEnv = null;
            if (this.radioGroupExtent.SelectedIndex == 0)
            {
                pEnv = (dltbClass as IGeoDataset).Extent;
            }
            else
            {
                pEnv = (this.currMap as IActiveView).Extent;
            }


            //排除地类
            exceptDLBM.Clear();
            for (int k = 0; k < this.cmbExceptedDlbms.Properties.Items.Count; k++)
            {
                if (this.cmbExceptedDlbms.Properties.Items[k].CheckState == CheckState.Checked)
                {
                    exceptDLBM.Add(OtherHelper.GetLeftName(this.cmbExceptedDlbms.Properties.Items[k].ToString()));
                }
            }
            List<int> lstExcepted = new List<int>();

            IIdentify identifyDltb = pFeaLyr as IIdentify;
            IArray arDltbs = identifyDltb.Identify(pEnv);
            if (arDltbs == null)
                return ;
            this.Cursor = Cursors.WaitCursor;
            this.lblstatus.Text = "开始处理...";
            Dictionary<string, IGeometry> geoDIc = new Dictionary<string, IGeometry>();
            Application.DoEvents();
            IWorkspaceEdit wsEdt = RCIS.Global.GlobalEditObject.GlobalWorkspace as IWorkspaceEdit;
            wsEdt.StartEditing(false);
            wsEdt.StartEditOperation();
            try
            {
                for (int i = 0; i < arDltbs.Count; i++)
                {
                    IFeatureIdentifyObj obj = arDltbs.get_Element(i) as IFeatureIdentifyObj;
                    IRowIdentifyObject aRow = obj as IRowIdentifyObject;
                    IFeature aFeature = aRow.Row as IFeature;
                    string dlbm = FeatureHelper.GetFeatureStringValue(aFeature, "DLBM");
                    if (exceptDLBM.Contains(dlbm))
                    {
                        continue; ///铁路公路不处理
                    }

                    //所有地类图斑
                    IPolygon aPolygon = aFeature.Shape as IPolygon;
                    double area = (aPolygon as IArea).Area;
                    double length = aPolygon.Length;

                    bool isMerge = false;
                    if (area < dmaxTbmj)
                    {
                        isMerge = true;

                    }

                    if (this.checkEditMjCZC.Checked)
                    {
                        if (area / length < dmaxWidth)
                            isMerge = true;

                    }
                    if (!isMerge)
                    {
                        continue;
                    }

                    //先找地类相同的 周边相同坐落权属的最大图斑
                    IFeature maxFea = null;
                    //IFeature maxFea = SmallTbDoHelper.getMaxFeatureSameDL2(aFeature, dltbClass, dmaxTbmj, ref lstExcepted);
                    if (maxFea == null)
                        maxFea = SmallTbDoHelper.getMaxFeatureSameDL1(aFeature, dltbClass, dmaxTbmj, ref lstExcepted); //相同一级类最大的图斑
                    if (maxFea == null)
                        maxFea = SmallTbDoHelper.getMaxFeatureSame3DL(aFeature, dltbClass, dmaxTbmj, ref lstExcepted);

                    if (maxFea == null)
                        maxFea = SmallTbDoHelper.getMaxFeature(aFeature, dltbClass, dmaxTbmj, ref lstExcepted);
                    if (maxFea == null) continue;
                    //List<string> aa = new List<string>() { };
                    IGeometry pGeo = null;
                    if (geoDIc.Keys.Contains(aFeature.OID.ToString()))
                        pGeo = RCIS.GISCommon.GeometryHelper.UnionPolygon(new List<IGeometry>() { geoDIc[aFeature.OID.ToString()], maxFea.ShapeCopy });
                    else
                        pGeo = RCIS.GISCommon.GeometryHelper.UnionPolygon(new List<IGeometry>() { aFeature.ShapeCopy, maxFea.ShapeCopy });
                    //ITopologicalOperator2 pTop = maxFea.Shape as ITopologicalOperator2;
                    //IGeometry newGeo = pTop.Union(aFeature.ShapeCopy);
                    maxFea.Shape = pGeo;
                    maxFea.Store();
                    if (geoDIc.Keys.Contains(maxFea.OID.ToString()))
                        geoDIc[maxFea.OID.ToString()] = maxFea.ShapeCopy;
                    else
                        geoDIc.Add(maxFea.OID.ToString(), maxFea.ShapeCopy);
                    RCIS.Utility.OtherHelper.ReleaseComObject(maxFea);
                    RCIS.Utility.OtherHelper.ReleaseComObject(pGeo);
                    aFeature.set_Value(aFeature.Fields.FindField("BZ"), "DEL");
                    aFeature.Store();
                    //aFeature.Delete(); //原来的删除
                    //lstExcepted.Add(aFeature.OID);
                    lblstatus.Text = "当前处理" + aFeature.OID;
                    RCIS.Utility.OtherHelper.ReleaseComObject(aFeature);

                    lblstatus.Update();
                    //
                }
                geoDIc.Clear();
                wsEdt.StopEditOperation();
                wsEdt.StopEditing(true);
                IQueryFilter pQf = new QueryFilterClass();
                pQf.WhereClause = "BZ='DEL'";
                (dltbClass as ITable).DeleteSearchedRows(pQf);
                bool b;
                if (dltbClass.FeatureDataset != null)
                    b = RCIS.GISCommon.GpToolHelper.RepairGeometry((dltbClass as IDataset).Workspace.PathName + "//" + dltbClass.FeatureDataset.Name + "//" + (dltbClass as IDataset).Name);
                else
                    b = RCIS.GISCommon.GpToolHelper.RepairGeometry((dltbClass as IDataset).Workspace.PathName + "//" + (dltbClass as IDataset).Name);
                this.Cursor = Cursors.Default;
                GC.Collect();
                GC.WaitForPendingFinalizers();
                MessageBox.Show("处理完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                wsEdt.AbortEditOperation();
                wsEdt.StopEditing(false);
                MessageBox.Show(ex.Message);
            }
            this.lblstatus.Text = "";
        }
    }
}
