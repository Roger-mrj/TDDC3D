using System;
using System.Collections;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using RCIS.GISCommon;
using RCIS.Utility;
namespace TDDC3D.datado
{
    public partial class KzmjTpForm : Form
    {
        public KzmjTpForm()
        {
            InitializeComponent();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        public IWorkspace currWs = null;

        ///// <summary>
        ///// 获取所有 村的 计算面积及行政代码
        ///// </summary>
        ///// <returns></returns>
        //private Dictionary<string, double> getXzqJsmj()
        //{
        //    Dictionary<string, double> dicMj = new Dictionary<string, double>();
        //    if (this.currWs == null) return dicMj;
        //    IFeatureWorkspace peaWs = this.currWs as IFeatureWorkspace;
        //    IFeatureClass xzqClass = peaWs.OpenFeatureClass("XZQ");
        //    IFeatureCursor pCursor = xzqClass.Search(null,true);
        //    IFeature aFea = null;
        //    int idx1 = xzqClass.Fields.FindField("XZQDM");
        //    int idx2 = xzqClass.Fields.FindField("JSMJ");
        //    while ( (aFea=pCursor.NextFeature())!=null)
        //    {
        //        string dm = aFea.get_Value(idx1).ToString();
        //        double mj = (double)aFea.get_Value(idx2);
        //        if (!dicMj.ContainsKey(dm))
        //        {
        //            dicMj.Add(dm, mj);
        //        }
                
        //    }
        //    System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
        //    return dicMj;


        //}


       
        //private bool XzqTP(string flag, double dKzmj, IFeatureClass xzqClass)
        //{

        //    bool bRet = true;
        //    double dzmj = 0, dmin = 0, dmax = 0, dmean = 0;
        //    ////行政区的计算面积           
        //    IQueryFilter pQF1 = new QueryFilterClass();
        //    pQF1.WhereClause = "MSSM = '"+flag.Trim()+"' "; 

        //    //FeatureHelper.StatsFieldValue(xzqClass, pQF1, "JSMJ", out dmax, out dmin, out dzmj, out dmean);
        //    FeatureHelper.StatsFieldValue(xzqClass, pQF1, "DCMJ", out dmax, out dmin, out dzmj, out dmean); //控制面积改为 调查面积
            

        //    double diff = MathHelper.Round(dKzmj - dzmj, 2);

        //    if (Math.Abs(diff) < 0.00001)
        //    {
                
        //        return bRet;
        //    }

        //    //按0.01 分配
        //    int iDiff = (int)(diff * 100);
        //    int iCunNum = xzqClass.FeatureCount(pQF1); //村数量，现在是乡的数量

        //    int f = Math.Abs( iDiff) / iCunNum; //整数
        //    int r = Math.Abs( iDiff) % iCunNum; //商

        //    int zf = iDiff >= 0 ? 1 : -1;  //正负

        //    ArrayList arXzqs = new ArrayList();
        //    //前r 个村 ，（f+1）*0.01，其他村 f*0.01
        //    IFeatureCursor pCursor = xzqClass.Search(pQF1, true);
        //    IFeature aXzq = null;
        //    while ((aXzq = pCursor.NextFeature()) != null)
        //    {
        //        string dm = FeatureHelper.GetFeatureStringValue(aXzq, "XZQDM");
        //        double  dcmj = FeatureHelper.GetFeatureDoubleValue(aXzq, "DCMJ");
        //        arXzqs.Add( new dmmjObj(dm,dcmj));
        //    }
        //    System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);


        //    IComparer comparer = new dmmjCompare();
        //    arXzqs.Sort(comparer);
            
        //    int iNum = 1;
        //    for (int kk = 0; kk < arXzqs.Count; kk++)
        //    {
        //        dmmjObj aObj = arXzqs[kk] as dmmjObj;
        //        string xzqdm = aObj.zldwdm;
        //        double oldMj = aObj.mj;
        //        double newMj;
        //        if (iNum <= Math.Abs(r))
        //        {
        //            newMj = oldMj + (f + 1) * zf * 0.01;
        //        }
        //        else
        //        {
        //            newMj = oldMj + (f) * zf * 0.01;
        //        }
        //        iNum++;

        //        this.currWs.ExecuteSQL("update XZQ set DCMJ=" + newMj + " where XZQDM='" + xzqdm + "'");

        //    }
        //    #region 
        //    //int idx = xzqClass.Fields.FindField("DCMJ");  //控制面积  改为调查面积
        //    //ICursor pCursor = null;
        //    //IRow aFea = null;
        //    //IWorkspaceEdit pWsEdit = this.currWs as IWorkspaceEdit;
        //    //pWsEdit.StartEditing(true);
        //    //pWsEdit.StartEditOperation();
        //    //try
        //    //{
        //    //    int iNum = 1;

                
        //    //    ITableSort tableSort = new TableSortClass();
        //    //    tableSort.Fields = "DCMJ";
        //    //    tableSort.set_Ascending("DCMJ", false);
        //    //    tableSort.QueryFilter = pQF1;
        //    //    tableSort.Table = xzqClass as ITable;
        //    //    tableSort.Sort(null);
        //    //    pCursor = tableSort.Rows;

        //    //    while ((aFea = pCursor.NextRow()) != null)
        //    //    {
        //    //        int oid = aFea.OID;

        //    //        string mc = FeatureHelper.GetFeatureStringValue(aFea as IFeature, "XZQMC");
        //    //        int idx = aFea.Fields.FindField("DCMJ");
        //    //        double oldMj = (double)aFea.get_Value(idx);
        //    //        double newMj;
        //    //        if (iNum <= Math.Abs(r))
        //    //        {
        //    //            newMj = oldMj + (f + 1) * zf* 0.01;
        //    //        }
        //    //        else
        //    //        {
        //    //            newMj = oldMj + (f) *zf*  0.01;
        //    //        }
        //    //        aFea.set_Value(idx, MathHelper.RoundEx( newMj,2));
        //    //        aFea.Store();

        //    //        iNum++;
        //    //    }

        //    //    pWsEdit.StopEditOperation();
        //    //    pWsEdit.StopEditing(true);
                

        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    pWsEdit.AbortEditOperation();
        //    //    pWsEdit.StopEditing(false);
        //    //    bRet = false;
        //    //}
        //    //finally
        //    //{

        //    //    OtherHelper.ReleaseComObject(pCursor);
        //    //    OtherHelper.ReleaseComObject(pQF1);

        //    //}
        //    #endregion 

        //    return bRet;
        //}
        private void UpdateStatus(string txt)
        {
            memoLog.Text =DateTime.Now.ToString() + ":" + txt+ "\r\n" +memoLog.Text;
            Application.DoEvents();
        }

        #region 
        ////按村调平dltb
        //private void dltbTpByACun(IGeometry geo,  double kzmj,IFeatureClass dltbClass)
        //{
            
        //    double dzmj = 0, dmin = 0, dmax = 0, dmean = 0;
        //    ISpatialFilter pSF = new SpatialFilterClass();
        //    pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
        //    pSF.Geometry = geo;
        //    //通过空间查询方式获取图斑
        //    FeatureHelper.StatsFieldValue(dltbClass, pSF as IQueryFilter, "TBMJ", out dmax, out dmin, out dzmj, out dmean);

        //    double diff = MathHelper.Round(kzmj - dzmj, 2);
        //    if (Math.Abs(diff) < 0.00001)
        //    {                
        //        return;
        //    }

        //    //按0.01 分配
        //    int iDiff = (int)(diff * 100);
        //    int iDltbNum = dltbClass.FeatureCount(pSF); //图斑数量
        //    int f = Math.Abs( iDiff) / iDltbNum; //整数
        //    int r = Math.Abs( iDiff) % iDltbNum; //商
        //    int zf = iDiff >= 0 ? 1 : -1;


        //    //前r 个图斑 ，（f+1）*0.01，其他图斑 f*0.01

        //   // int idx = dltbClass.Fields.FindField("TBMJ");
        //    ICursor pCursor = null;
        //    IRow aFea = null;
        //    IWorkspaceEdit pWsEdit = this.currWs as IWorkspaceEdit;
        //    pWsEdit.StartEditing(true);
        //    pWsEdit.StartEditOperation();
        //    try
        //    {
        //        int iNum = 1;
                
        //        ITableSort tableSort = new TableSortClass();
        //        tableSort.Fields = "TBMJ";
        //        tableSort.set_Ascending("TBMJ", false);
        //        tableSort.QueryFilter =pSF as IQueryFilter ;
        //        tableSort.Table = dltbClass as ITable;
        //        tableSort.Sort(null);
        //        pCursor = tableSort.Rows;

        //        while ((aFea = pCursor.NextRow()) != null)
        //        {
        //            int oid = aFea.OID;
        //            int idx = aFea.Fields.FindField("TBMJ");

        //            double oldMj = (double)aFea.get_Value(idx);
        //            double newMj;
        //            if (iNum <= Math.Abs(r))
        //            {
        //                newMj = oldMj + (f + 1) * zf* 0.01;
        //            }
        //            else
        //            {
        //                //如果 f 是0 ，后面的不要了
        //                if (f == 0)
        //                    break;
        //                newMj = oldMj + (f) * zf* 0.01;
        //            }

        //            if (newMj < 0)
        //            {
        //            }

        //            aFea.set_Value(idx,MathHelper.RoundEx( newMj,2));
        //            aFea.Store();

        //            iNum++;
        //        }

        //        pWsEdit.StopEditOperation();
        //        pWsEdit.StopEditing(true);
                

        //    }
        //    catch (Exception ex)
        //    {
        //        pWsEdit.AbortEditOperation();
        //        pWsEdit.StopEditing(false);
        //    }
        //    finally
        //    {

        //        OtherHelper.ReleaseComObject(pCursor);
        //    }

        //}

        #endregion 

        private void dltbTpByCdm(string zldwdm, double kzmj, IFeatureClass dltbClass)
        {
            double dzmj = 0, dmin = 0, dmax = 0, dmean = 0;
            IQueryFilter pQF = new QueryFilterClass();
            pQF.WhereClause = "ZLDWDM='" + zldwdm + "'";
            //通过空间查询方式获取图斑
            FeatureHelper.StatsFieldValue(dltbClass, pQF, "TBMJ", out dmax, out dmin, out dzmj, out dmean);

            double diff = MathHelper.Round(kzmj - dzmj, 2);
            if (Math.Abs(diff) < 0.00001)
            {
                return;
            }

            //按0.01 分配
            long  iDiff = (long )(diff * 100);
            long  iDltbNum = dltbClass.FeatureCount(pQF); //图斑数量
            long  f = Math.Abs(iDiff) / iDltbNum; //整数
            long  r = Math.Abs(iDiff) % iDltbNum; //商
            int zf = iDiff >= 0 ? 1 : -1;

            ArrayList arDltbs = new ArrayList();
            //前r 个村 ，（f+1）*0.01，其他村 f*0.01
            IFeatureCursor pCursor = dltbClass.Search(pQF, true);
            IFeature aDltb = null;
            while ((aDltb = pCursor.NextFeature()) != null)
            {
                //string BSM = FeatureHelper.GetFeatureStringValue(aDltb, "BSM");
                string bsm = aDltb.OID.ToString();
                double mj = FeatureHelper.GetFeatureDoubleValue(aDltb, "TBMJ");
                arDltbs.Add(new dmmjObj(bsm, mj));
            }
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);

            IComparer comparer = new dmmjCompare();
            arDltbs.Sort(comparer);
            int iNum = 1;
            for (int kk = 0; kk < arDltbs.Count; kk++)
            {
                dmmjObj aObj = arDltbs[kk] as dmmjObj;
                string bsm = aObj.zldwdm;
                double oldMj = aObj.mj;
                double newMj;
                if (iNum <= Math.Abs(r))
                {
                    newMj = oldMj + (f + 1) * zf * 0.01;
                }
                else
                {
                    newMj = oldMj + (f) * zf * 0.01;
                }
                iNum++;

                if (bsm == "1210")
                {
                
                }
                newMj = MathHelper.Round(newMj, 2);
                this.currWs.ExecuteSQL("update DLTB set TBMJ=" + newMj + " where OBJECTID=" + bsm + "");

            }

            #region 
            ////前r 个图斑 ，（f+1）*0.01，其他图斑 f*0.01
            //ICursor pCursor = null;
            //IRow aFea = null;
            //IWorkspaceEdit pWsEdit = this.currWs as IWorkspaceEdit;
            //pWsEdit.StartEditing(true);
            //pWsEdit.StartEditOperation();
            //try
            //{
            //    int iNum = 1;

            //    ITableSort tableSort = new TableSortClass();
            //    tableSort.Fields = "TBMJ";
            //    tableSort.set_Ascending("TBMJ", false);
            //    tableSort.QueryFilter = pQF as IQueryFilter;
            //    tableSort.Table = dltbClass as ITable;
            //    tableSort.Sort(null);
            //    pCursor = tableSort.Rows;

            //    while ((aFea = pCursor.NextRow()) != null)
            //    {
            //        int oid = aFea.OID;
            //        int idx = aFea.Fields.FindField("TBMJ");

            //        double oldMj = (double)aFea.get_Value(idx);
            //        double newMj;
            //        if (iNum <= Math.Abs(r))
            //        {
            //            newMj = oldMj + (f + 1) * zf * 0.01;
            //        }
            //        else
            //        {
            //            //如果 f 是0 ，后面的不要了
            //            if (f == 0)
            //                break;
            //            newMj = oldMj + (f) * zf * 0.01;
            //        }

            //        if (newMj < 0)
            //        {
            //        }

            //        aFea.set_Value(idx, MathHelper.RoundEx(newMj, 2));
            //        aFea.Store();

            //        iNum++;
            //    }

            //    pWsEdit.StopEditOperation();
            //    pWsEdit.StopEditing(true);


            //}
            //catch (Exception ex)
            //{
            //    pWsEdit.AbortEditOperation();
            //    pWsEdit.StopEditing(false);
            //}
            //finally
            //{

            //    OtherHelper.ReleaseComObject(pCursor);
            //}
            #endregion 

        }

