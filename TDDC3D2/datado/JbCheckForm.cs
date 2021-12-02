using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TDDC3D.datado
{
    public partial class JbCheckForm : Form
    {
        public JbCheckForm()
        {
            InitializeComponent();
        }

        public IMapControl2 mapctl = null;

        public Double distance
        {
            get { 
                double dis=6;
                double.TryParse(this.textEdit1.Text, out dis);
                return dis;

            }
        }

        public string JxLb
        {
            get{
                return this.comboBoxEdit1.Text.Trim();
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            IMap pMap = this.mapctl.ActiveView.FocusMap;
            xzqLayer = RCIS.GISCommon.LayerHelper.QueryLayerByModelName(pMap, RCIS.Global.AppParameters.LAYER_XZQ);
            if (xzqLayer == null)
            {
                System.Windows.Forms.MessageBox.Show("找不到行政区层");
                return;
            }
            //每一段线 查询地类图斑
            IFeatureLayer dltbLayer = RCIS.GISCommon.LayerHelper.QueryLayerByModelName(pMap, RCIS.Global.AppParameters.LAYER_DLTB);
            if (dltbLayer == null)
            {
                System.Windows.Forms.MessageBox.Show("找不到地类图斑层");
                return;
            }
            logTable = buildTable();


            //找到界线
            List<IGeometry> allJxLine = getAllJx();
            //找到两两相交的那些线
            List<IGeometry> IntersectLines = this.getIntersecLine(allJxLine);

            List<aPtItem> leftItmes = new List<aPtItem>();
            List<aPtItem> rightItems = new List<aPtItem>();


            IFeatureClass pDltbClass = dltbLayer.FeatureClass;
            ISpatialFilter pSF = new SpatialFilterClass();
            foreach (IGeometry aLine in IntersectLines)
            {
                ITopologicalOperator pALineTop = aLine as ITopologicalOperator;
                pSF.Geometry = aLine;
                pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                IFeatureCursor pCursor = pDltbClass.Search(pSF as IQueryFilter, false);
                try
                {
                    #region  //地类图斑与这条变相交的那些，
                    IFeature aDltbFeature = null;
                    while ((aDltbFeature = pCursor.NextFeature()) != null)
                    {

                        //如果也是交于一条边
                        IGeometry lastLine = pALineTop.Intersect(aDltbFeature.ShapeCopy, esriGeometryDimension.esriGeometry1Dimension);
                        if (!lastLine.IsEmpty)
                        {

                            IPointCollection ptCols = lastLine as IPointCollection;

                            //看是否在边左边还是右边
                            IPoint inPoint = (aDltbFeature.ShapeCopy as IArea).Centroid;
                            IPoint outPoint = new PointClass();
                            ICurve pCurve = aLine as ICurve;
                            bool isRight = true;
                            double distanceFromCurve = 0;
                            double distanceAlongCurve = 0;
                            pCurve.QueryPointAndDistance(esriSegmentExtension.esriNoExtension, inPoint, true, outPoint, ref distanceAlongCurve, ref distanceFromCurve, ref isRight);

                            if (isRight)
                            //记录这条边所有点，放在左边或者右边的列表中
                            {
                                for (int kk = 0; kk < ptCols.PointCount; kk++)
                                {
                                    rightItems.Add(new aPtItem(ptCols.get_Point(kk), aDltbFeature.OID));
                                }
                            }
                            else
                            {
                                for (int kk = 0; kk < ptCols.PointCount; kk++)
                                {
                                    leftItmes.Add(new aPtItem(ptCols.get_Point(kk), aDltbFeature.OID));
                                }
                            }

                        }

                    }

                    #endregion
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                }


            }

            //左边点集合 和右边点集合，距离判断是否小于distance
            foreach (aPtItem aleft in leftItmes)
            {
                IPoint leftPt = aleft.aPoint as IPoint;
                foreach (aPtItem aright in rightItems)
                {
                    //如果是 ，将这两点记录下来
                    IPoint rightPt = aright.aPoint as IPoint;
                    double len = System.Math.Sqrt((rightPt.X - leftPt.X) * (rightPt.X - leftPt.X) + (rightPt.Y - rightPt.Y) * (rightPt.Y - rightPt.Y));
                    if (len < this.distance)
                    {
                        InsertALog(aleft);
                    }

                }
            }

            this.gridControl1.DataSource = this.logTable;

        }

        public ILayer xzqLayer = null;
      
        public List<IGeometry> getAllJx()
        {
            List<IGeometry> lst = new List<IGeometry>();
            IIdentify idJxs = xzqLayer as IIdentify;
            IArray arJxs = idJxs.Identify(this.mapctl.ActiveView.Extent);
            if (arJxs == null)
                return lst;
            for (int i = 0; i < arJxs.Count; i++)
            {
                IFeatureIdentifyObj obj = arJxs.get_Element(i) as IFeatureIdentifyObj;
                IRowIdentifyObject aRow = obj as IRowIdentifyObject;
                IFeature aFeature = aRow.Row as IFeature;
                ITopologicalOperator pTop = aFeature.ShapeCopy as ITopologicalOperator;
                IGeometry pline = pTop.Boundary;
                lst.Add(pline);
            }
            return lst;

        }

        private List<IGeometry> getIntersecLine(List<IGeometry> allJxLine)
        {
            List<IGeometry> lst = new List<IGeometry>();
            for (int i = 0; i < allJxLine.Count - 1; i++)
            {
                for (int j = i + 1; j < allJxLine.Count; j++)
                {
                    IGeometry geo1 = allJxLine[i];
                    IGeometry geo2 = allJxLine[j];
                    ITopologicalOperator ptop1 = geo1 as ITopologicalOperator;
                    IGeometry interGeo = ptop1.Intersect(geo2, esriGeometryDimension.esriGeometry1Dimension);
                    if (!interGeo.IsEmpty)
                    {
                        IPolyline aInterLine = interGeo as IPolyline;
                        bool bExist = false;
                        foreach (IGeometry aGeo in lst)
                        {
                            IPolyline aLIne = aGeo as IPolyline;
                            if (RCIS.GISCommon.GeometryHelper.LineEquals2(aLIne, aInterLine))
                            {
                                bExist = true;
                            }
                        }
                        if (!bExist)
                        {
                            lst.Add(interGeo);
                        }
                    }
                }
            }
            return lst;
        }


        public class aPtItem
        {
            public IGeometry aPoint;
            public int parentOid;
            public aPtItem(IGeometry _pt, int _oid)
            {
                this.aPoint = _pt;
                this.parentOid = _oid;
            }
        }

        public System.Data.DataTable logTable = null;

        public System.Data.DataTable buildTable()
        {
            System.Data.DataTable logTab = new System.Data.DataTable();
            System.Data.DataColumn dc = new System.Data.DataColumn("DLTBOID", typeof(int));
            logTab.Columns.Add(dc);
            dc = new System.Data.DataColumn("LEFTOBJ", typeof(IPoint));
            logTab.Columns.Add(dc);

            return logTab;
        }

        public void InsertALog(aPtItem aItem)
        {
            System.Data.DataRow arow = logTable.NewRow();
            arow["DLTBOID"] = aItem.parentOid;
            arow["LEFTOBJ"] = aItem.aPoint;
            logTable.Rows.Add(arow);
        }


        private void JbCheckForm_Load(object sender, EventArgs e)
        {

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            if (this.gridView1.SelectedRowsCount == 0)
                return;
            
            try
            {
                IPoint apt=(IPoint) this.gridView1.GetRowCellValue(this.gridView1.FocusedRowHandle, "LEFTOBJ");
                ITopologicalOperator ptop = apt as ITopologicalOperator;
                IGeometry buffGeo = ptop.Buffer(1);
                IEnvelope env = buffGeo.Envelope;
                env.Expand(1.5, 1.5, true);
                this.mapctl.ActiveView.Extent = env;

                this.mapctl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewAll, null, this.mapctl.ActiveView.Extent);
                this.mapctl.ActiveView.ScreenDisplay.UpdateWindow();
                this.mapctl.FlashShape(buffGeo, 3, 300, null);

            }
            catch (Exception cex)
            {
            }
        }
    }
}
