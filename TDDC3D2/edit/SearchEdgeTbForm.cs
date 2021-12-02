using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geometry;
using RCIS.GISCommon;
using RCIS.Utility;
using RCIS.GISCommon;
using ESRI.ArcGIS.Geodatabase;

using ESRI.ArcGIS.Carto;

namespace TDDC3D.edit
{
    public partial class SearchEdgeTbForm : Form
    {
        public SearchEdgeTbForm()
        {
            InitializeComponent();
        }

        public IMap currMap = null;
      
        private void SearchEdgeTbForm_Load(object sender, EventArgs e)
        {
            LayerHelper.LoadLayer2Combox(this.cmbLayer, this.currMap, true, esriGeometryType.esriGeometryPolygon);
            int idx = 0;
            for (int i = 0; i < cmbLayer.Properties.Items.Count; i++)
            {
                string txt = OtherHelper.GetLeftName(this.cmbLayer.Properties.Items[i].ToString());
                if (txt.ToUpper() == "DLTB")
                {
                    idx = i;
                    break;
                }
            }
            this.cmbLayer.SelectedIndex = idx;
        }

        public DataTable resultTab = null;

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (this.cmbLayer.Text.Trim() == "") return;
            double dFDistance = 10;
            double dZDistance = 20;
            double.TryParse(this.txtFBuffer.Text, out dFDistance);
            double.TryParse(this.txtZbuffer.Text, out dZDistance);

            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("正在查找", "正在处理...");
            wait.Show();

            //建立Datatable
            this.resultTab = new DataTable();
            DataColumn dc = new DataColumn("LAYER", typeof(string));
            resultTab.Columns.Add(dc);
            dc = new DataColumn("BSM", typeof(string));
            resultTab.Columns.Add(dc);
            dc = new DataColumn("OID", typeof(long));
            resultTab.Columns.Add(dc);

            string className = OtherHelper.GetLeftName(this.cmbLayer.Text);
            IFeatureLayer currLyr = LayerHelper.QueryLayerByModelName(this.currMap,className) as IFeatureLayer;
            
            this.currMap.ClearSelection();    

            IFeatureClass pFC = currLyr.FeatureClass;
            ISpatialFilter pSF = new SpatialFilterClass();
            pSF.Geometry = (this.currMap as IActiveView).Extent;
            pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            IFeatureCursor pCursor = pFC.Search(pSF, false);
            try
            {
                IFeature aSrcDltb = null;
                while ((aSrcDltb = pCursor.NextFeature()) != null)
                {
                    IGeometry aGeo = aSrcDltb.Shape;
                    ITopologicalOperator ptop = aGeo as ITopologicalOperator;
                    IGeometry buffGeo = ptop.Buffer(-1 * dFDistance);

                    if (buffGeo.IsEmpty)
                        continue;
                    ptop = buffGeo as ITopologicalOperator;
                    IGeometry buffGeo2 = ptop.Buffer(dZDistance);
                    IRelationalOperator pRO = buffGeo2 as IRelationalOperator;
                    if (!pRO.Contains(aGeo))
                    {
                        this.currMap.SelectFeature(currLyr, aSrcDltb);
                        //加入一行
                        DataRow dr = resultTab.NewRow();
                        dr["LAYER"] = className;
                        dr["BSM"] = FeatureHelper.GetFeatureStringValue(aSrcDltb, "BSM");
                        dr["OID"] = aSrcDltb.OID;
                        resultTab.Rows.Add(dr);

                        wait.SetCaption("已选择" + aSrcDltb.OID + "...");
                        Application.DoEvents();
                    }                                  
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                OtherHelper.ReleaseComObject(pCursor);
                wait.Close();
            }

            this.DialogResult = DialogResult.OK;
            Close();


        }

