using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using RCIS.GISCommon;
using RCIS.Utility;
using System;
using System.Collections;
using System.Windows.Forms;

namespace TDDC3D.datado
{
    public partial class DoDuplicateForm2 : Form
    {
        public DoDuplicateForm2()
        {
            InitializeComponent();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }
        public IMap currMap = null;

        private void DoDuplicateForm2_Load(object sender, EventArgs e)
        {
            LayerHelper.LoadLayer2Combox(this.cmbLayers, currMap);

        }

        private ArrayList getDup(IFeature aFea, IFeatureClass pFC,ArrayList existList)
        {
            ArrayList ar = new ArrayList();

            IRelationalOperator pRO = aFea.ShapeCopy as IRelationalOperator;
            ISpatialFilter pSF = new SpatialFilterClass();
            pSF.Geometry = aFea.ShapeCopy;
            pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            //与当前要素 重叠的要素
            IFeatureCursor pcursor = pFC.Search(pSF as IQueryFilter, false);
            IFeature getFea = null;
            try
            {
                while ((getFea = pcursor.NextFeature()) != null)
                {
                    if (existList.Contains(getFea.OID))
                        continue;
                    if (getFea.OID == aFea.OID)
                        continue;
                    if (pFC.ShapeType == esriGeometryType.esriGeometryPoint)
                    {
                        if (GeometryHelper.PointEquals(getFea.ShapeCopy as IPoint , aFea.ShapeCopy as IPoint))
                        {
                            ar.Add(getFea.OID);
                        }
                    }
                    else if (pFC.ShapeType == esriGeometryType.esriGeometryPolygon)
                    {
                        IRelationalOperator2 pRO2 = getFea.ShapeCopy as IRelationalOperator2;
                        if (pRO.Contains(getFea.ShapeCopy) && (pRO2.Contains(aFea.ShapeCopy)))
                        {
                            ar.Add(getFea.OID);
                        }
                    }
                    else if (pFC.ShapeType == esriGeometryType.esriGeometryPolyline)
                    {
                        //
                        if (GeometryHelper.LineEquals(aFea.ShapeCopy as IPolyline, getFea.ShapeCopy as IPolyline))
                            ar.Add(getFea.OID);
                    }
                    

                }
            }
            catch(Exception ex)
            { }
            finally
            {
                OtherHelper.ReleaseComObject(pcursor);
            }
            return ar;

        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (this.cmbLayers.Text.Trim() == "")
                return;
            string className = this.cmbLayers.Text.Trim();
            className = OtherHelper.GetLeftName(className);

            this.Cursor = Cursors.WaitCursor;
            this.lblStatus.Text = "正在查找所有重复要素...";
            Application.DoEvents();
            try
            {
                IFeatureLayer pFeaLyr = LayerHelper.QueryLayerByModelName(this.currMap, className);
                IFeatureClass pFC = pFeaLyr.FeatureClass;
                ITable pTable = pFC as ITable;
                ArrayList arDuplicate = new ArrayList();//记录 所有的 重复要素oid
                #region 查找重复
                IIdentify identify = pFeaLyr as IIdentify;
                IArray allFeatureIds = identify.Identify((pFC as IGeoDataset).Extent);
                for (int i = 0; i < allFeatureIds.Count; i++)
                {
                    IFeatureIdentifyObj idObj = allFeatureIds.get_Element(i) as IFeatureIdentifyObj;
                    IRowIdentifyObject pRow = idObj as IRowIdentifyObject;
                    IFeature pfea = pRow.Row as IFeature;

                    //避免循环查找，从 2 找到 100 ，到100 又找到 2
                    if (arDuplicate.Contains(pfea.OID))
                        continue;

                    //获取与他重复的，放到列表里
                    ArrayList arSmallDup = this.getDup(pfea, pFC, arDuplicate);
                    arDuplicate.AddRange(arSmallDup.ToArray());
                    if (i % 50 == 0)
                    {
                        this.lblStatus.Text = "正在找" + i + "的重复数据，当前找到"+arDuplicate.Count+"个...";
                        Application.DoEvents();
                    }

                }
                #endregion
                if (arDuplicate.Count == 0)
                {
                    this.lblStatus.Text = "";
                    this.Cursor = Cursors.Default;
                    MessageBox.Show("没找到重复要素!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                IWorkspaceEdit pWSEDit = RCIS.Global.GlobalEditObject.GlobalWorkspace as IWorkspaceEdit;
                pWSEDit.StartEditing(false);
                pWSEDit.StartEditOperation();
                try
                {
                    #region 批次删除
                    IQueryFilter pQueryFilter = null;

                    int iGroup = arDuplicate.Count / 30;
                    int remainder = arDuplicate.Count % 30;
                    //每30个一组，批量删除
                    for (int i = 0; i < iGroup; i++)
                    {
                        this.lblStatus.Text = "开始删除" + i + "批次数据...";
                        Application.DoEvents();
                        string where = " OBJECTID in (";
                        for (int j = 0; j < 30; j++)
                        {
                            where += arDuplicate[i * 30 + j] + ",";
                        }
                        if (where.EndsWith(","))
                            where = where.Remove(where.Length - 1, 1);
                        where += " ) ";

                        pQueryFilter = new QueryFilterClass();
                        pQueryFilter.WhereClause = where;

                        pTable.DeleteSearchedRows(pQueryFilter);
                    }

                    string where2 = " OBJECTID in (";
                    for (int j = 0; j < remainder; j++)
                    {
                        where2 += arDuplicate[iGroup * 30 + j] + ",";
                    }
                    if (where2.EndsWith(","))
                        where2 = where2.Remove(where2.Length - 1, 1);
                    where2 += " ) ";

                    pQueryFilter = new QueryFilterClass();
                    pQueryFilter.WhereClause = where2;

                    pTable.DeleteSearchedRows(pQueryFilter);
                    #endregion 
                    pWSEDit.StopEditOperation();
                    pWSEDit.StopEditing(true);
                }
                catch (Exception ex)
                {
                    pWSEDit.AbortEditOperation();
                    pWSEDit.StopEditing(false);
                    throw ex;
                }
                
                
                

                this.Cursor = Cursors.Default;
                this.lblStatus.Text = "";
                MessageBox.Show("删除完毕！共删除"+arDuplicate.Count+"个要素!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
            
            

           

            


        }

        
    }
}
