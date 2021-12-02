using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Collections;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using RCIS.GISCommon;
using RCIS.Utility;

namespace TDDC3D.datado
{
    public partial class CalmjandTpFrm : Form
    {
        public CalmjandTpFrm()
        {
            InitializeComponent();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void UpdateStatus(string txt)
        {
            memoLog.Text ="【"+ DateTime.Now.ToString() + "】:" + txt + "\r\n" + memoLog.Text;
            Application.DoEvents();
        }

        //private bool XzqTP(string flag, double dKzmj, IFeatureClass xzqClass)
        //{

        //    bool bRet = true;
        //    double dzmj = 0, dmin = 0, dmax = 0, dmean = 0;
        //    ////行政区的计算面积           
        //    IQueryFilter pQF1 = new QueryFilterClass();
        //    pQF1.WhereClause = "MSSM = '" + flag.Trim() + "' ";

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

        //    int f = Math.Abs(iDiff) / iCunNum; //整数
        //    int r = Math.Abs(iDiff) % iCunNum; //商

        //    int zf = iDiff >= 0 ? 1 : -1;  //正负

        //    ArrayList arXzqs = new ArrayList();
        //    //前r 个村 ，（f+1）*0.01，其他村 f*0.01
        //    IFeatureCursor pCursor = xzqClass.Search(pQF1, true);
        //    IFeature aXzq = null;
        //    while ((aXzq = pCursor.NextFeature()) != null)
        //    {
        //        string dm = FeatureHelper.GetFeatureStringValue(aXzq, "XZQDM");
        //        double dcmj = FeatureHelper.GetFeatureDoubleValue(aXzq, "DCMJ");
        //        arXzqs.Add(new dmmjObj(dm, dcmj));
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
           

        //    return bRet;
        //}
        /// <summary>
        /// 传入9位xzqdm
        /// </summary>
        /// <param name="xzqdm"></param>
        /// <param name="kzmj"></param>
        /// <param name="cjdcqClass"></param>
        //private void TpCjdcqByAXzq(IGeometry geo, double kzmj, IFeatureClass cjdcqClass)
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
               
        //    }
        //    OtherHelper.ReleaseComObject(pCjdcqCursor);
        //    //得到总面积
        //    double dSum = 0;
        //    foreach (dmmjObj aItem in arCdmmj)
        //    {
        //        dSum += aItem.mj;
        //    }


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
        //    // for (int kk=arCdmmj.Count-1;kk>=0;kk--)
        //    for (int kk = 0; kk < arCdmmj.Count; kk++)
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





        //}

        //private void CjtcqTp(IFeatureClass xzqClass, IFeatureClass cjdcqClass)
        //{
        //    IFeatureCursor pCursor = xzqClass.Search(null, true);
        //    IFeature aXZQ = null;
        //    try
        //    {
        //        while ((aXZQ = pCursor.NextFeature()) != null)
        //        {

        //            // string xzqdm = FeatureHelper.GetFeatureStringValue(aXZQ, "XZQDM");   //到乡一级
        //            string xzqmc = FeatureHelper.GetFeatureStringValue(aXZQ, "XZQMC");
        //            string xzqdm = FeatureHelper.GetFeatureStringValue(aXZQ, "XZQDM");
        //            double kzmj = FeatureHelper.GetFeatureDoubleValue(aXZQ, "DCMJ");

        //            TpCjdcqByAXzq(aXZQ.ShapeCopy, kzmj, cjdcqClass);


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

        private void dltbTpByCdm(string zldwdm, double kzmj, IFeatureClass dltbClass,string flag)
        {
            double dzmj = 0, dmin = 0, dmax = 0, dmean = 0;
            IQueryFilter pQF = new QueryFilterClass();
            pQF.WhereClause = "ZLDWDM='" + zldwdm + "' and MSSM='"+flag+"'";
            //通过空间查询方式获取图斑
            FeatureHelper.StatsFieldValue(dltbClass, pQF, "TBMJ", out dmax, out dmin, out dzmj, out dmean);


           // int tbnum = dltbClass.FeatureCount(pQF);
            dzmj = MathHelper.Round(dzmj,2);
            double diff = MathHelper.Round(kzmj - dzmj, 2);
            if (Math.Abs(diff) < 0.000001)
            {
                return;
            }

            //按0.01 分配
            double ddiff = diff * 100;
            ddiff = MathHelper.Round(ddiff, 2);
            long  iDiff = (long )ddiff;
            int iDltbNum = dltbClass.FeatureCount(pQF); //图斑数量
            long  f = Math.Abs(iDiff) / iDltbNum; //整数
            long  r = Math.Abs(iDiff) % iDltbNum; //商
            int zf = iDiff >= 0 ? 1 : -1;

            ArrayList arDltbs = new ArrayList();
            //前r 个村 ，（f+1）*0.01，其他村 f*0.01
            IFeatureCursor pCursor = dltbClass.Search(pQF, true);
            IFeature aDltb = null;
            while ((aDltb = pCursor.NextFeature()) != null)
            {
                string bsm = aDltb.OID.ToString();
                double mj = FeatureHelper.GetFeatureDoubleValue(aDltb, "TBMJ");
                arDltbs.Add(new dmmjObj(bsm, mj));
            }
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);

            IComparer comparer = new dmmjCompare();
            arDltbs.Sort(comparer);
            int iNum = 1;
            //IWorkspaceEdit pWsEdit = RCIS.Global.GlobalEditObject.GlobalWorkspace as IWorkspaceEdit;
            //pWsEdit.StartEditing(false);
            //pWsEdit.StartEditOperation();
            try
            {


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

                    newMj = MathHelper.Round(newMj, 2);
                    this.currWs.ExecuteSQL("update DLTB set TBMJ=" + newMj + " where OBJECTID=" + bsm + "");

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
          

        }


        private void DltbTp(IFeatureClass dltbClass, IFeatureClass cjdcqClass,string flag)
        {
            IQueryFilter pQF = new QueryFilterClass();
            pQF.WhereClause = "MSSM='" + flag + "'";

            IFeatureCursor pCursor = cjdcqClass.Search(pQF, true);
            IFeature aCun = null;
            try
            {
                while ((aCun = pCursor.NextFeature()) != null)
                {
                    string zldwdm = FeatureHelper.GetFeatureStringValue(aCun, "ZLDWDM");
                    string zldwmc = FeatureHelper.GetFeatureStringValue(aCun, "ZLDWMC");

                 

                    double kzmj = FeatureHelper.GetFeatureDoubleValue(aCun, "DCMJ");
                    dltbTpByCdm(zldwdm, kzmj, dltbClass,flag);
                    UpdateStatus(zldwmc + "下的地类图斑面积调平完毕！");                
                   
                    
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
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pQF);
            }
        }


        public IWorkspace currWs = null;

        private void CalTBMJ(IFeatureClass dltbClass, string fieldName)
        {
            int currDh = 0;

            string className = (dltbClass as IDataset).Name;

            IWorkspaceEdit pwsEdit = this.currWs as IWorkspaceEdit;
            pwsEdit.StartEditing(false);
            pwsEdit.StartEditOperation();
            try
            {
                SphereArea.SphereAreaClass area = new SphereArea.SphereAreaClass();
                //先 计算图斑面积
                IFeatureCursor pDltbCursor = dltbClass.Update(null, true);
                IFeature aDltbFea = null;
                while ((aDltbFea = pDltbCursor.NextFeature()) != null)
                {
                    if (currDh == 0)
                    {
                        ESRI.ArcGIS.Geometry.IPoint selectPoint = (aDltbFea.ShapeCopy as IArea).Centroid;
                        double X = selectPoint.X;
                        currDh = (int)(X / 1000000);////WK---带号
                    }

                    double tbmj = area.SphereArea(aDltbFea.ShapeCopy, currDh);
                    FeatureHelper.SetFeatureValue(aDltbFea, fieldName, tbmj);
                    pDltbCursor.UpdateFeature(aDltbFea);

                }

                //计算地类面积
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pDltbCursor);

                pwsEdit.StopEditOperation();
                pwsEdit.StopEditing(true);

                

                UpdateStatus(className+ fieldName+"面积计算完毕！");
            }
            catch (Exception ex)
            {
                pwsEdit.AbortEditOperation();
                pwsEdit.StopEditing(false);

                UpdateStatus(className + fieldName + "面积计算失败！" + ex.Message);
            }
        }