        private Boolean existSmallAngle(IPolygon pPolygon, double maxAngle, out int iPointID)
        {
            iPointID = -1;
            IPointCollection pPCol = pPolygon as IPointCollection;
            if (pPCol.PointCount < 3) return false;
            for (int i = 0; i < pPCol.PointCount - 1; i++)
            {
                IPoint pPointC = pPCol.get_Point(i);
                IPoint pPointP;
                if (i == 0)
                    pPointP = pPCol.get_Point(pPCol.PointCount - 2);
                else
                    pPointP = pPCol.get_Point(i - 1);
                IPoint pPointN = pPCol.get_Point(i + 1);
                if (GetAngle(pPointC, pPointP, pPointN) < maxAngle)
                {
                    iPointID = i;
                    return true;
                }
            }
            return false;
        }
        private double GetAngle(IPoint cen, IPoint first, IPoint second)
        {
            double ma_x = first.X - cen.X;
            double ma_y = first.Y - cen.Y;
            double mb_x = second.X - cen.X;
            double mb_y = second.Y - cen.Y;
            double v1 = (ma_x * mb_x) + (ma_y * mb_y);
            double ma_val = Math.Sqrt(ma_x * ma_x + ma_y * ma_y);
            double mb_val = Math.Sqrt(mb_x * mb_x + mb_y * mb_y);
            double cosM = v1 / (ma_val * mb_val);
            double angleAMB = Math.Acos(cosM) * 180 / Math.PI;
            return angleAMB;
        }

        private bool existSmallAngle(IFeature aSrcDltb,  double maxAngle, double minLen, out IPolyline pLine)
        {
            pLine = null;
            IGeometry aGeo = aSrcDltb.ShapeCopy;
            //获取所有点
            IPointCollection ptCols = aGeo as IPointCollection;
            //第一个夹角
            if (ptCols.PointCount < 3) return false;
            IPoint pt1 = ptCols.get_Point(0);
            IPoint pt2 = ptCols.get_Point(1);
            IPoint pt3 = ptCols.get_Point(2);
            ILine line1 = new LineClass();
            line1.PutCoords(pt1, pt2);
            double length1 = line1.Length;
            if (line1.Length < minLen) return false;

            ILine line2 = new LineClass();
            line2.PutCoords(pt2, pt3);
            if (line2.Length < minLen) return false;

            GeometryEnvironment geometryEnvironment = new GeometryEnvironment();
            IConstructAngle constructAngle = geometryEnvironment as IConstructAngle;
            double piangle = constructAngle.ConstructThreePoint(pt1, pt2, pt3);
            double angle = piangle * 180 / Math.PI;
            if (angle > maxAngle)
            {
                return false;
            }
            else
            {
                IPointCollection pPointCol = new PolylineClass();
                pPointCol.AddPoint(pt1);
                pPointCol.AddPoint(pt2);
                pPointCol.AddPoint(pt3);
                pLine = pPointCol as IPolyline;
                return true;
            }

        }

        private bool existSmallAngle2(IFeature aSrcDltb, double maxAngle, double minLen, out IPolyline pLine)
        {
            pLine = null;
            IGeometry aGeo = aSrcDltb.ShapeCopy;
            //获取所有点
            IPointCollection ptCols = aGeo as IPointCollection;
            //第一个夹角
            if (ptCols.PointCount < 3) return false;
            IPoint pt1 = ptCols.get_Point(0);
            IPoint pt2 = ptCols.get_Point(1);
            IPoint pt3 = ptCols.get_Point(2);
            ILine line1 = new LineClass();
            line1.PutCoords(pt1, pt2);
            double length1 = line1.Length;
           

            ILine line2 = new LineClass();
            line2.PutCoords(pt2, pt3);
            double lenght2 = line2.Length;

            if (Math.Abs(lenght2 - length1) < minLen) return false;

            GeometryEnvironment geometryEnvironment = new GeometryEnvironment();
            IConstructAngle constructAngle = geometryEnvironment as IConstructAngle;
            double piangle = constructAngle.ConstructThreePoint(pt1, pt2, pt3);
            double angle = piangle * 180 / Math.PI;
            if (angle > maxAngle)
                return false;
            else
            {
                IPointCollection pPointCol = new PolylineClass();
                pPointCol.AddPoint(pt1);
                pPointCol.AddPoint(pt2);
                pPointCol.AddPoint(pt3);
                pLine = pPointCol as IPolyline;
                return true;
            }

        }