        private void DltbTp(IFeatureClass dltbClass, IFeatureClass cjdcqClass)
        {
            IFeatureCursor pCursor = cjdcqClass.Search(null, true);
            IFeature aCun = null;
            try
            {
                while ((aCun = pCursor.NextFeature()) != null)
                {
                    double kzmj = FeatureHelper.GetFeatureDoubleValue(aCun, "DCMJ");
                    string zldwdm = FeatureHelper.GetFeatureStringValue(aCun, "ZLDWDM");
                    string zldwmc = FeatureHelper.GetFeatureStringValue(aCun, "ZLDWMC");

                   
                    //dltbTpByACun(aCun.ShapeCopy, kzmj,dltbClass);
                    dltbTpByCdm(zldwdm, kzmj, dltbClass);

                    UpdateStatus(zldwmc + "下的图斑面积调平完毕，调平后总调查面积为" + kzmj + ".");

                }
                UpdateStatus("DLTB调平完成。");
            }
            catch (Exception ex)
            {
                UpdateStatus("DLTB调平失败！" + ex.Message);
            }
            finally
            {

                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
            }
        }

        private void CalXzqDcmj(IFeatureClass pXzqClass, IFeatureClass pDltbClass)
        {
            IWorkspaceEdit pwsEdit = this.currWs as IWorkspaceEdit;
            pwsEdit.StartEditing(false);
            pwsEdit.StartEditOperation();
            try
            {


                IFeatureCursor pXzqCursor = pXzqClass.Update(null, true);
                IFeature aXzq = null;
                while ((aXzq = pXzqCursor.NextFeature()) != null)
                {

                    ISpatialFilter pSF = new SpatialFilterClass();
                    pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
                    pSF.Geometry = aXzq.ShapeCopy;
                    double dmax = 0, dmin = 0, dsum = 0, dmean = 0;
                    FeatureHelper.StatsFieldValue(pDltbClass, pSF as IQueryFilter, "TBMJ", out dmax, out dmin, out dsum, out dmean);
                    dsum = MathHelper.Round(dsum, 2);
                    FeatureHelper.SetFeatureValue(aXzq, "DCMJ", dsum);
                    pXzqCursor.UpdateFeature(aXzq);

                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pXzqCursor);

                pwsEdit.StopEditOperation();
                pwsEdit.StopEditing(true);

                UpdateStatus("XZQ调查面积赋值完毕！");
            }
            catch (Exception ex)
            {
                pwsEdit.AbortEditOperation();
                pwsEdit.StopEditing(false);

                UpdateStatus("XZQ调查面积计算失败！" + ex.Message);
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
           
           

            

        }

        /// <summary>
        /// 传入9位xzqdm
        /// </summary>
        /// <param name="xzqdm"></param>
        /// <param name="kzmj"></param>
        /// <param name="cjdcqClass"></param>
        //private void TpCjdcqByAXzq(IGeometry geo,double kzmj, IFeatureClass cjdcqClass)
        //{
        //    //double dzmj = 0, dmin = 0, dmax = 0, dmean = 0;
        //    ISpatialFilter pSF = new SpatialFilterClass();
        //    pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
        //    pSF.Geometry = geo;

        //    ArrayList arCdmmj = new ArrayList();
        //    //得到 村的代码和原来的调查面积
        //    IFeatureCursor pCjdcqCursor = cjdcqClass.Search(pSF, true);
        //    IFeature aCun = null;
        //    try
        //    {
        //        while ((aCun = pCjdcqCursor.NextFeature()) != null)
        //        {
        //            string cdm = FeatureHelper.GetFeatureStringValue(aCun, "ZLDWDM");
        //            double mj = FeatureHelper.GetFeatureDoubleValue(aCun, "DCMJ");
        //            dmmjObj aObj = new dmmjObj();
        //            aObj.zldwdm = cdm;
        //            aObj.mj = mj;

        //            bool exist = false;
        //            foreach (dmmjObj aItem in arCdmmj)
        //            {
        //                if (aItem.zldwdm == aObj.zldwdm)
        //                {
        //                    exist = true;
        //                    break;
        //                }

        //            }
        //            if (!exist)
        //            {
        //                arCdmmj.Add(aObj);
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        OtherHelper.ReleaseComObject(pCjdcqCursor);
        //    }

        //    //得到总面积
        //    double dSum = 0;
        //    foreach (dmmjObj aItem in arCdmmj)
        //    {
        //        dSum += aItem.mj;
        //    }

        //   // FeatureHelper.StatsFieldValue(cjdcqClass, pSF as IQueryFilter, "DCMJ", out dmax, out dmin, out dzmj, out dmean);

        //    double diff = MathHelper.Round(kzmj - dSum, 2);
        //    if (Math.Abs(diff) < 0.00001)
        //    {
        //        return;
        //    }

           
        //    //进行排序

        //    IComparer comparer = new dmmjCompare();
        //    arCdmmj.Sort(comparer);



        //    //按0.01 分配
        //    int iDiff = (int)(diff * 100);
        //    int iDltbNum = arCdmmj.Count;
        //    int f = Math.Abs(iDiff) / iDltbNum; //整数
        //    int r = Math.Abs(iDiff) % iDltbNum; //商
        //    int zf = iDiff >= 0 ? 1 : -1;

            

        //    int iNum = 1;
        //    //前r 个 ，（f+1）*0.01，其他图斑 f*0.01
        //   // for (int kk=arCdmmj.Count-1;kk>=0;kk--)
        //    for (int kk=0;kk<arCdmmj.Count;kk++)
        //    {
        //        dmmjObj aObj = arCdmmj[kk] as dmmjObj;
        //        double oldMj = aObj.mj;
        //        double newMj;
        //        if (iNum <= Math.Abs(r))
        //        {
        //            newMj = oldMj + (f + 1) * zf * 0.01;
        //        }
        //        else
        //        {
        //            //如果 f 是0 ，后面的不要了
        //            if (f == 0)
        //                break;
        //            newMj = oldMj + (f) * zf * 0.01;
        //        }

               

        //        newMj = MathHelper.Round(newMj, 2);
        //        this.currWs.ExecuteSQL("update  CJDCQ set  DCMJ=" + newMj + " where ZLDWDM='" + aObj.zldwdm + "'");
        //        iNum++;
        //    }



        //    #region 废弃
        //    //ICursor pCursor = null;
        //    //IRow aFea = null;
        //    //IWorkspaceEdit pWsEdit = this.currWs as IWorkspaceEdit;
        //    //pWsEdit.StartEditing(true);
        //    //pWsEdit.StartEditOperation();
        //    //try
        //    //{
        //    //    int iNum = 1;

        //    //    ITableSort tableSort = new TableSortClass();
        //    //    tableSort.Fields = "DCMJ";
        //    //    tableSort.set_Ascending("DCMJ", false);
        //    //    tableSort.QueryFilter = pSF as IQueryFilter;
        //    //    tableSort.Table = cjdcqClass as ITable;
        //    //    tableSort.Sort(null);
        //    //    pCursor = tableSort.Rows;

        //    //    while ((aFea = pCursor.NextRow()) != null)
        //    //    {
        //    //        int oid = aFea.OID;
        //    //        int idx = aFea.Fields.FindField("DCMJ");
        //    //        double oldMj = (double)aFea.get_Value(idx);
        //    //        double newMj;
        //    //        if (iNum <= Math.Abs(r))
        //    //        {
        //    //            newMj = oldMj + (f + 1) * zf * 0.01;
        //    //        }
        //    //        else
        //    //        {
        //    //            //如果 f 是0 ，后面的不要了
        //    //            if (f == 0)
        //    //                break;
        //    //            newMj = oldMj + (f) * zf * 0.01;
        //    //        }

        //    //        if (newMj < 0)
        //    //        {
        //    //        }

        //    //        aFea.set_Value(idx, MathHelper.RoundEx(newMj, 2));
        //    //        aFea.Store();

        //    //        iNum++;
        //    //    }

        //    //    pWsEdit.StopEditOperation();
        //    //    pWsEdit.StopEditing(true);


        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    pWsEdit.AbortEditOperation();
        //    //    pWsEdit.StopEditing(false);
        //    //}
        //    //finally
        //    //{

        //    //    OtherHelper.ReleaseComObject(pCursor);
        //    //}
        //    #endregion 


        //}

        #region 
        //private void TpCjdcqByAXZQ(IGeometry geo, double kzmj, IFeatureClass cjdcqClass)
        //{

        //    double dzmj = 0, dmin = 0, dmax = 0, dmean = 0;
        //    //通过空间查询方式获取村的dcmj
        //    ISpatialFilter pSF = new SpatialFilterClass();
        //    pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
        //    pSF.Geometry = geo;
        //    FeatureHelper.StatsFieldValue(cjdcqClass, pSF as IQueryFilter, "DCMJ", out dmax, out dmin, out dzmj, out dmean);



        //    double diff = MathHelper.Round(kzmj - dzmj, 2);
        //    if (Math.Abs(diff) < 0.00001)
        //    {
        //        return;
        //    }

        //    //按0.01 分配
        //    int iDiff = (int)(diff * 100);
        //    int iDltbNum = cjdcqClass.FeatureCount(pSF); //图斑数量
        //    int f = Math.Abs(iDiff) / iDltbNum; //整数
        //    int r = Math.Abs(iDiff) % iDltbNum; //商
        //    int zf = iDiff >= 0 ? 1 : -1;


        //    //前r 个图斑 ，（f+1）*0.01，其他图斑 f*0.01


        //    ICursor pCursor = null;
        //    IRow aFea = null;
        //    IWorkspaceEdit pWsEdit = this.currWs as IWorkspaceEdit;
        //    pWsEdit.StartEditing(true);
        //    pWsEdit.StartEditOperation();
        //    try
        //    {
        //        int iNum = 1;

        //        ITableSort tableSort = new TableSortClass();
        //        tableSort.Fields = "DCMJ";
        //        tableSort.set_Ascending("DCMJ", false);
        //        tableSort.QueryFilter = pSF as IQueryFilter;
        //        tableSort.Table = cjdcqClass as ITable;
        //        tableSort.Sort(null);
        //        pCursor = tableSort.Rows;

        //        while ((aFea = pCursor.NextRow()) != null)
        //        {
        //            int oid = aFea.OID;
        //            int idx = aFea.Fields.FindField("DCMJ");
        //            double oldMj = (double)aFea.get_Value(idx);
        //            double newMj;
        //            if (iNum <= Math.Abs(r))
        //            {
        //                newMj = oldMj + (f + 1) * zf * 0.01;
        //            }
        //            else
        //            {
        //                //如果 f 是0 ，后面的不要了
        //                if (f == 0)
        //                    break;
        //                newMj = oldMj + (f) * zf * 0.01;
        //            }

        //            if (newMj < 0)
        //            {
        //            }

        //            aFea.set_Value(idx, MathHelper.RoundEx(newMj, 2));
        //            aFea.Store();

        //            iNum++;
        //        }

        //        pWsEdit.StopEditOperation();
        //        pWsEdit.StopEditing(true);


        //    }
        //    catch (Exception ex)
        //    {
        //        pWsEdit.AbortEditOperation();
        //        pWsEdit.StopEditing(false);
        //    }
        //    finally
        //    {

        //        OtherHelper.ReleaseComObject(pCursor);
        //    }
        //}
        #endregion 
        //private void CjtcqTp(IFeatureClass xzqClass,IFeatureClass cjdcqClass)
        //{
        //    IFeatureCursor pCursor = xzqClass.Search(null, true);
        //    IFeature aXZQ = null;
        //    try
        //    {
        //        while ((aXZQ = pCursor.NextFeature()) != null)
        //        {

        //           // string xzqdm = FeatureHelper.GetFeatureStringValue(aXZQ, "XZQDM");   //到乡一级
        //            string xzqmc = FeatureHelper.GetFeatureStringValue(aXZQ, "XZQMC");
        //            string xzqdm = FeatureHelper.GetFeatureStringValue(aXZQ, "XZQDM");
        //            double kzmj = FeatureHelper.GetFeatureDoubleValue(aXZQ, "DCMJ");

        //            TpCjdcqByAXzq(aXZQ.ShapeCopy, kzmj,cjdcqClass);


        //            UpdateStatus(xzqmc + "下的村级调查去面积调平完毕，调平后总面积为" + kzmj + ".");

        //        }
        //        UpdateStatus("CJDCQ调平完毕。");
        //    }
        //    catch (Exception ex)
        //    {
        //        UpdateStatus("CDJCQ调平失败！" + ex.Message);
        //    }
        //    finally
        //    {

        //        System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
        //    }
        //}

        //按照 mssm 为 00 或者 01 的分别 按照控制面积进行调平
        private void CjdcqTp(IFeatureClass cjdcqClass, double kzmj, string flag)
        {
            ArrayList arCdmmj = new ArrayList();

            //获取所有需要调平的 村
            IQueryFilter pQF = new QueryFilterClass();
            pQF.WhereClause = "MSSM='" + flag + "'";            
            //得到 村的代码和原来的调查面积
            IFeatureCursor pCjdcqCursor = cjdcqClass.Search(pQF, true);
            IFeature aCun = null;
            try
            {
                while ((aCun = pCjdcqCursor.NextFeature()) != null)
                {
                    string cdm = FeatureHelper.GetFeatureStringValue(aCun, "ZLDWDM");
                    double mj = FeatureHelper.GetFeatureDoubleValue(aCun, "DCMJ");
                    dmmjObj aObj = new dmmjObj();
                    aObj.zldwdm = cdm;
                    aObj.mj = mj;

                    bool exist = false;
                    foreach (dmmjObj aItem in arCdmmj)
                    {
                        if (aItem.zldwdm == aObj.zldwdm)
                        {
                            exist = true;

                            //aItem.mj = aItem.mj + aObj.mj;
                            break;
                        }

                    }
                    if (!exist)
                    {
                        arCdmmj.Add(aObj);
                    }
                    

                }
            }
            catch (Exception ex)
            {
                OtherHelper.ReleaseComObject(pCjdcqCursor);
            }

            //得到总面积
            double dSum = 0;
            foreach (dmmjObj aItem in arCdmmj)
            {
                dSum += aItem.mj;
            }

          
            //计算差值
            double diff = MathHelper.Round(kzmj - dSum, 2);
            if (Math.Abs(diff) < 0.00001)
            {
                return;
            }


            //进行排序
            IComparer comparer = new dmmjCompare();
            arCdmmj.Sort(comparer);

            //按0.01 分配
            Int64 iDiff = (Int64)(diff * 100);
            Int64 iDltbNum = arCdmmj.Count;
            Int64 iDiffAbs = iDiff;
            if (iDiff<0)
            {
                iDiffAbs=iDiff*(-1);
            }


            Int64 f = iDiffAbs / iDltbNum; //整数
            Int64 r = iDiffAbs % iDltbNum; //商
            int zf = iDiff >= 0 ? 1 : -1;

            int iNum = 1;
            //前r 个 ，（f+1）*0.01，其他图斑 f*0.01
            // for (int kk=arCdmmj.Count-1;kk>=0;kk--)
            for (int kk = 0; kk < arCdmmj.Count; kk++)
            {
                dmmjObj aObj = arCdmmj[kk] as dmmjObj;
                double oldMj = aObj.mj;
                double newMj;
                if (iNum <= Math.Abs(r))
                {
                    newMj = oldMj + (f + 1) * zf * 0.01;
                }
                else
                {
                    //如果 f 是0 ，后面的不要了
                    if (f == 0)
                        break;
                    newMj = oldMj + (f) * zf * 0.01;
                }



                newMj = MathHelper.Round(newMj, 2);
                this.currWs.ExecuteSQL("update  CJDCQ set  DCMJ=" + newMj + " where ZLDWDM='" + aObj.zldwdm + "'");
                iNum++;
            }

        }


       

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            #region 前置条件
            if (this.currWs == null) return;
            IFeatureClass XzqClass = null;
            IFeatureClass DltbClass = null;
            IFeatureClass CjdcqClass = null;
            IFeatureWorkspace peaWs = this.currWs as IFeatureWorkspace;
            try
            {
                XzqClass = peaWs.OpenFeatureClass("XZQ");
                DltbClass = peaWs.OpenFeatureClass("DLTB");
                CjdcqClass = peaWs.OpenFeatureClass("CJDCQ");
            }
            catch (Exception ex)
            {
            }
            if (XzqClass == null || CjdcqClass == null || XzqClass == null)
            {
                MessageBox.Show("缺少必备图层！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            #endregion 

            double dKzmj = 0;
            double.TryParse(this.txtKzmj.Text, out dKzmj);

            double dHdKzmj = 0;
            double.TryParse(this.txtHdKzmj.Text, out dHdKzmj);

            #region  //行政区面积调平
            bool bRet = true;
            try
            {
               
                if (dHdKzmj > 0)
                {
                    CjdcqTp(CjdcqClass, dHdKzmj, "01");
                }
                if (dKzmj > 0)
                {
                    CjdcqTp(CjdcqClass, dKzmj, "00");
                }
                

                DltbTp(DltbClass, CjdcqClass);
                //然后 把地类图斑面积和 赋给 xzq
                CalXzqDcmj(XzqClass, DltbClass);

                OtherHelper.ReleaseComObject(XzqClass);
                OtherHelper.ReleaseComObject(CjdcqClass);
                OtherHelper.ReleaseComObject(DltbClass);
                MessageBox.Show("调平完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            #endregion 

           
        }

        private void KzmjTpForm_Load(object sender, EventArgs e)
        {

        }
    }

    public class dmmjObj
    {
        public string zldwdm = "";
        public double mj = 0;
        public dmmjObj()
        {
        }
        public dmmjObj(string _dm, double _mj)
        {
            this.zldwdm = _dm;
            this.mj = _mj;
        }
    }

    public class dmmjCompare : IComparer
    {
        int IComparer.Compare(object x, object y)
        {
            dmmjObj fea1 = x as dmmjObj;
            dmmjObj fea2 = y as dmmjObj;

            try
            {
                if (fea1.mj > fea2.mj)
                {
                    return -1;
                }
               else
                {
                    return 1;
                }
            }
            catch (Exception ex)
            {
                return 1;
            }
           
        }
    }

}