        //给村级调查区 赋调查面积，这个是从tbmj计算 合并后的
        private void SetCjdcqDcmj(IFeatureClass cjdcqClass, Dictionary<string, double> dmMjs,string flag)
        {

            IQueryFilter pQF = new QueryFilterClass();
            pQF.WhereClause = "MSSM='" + flag + "'";

            IWorkspaceEdit pwsEdit = this.currWs as IWorkspaceEdit;
            pwsEdit.StartEditing(false);
            pwsEdit.StartEditOperation();
            try
            {

                IFeatureCursor pCursor = cjdcqClass.Update(pQF, true);
                IFeature aFeature = null;
                while ((aFeature = pCursor.NextFeature()) != null)
                {
                    string zldwdm = FeatureHelper.GetFeatureStringValue(aFeature, "ZLDWDM");
                    string zldwmc = FeatureHelper.GetFeatureStringValue(aFeature, "ZLDWMC");
                    if (dmMjs.ContainsKey(zldwdm))
                    {
                        double mj = dmMjs[zldwdm];
                        mj = MathHelper.Round(mj, 2);
                        FeatureHelper.SetFeatureValue(aFeature, "DCMJ", mj);
                    }
                    else
                    {
                        UpdateStatus(zldwmc+"调平过程异常。。。");
                    }
                    pCursor.UpdateFeature(aFeature);

                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);

                pwsEdit.StopEditOperation();
                pwsEdit.StopEditing(true);

                UpdateStatus("村级调查区调平完毕！");
            }
            catch (Exception ex)
            {
                pwsEdit.AbortEditOperation();
                pwsEdit.StopEditing(false);

                UpdateStatus("村级调查区调平失败！" + ex.Message);
            }
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pQF);
        }