        private void btnQuery2_Click(object sender, EventArgs e)
        {
            if (this.cmbLayer.Text.Trim() == "") return;

            double maxAngle = 30;
            double minLen = 10;
            maxAngle =(double) this.spinEditAngle.Value;
            switch (rdoLen.SelectedIndex)
            {
                case 0:
                    minLen = (double)this.spenEdtLen.Value;
                    break;
                case 1:
                    minLen = (double)this.spinEditLen2.Value;
                    break;
            }

            this.resultTab = new DataTable();
            DataColumn dc = new DataColumn("LAYER", typeof(string));
            resultTab.Columns.Add(dc);
            dc = new DataColumn("BSM", typeof(string));
            resultTab.Columns.Add(dc);
            dc = new DataColumn("OID", typeof(long));
            resultTab.Columns.Add(dc);

            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("正在按照方法二查找", "正在处理...");
            wait.Show();

            string className = OtherHelper.GetLeftName(this.cmbLayer.Text);
            IFeatureLayer currLyr = LayerHelper.QueryLayerByModelName(this.currMap, className) as IFeatureLayer;

            IGeoDataset pGeoDataset = currLyr.FeatureClass as IGeoDataset;
            string sTempFile = Application.StartupPath + @"\tempLine.shp";
            if (System.IO.File.Exists(sTempFile))
            {
                IFeatureClass pDel = WorkspaceHelper2.GetShapefileFeatureClass(sTempFile);
                IDataset pDelDataset = pDel as IDataset;
                pDelDataset.Delete();
            }
            IFeatureClass pTempFeatureClass = WorkspaceHelper2.CreateSHP(sTempFile, esriGeometryType.esriGeometryPolyline, pGeoDataset.SpatialReference);
            IFeatureCursor ptempCursor = pTempFeatureClass.Insert(true);

            this.currMap.ClearSelection();

            IFeatureClass pFC = currLyr.FeatureClass;
            ISpatialFilter pSF = new SpatialFilterClass();
            pSF.Geometry = (this.currMap as IActiveView).Extent;
            pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            IFeatureCursor pCursor = pFC.Search(pSF, true);

            try
            {
                IFeature aDltb = null;
                while ((aDltb = pCursor.NextFeature()) != null)
                {
                    IPolyline pLine = new PolylineClass();
                    bool isExistSmallAngle = false;
                    switch (rdoLen.SelectedIndex)
                    {
                        case 0:
                            isExistSmallAngle = this.existSmallAngle(aDltb, maxAngle, minLen, out pLine);
                            break;
                        case 1:
                            isExistSmallAngle = this.existSmallAngle2(aDltb, maxAngle, minLen, out pLine);
                            break;
                    }
                    if (isExistSmallAngle)
                    {
                        this.currMap.SelectFeature(currLyr, aDltb);
                        //加入一行
                        DataRow dr = resultTab.NewRow();
                        dr["LAYER"] = className;
                        dr["BSM"] = FeatureHelper.GetFeatureStringValue(aDltb, "BSM");
                        dr["OID"] = aDltb.OID;
                        resultTab.Rows.Add(dr);

                        if (pLine != null)
                        {
                            IFeatureBuffer pFB = pTempFeatureClass.CreateFeatureBuffer();
                            pFB.Shape = pLine;
                            pFB.set_Value(pTempFeatureClass.FindField("id"), aDltb.OID);
                            ptempCursor.InsertFeature(pFB);
                        }
                        wait.SetCaption("已选择" + aDltb.OID + "...");
                        wait.Update();
                    }
                }
                ptempCursor.Flush();
                wait.Close();
            }
            catch (Exception ex)
            {
                wait.Close();
                MessageBox.Show(ex.Message);
            }
            finally
            {
                OtherHelper.ReleaseComObject(pCursor);
            }
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (this.cmbLayer.Text.Trim() == "") return;

            double maxAngle = 30;
            maxAngle = (double)this.spinEditAngle2.Value;

            this.resultTab = new DataTable();
            DataColumn dc = new DataColumn("LAYER", typeof(string));
            resultTab.Columns.Add(dc);
            dc = new DataColumn("BSM", typeof(string));
            resultTab.Columns.Add(dc);
            dc = new DataColumn("OID", typeof(long));
            resultTab.Columns.Add(dc);
            dc = new DataColumn("POINTID", typeof(long));
            resultTab.Columns.Add(dc);

            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("正在按照方法三查找", "正在处理...");
            wait.Show();

            string className = OtherHelper.GetLeftName(this.cmbLayer.Text);
            IFeatureLayer currLyr = LayerHelper.QueryLayerByModelName(this.currMap, className) as IFeatureLayer;

            this.currMap.ClearSelection();

            IFeatureClass pFC = currLyr.FeatureClass;
            ISpatialFilter pSF = new SpatialFilterClass();
            pSF.Geometry = (this.currMap as IActiveView).Extent;
            pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            IFeatureCursor pCursor = pFC.Search(pSF, true);

            try
            {
                IFeature aDltb = null;
                while ((aDltb = pCursor.NextFeature()) != null)
                {
                    int iPID = -1;
                    if (this.existSmallAngle(aDltb.ShapeCopy as IPolygon, maxAngle, out iPID))
                    {
                        this.currMap.SelectFeature(currLyr, aDltb);
                        //加入一行
                        DataRow dr = resultTab.NewRow();
                        dr["LAYER"] = className;
                        dr["BSM"] = FeatureHelper.GetFeatureStringValue(aDltb, "BSM");
                        dr["OID"] = aDltb.OID;
                        dr["POINTID"] = iPID;
                        resultTab.Rows.Add(dr);

                        wait.SetCaption("已选择" + aDltb.OID + "...");
                        wait.Update();
                    }
                }
                wait.Close();
            }
            catch (Exception ex)
            {
                wait.Close();
                MessageBox.Show(ex.Message);
            }
            finally
            {
                OtherHelper.ReleaseComObject(pCursor);
            }
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog savedia = new SaveFileDialog();
            savedia.Filter = "SHP格式数据(*.shp)|*.shp";
            if (savedia.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                double maxAngle = 30;
                maxAngle = (double)this.spinEditAngle2.Value;
                string className = OtherHelper.GetLeftName(this.cmbLayer.Text);
                IFeatureLayer currLyr = LayerHelper.QueryLayerByModelName(this.currMap, className) as IFeatureLayer;
                IGeoDataset pGeodataset = currLyr.FeatureClass as IGeoDataset;

                IFeatureClass pFeatureClass = WorkspaceHelper2.CreateSHP(savedia.FileName, esriGeometryType.esriGeometryPoint, pGeodataset.SpatialReference);
                IFeatureCursor pInsert = pFeatureClass.Insert(true);

                IFeatureClass pFC = currLyr.FeatureClass;
                ISpatialFilter pSF = new SpatialFilterClass();
                pSF.Geometry = (this.currMap as IActiveView).Extent;
                pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                IFeatureCursor pCursor = pFC.Search(pSF, true);
                DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("正在按照方法三查找", "正在处理...");
                wait.Show();
                try
                {
                    IFeature aDltb = null;
                    while ((aDltb = pCursor.NextFeature()) != null)
                    {
                        int iPID = -1;
                        if (this.existSmallAngle(aDltb.ShapeCopy as IPolygon, maxAngle, out iPID))
                        {
                            IPointCollection pPC = aDltb.ShapeCopy as IPointCollection;
                            IPoint pP = pPC.Point[iPID];
                            IFeatureBuffer pF = pFeatureClass.CreateFeatureBuffer();
                            pF.Shape = pP;
                            pF.set_Value(pF.Fields.FindField("id"), aDltb.OID);
                            pInsert.InsertFeature(pF);
                            if (aDltb.OID % 100 == 0) pInsert.Flush();
                            wait.SetCaption("已选择" + aDltb.OID + "...");
                            wait.Update();
                        }
                    }
                    pInsert.Flush();
                    wait.Close();
                }
                catch (Exception ex)
                {
                    wait.Close();
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    OtherHelper.ReleaseComObject(pInsert);
                    OtherHelper.ReleaseComObject(pCursor);
                }
                MessageBox.Show("完成。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }



    }
}