        private void CalXzqDcmj(IFeatureClass pXzqClass, IFeatureClass pDltbClass,string flag)
        {
            Dictionary<string, double> dicXzqDcmj = new Dictionary<string, double>();
            //计算 得到每个行政区的调查面积
            IQueryFilter pQF = new QueryFilterClass();
            pQF.WhereClause = "MSSM='" + flag + "'";
            IFeatureCursor pXzqCursor = pXzqClass.Search(pQF, true);
            IFeature aXzq = null;
            while ((aXzq = pXzqCursor.NextFeature()) != null)
            {
                //记录xzqdm 和面积
                string xzqdm = FeatureHelper.GetFeatureStringValue(aXzq, "XZQDM");

                ISpatialFilter pSF = new SpatialFilterClass();
                pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
                pSF.Geometry = aXzq.ShapeCopy;
                double dmax = 0, dmin = 0, dsum = 0, dmean = 0;
                FeatureHelper.StatsFieldValue(pDltbClass, pSF as IQueryFilter, "TBMJ", out dmax, out dmin, out dsum, out dmean);
                dsum = MathHelper.Round(dsum, 2);

                if (!dicXzqDcmj.ContainsKey(xzqdm))
                {
                    dicXzqDcmj.Add(xzqdm, dsum);
                }
                else
                {
                    double dcmj = dicXzqDcmj[xzqdm];
                    dcmj = MathHelper.Round(dcmj + dsum, 2);
                    dicXzqDcmj[xzqdm] = dcmj;
                }

                //FeatureHelper.SetFeatureValue(aXzq, "DCMJ", dsum);
                //pXzqCursor.UpdateFeature(aXzq);

            }

            System.Runtime.InteropServices.Marshal.ReleaseComObject(pXzqCursor);
            //更新调查面积
            GC.Collect();
            GC.WaitForPendingFinalizers();

            IWorkspaceEdit pwsEdit = this.currWs as IWorkspaceEdit;
            pwsEdit.StartEditing(false);
            pwsEdit.StartEditOperation();
            try
            {

                IFeatureCursor updateCursor = pXzqClass.Update(pQF, true);
                aXzq = null;
                while ((aXzq = updateCursor.NextFeature()) != null)
                {
                    //记录xzqdm 和面积
                    string xzqdm = FeatureHelper.GetFeatureStringValue(aXzq, "XZQDM");
                    if (dicXzqDcmj.ContainsKey(xzqdm))
                    {
                        double dsum = dicXzqDcmj[xzqdm];
                        FeatureHelper.SetFeatureValue(aXzq, "DCMJ", dsum);
                        updateCursor.UpdateFeature(aXzq);
                    }                   

                }

                System.Runtime.InteropServices.Marshal.ReleaseComObject(updateCursor);              

                pwsEdit.StopEditOperation();
                pwsEdit.StopEditing(true);

                UpdateStatus("XZQ调查面积计算完毕！");
            }
            catch (Exception ex)
            {
                pwsEdit.AbortEditOperation();
                pwsEdit.StopEditing(false);

                UpdateStatus( "XZQ调查面积计算失败！" + ex.Message);
            }
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pQF);   

        }
           
     

        
        /// <summary>
        /// 计算村或者行政区 当前图斑面积和
        /// </summary>
        /// <param name="pCunClass"></param>
        /// <param name="mjField"></param>
        /// <param name="pDltbClass"></param>
        /// <returns></returns>
        private Dictionary<string, double> getCjdcqDcmj(IFeatureClass pCunClass,string mjField, IFeatureClass pDltbClass, string flag)
        {
            Dictionary<string, double> dicCunMj = new Dictionary<string, double>();

            IQueryFilter pQF = new QueryFilterClass();
            pQF.WhereClause = "MSSM='" + flag + "'";
            IFeatureCursor pCjdcqCursor = pCunClass.Search(pQF, true);
            try
            {
                
                IFeature aCunfea = null;
                while ((aCunfea = pCjdcqCursor.NextFeature()) != null)
                {
                    string xzqdm = FeatureHelper.GetFeatureStringValue(aCunfea, mjField);
                    ISpatialFilter pSF = new SpatialFilterClass();
                    pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
                    pSF.Geometry = aCunfea.ShapeCopy;
                    double dsum = 0;
                    dsum = FeatureHelper.StatsFieldSumValue(pDltbClass, pSF as IQueryFilter, "TBMJ");
                    dsum = MathHelper.Round(dsum, 2);
                    if (!dicCunMj.ContainsKey(xzqdm))
                    {
                        dicCunMj.Add(xzqdm, dsum);
                        //如果不存在这个村
                    }
                    else
                    {
                        //如果存在这个村，则调查面积累加
                        double oldMj = dicCunMj[xzqdm];
                        dsum += oldMj;
                        dsum = MathHelper.Round(dsum,2);
                        dicCunMj[xzqdm] = dsum;
                    }

                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCjdcqCursor);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pQF);
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();

            return dicCunMj;

        }


        private void CalTbDlmj(IFeatureClass dltbClass)
        {
            IFeature aFea = null;
            IFeatureCursor pFeaCursor = null;

            IWorkspaceEdit pWSEdit = this.currWs as IWorkspaceEdit;
            pWSEdit.StartEditing(false);
            pWSEdit.StartEditOperation();

            try
            {

                pFeaCursor = dltbClass.Update(null, false);
                while ((aFea = pFeaCursor.NextFeature()) != null)
                {
                    double tbmj = FeatureHelper.GetFeatureDoubleValue(aFea, "TBMJ");
                    double tkxs = FeatureHelper.GetFeatureDoubleValue(aFea, "KCXS");
                    if (tkxs == 0)
                    {
                        FeatureHelper.SetFeatureValue(aFea, "TBDLMJ", tbmj);
                    }
                    else
                    {
                        double TKMJ = MathHelper.Round(tbmj * tkxs, 2);
                        double dlmj = MathHelper.Round(tbmj - TKMJ, 2);
                        FeatureHelper.SetFeatureValue(aFea, "KCMJ", TKMJ);
                        FeatureHelper.SetFeatureValue(aFea, "TBDLMJ", dlmj);

                    }
                    pFeaCursor.UpdateFeature(aFea);

                }

                
               
                pWSEdit.StopEditOperation();
                pWSEdit.StopEditing(true);

                UpdateStatus("图斑地类面积计算完毕！");
            }
            catch (Exception ex)
            {
                pWSEdit.AbortEditOperation();
                pWSEdit.StopEditing(false);
                UpdateStatus("图斑地类面积计算错误！" + ex.Message);
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeaCursor);
            }
        }

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
                
            }
            OtherHelper.ReleaseComObject(pCjdcqCursor);
            //得到总面积
            double dSum = 0;
            foreach (dmmjObj aItem in arCdmmj)
            {
                dSum += aItem.mj;
                dSum = MathHelper.Round(dSum,2);
            }


            //计算差值
            double diff = MathHelper.Round(kzmj - dSum, 2);
            if (Math.Abs(diff) < 0.0001)
            {
                return;
            }


            //进行排序
            IComparer comparer = new dmmjCompare();
            arCdmmj.Sort(comparer);

            //按0.01 分配
            long  iDiff = (long )(diff * 100);
            int iDltbNum = arCdmmj.Count;
            long  f = Math.Abs(iDiff) / iDltbNum; //整数
            long  r = Math.Abs(iDiff) % iDltbNum; //商
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
                this.currWs.ExecuteSQL("update  CJDCQ set  DCMJ=" + newMj + " where ZLDWDM='" + aObj.zldwdm + "' and MSSM='"+flag+"'");
                iNum++;
            }

        }


        private void simpleButton4_Click(object sender, EventArgs e)
        {
            double dKzmj = 0;
            double.TryParse(this.txtKzmj.Text, out dKzmj);

            double dHdKzmj = 0;
            double.TryParse(this.txtHdKzmj.Text, out dHdKzmj);

            IFeatureClass dltbClass = null;
            IFeatureClass xzqClass = null;
            IFeatureClass cjdcqClass = null;
            IFeatureWorkspace pFeaWs=this.currWs as IFeatureWorkspace;
            try
            {
                dltbClass = pFeaWs.OpenFeatureClass("DLTB");
                xzqClass = pFeaWs.OpenFeatureClass("XZQ");
                cjdcqClass = pFeaWs.OpenFeatureClass("CJDCQ");
            }
            catch (Exception ex)
            {
            }
            if (dltbClass == null || xzqClass == null || cjdcqClass == null)
            {
                MessageBox.Show("找不到必备图层！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            this.Cursor = Cursors.WaitCursor;
            if (this.chkRecalTbmj.Checked)
            {
                UpdateStatus("开始计算图斑面积...");
                CalTBMJ(dltbClass, "TBMJ");
                CalTBMJ(cjdcqClass, "JSMJ");
                CalTBMJ(xzqClass, "JSMJ");
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

           // CalXzqDcmj(xzqClass, dltbClass);
            
            //然后计算村调查面积
            //分别对陆地 和海岛 计算其 调查面积
            Dictionary<string, double> dCunDcmjLD = this.getCjdcqDcmj(cjdcqClass,"ZLDWDM", dltbClass,"00");
            Dictionary<string, double> dCunDcmjHD = this.getCjdcqDcmj(cjdcqClass, "ZLDWDM", dltbClass, "01");

            //foreach (KeyValuePair<string, double> aItem in dCunDcmj)
            //{
            //    this.currWs.ExecuteSQL("update CJDCQ set DCMJ=" + aItem.Value + " where ZLDWDM='" + aItem.Key.ToString().Trim() + "'");
            //}
            SetCjdcqDcmj(cjdcqClass, dCunDcmjLD,"00");
            SetCjdcqDcmj(cjdcqClass, dCunDcmjHD, "01");
           
            GC.Collect();
            GC.WaitForPendingFinalizers();
            
                     //调平  
            if (dHdKzmj == 0 && dKzmj == 0)
            {
                MessageBox.Show("控制面积没有填写！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                //对村级调查区 调查面积调平
                if (dHdKzmj > 0)
                {
                    CjdcqTp(cjdcqClass, dHdKzmj, "01");
                }
                if (dKzmj > 0)
                {
                    CjdcqTp(cjdcqClass, dKzmj, "00");
                }


                DltbTp(dltbClass, cjdcqClass, "01");
                DltbTp(dltbClass, cjdcqClass,"00");


                GC.Collect();
                GC.WaitForPendingFinalizers();
                CalXzqDcmj(xzqClass, dltbClass,"01");  //重新计算xzq调查面积
                CalXzqDcmj(xzqClass, dltbClass, "00");
                
                CalTbDlmj(dltbClass);

                OtherHelper.ReleaseComObject(xzqClass);
                OtherHelper.ReleaseComObject(cjdcqClass);
                OtherHelper.ReleaseComObject(dltbClass);

                GC.Collect();
                GC.WaitForPendingFinalizers();
                this.Cursor = Cursors.Default;
                MessageBox.Show("调平完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message);
            }

        }

        private void CalmjandTpFrm_Load(object sender, EventArgs e)
        {

        }
    }
}
