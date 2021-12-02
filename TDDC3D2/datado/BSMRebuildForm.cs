using ESRI.ArcGIS.Geodatabase;
using RCIS.GISCommon;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using ESRI.ArcGIS.Geometry;

namespace TDDC3D.datado
{
    public partial class BSMRebuildForm : Form
    {
        public BSMRebuildForm()
        {
            InitializeComponent();
        }

        
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.chkListFCs.Items.Count; i++)
            {
                this.chkListFCs.SetItemChecked(i,true);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.chkListFCs.Items.Count; i++)
            {
                this.chkListFCs.SetItemChecked(i, !this.chkListFCs.GetItemChecked(i));
            }
        }

        private Dictionary<string, string> getCDM()
        {
            string sql = "select CLASSNAME,LayerDM from SYS_YSDM  where type in ('POINT','LINE','POLYGON')  ";
            DataTable dt = RCIS.Database.LS_SetupMDBHelper.GetDataTable(sql,"tmp");
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (DataRow dr in dt.Rows)
            {
                dic.Add(dr["CLASSNAME"].ToString().Trim().ToUpper(), dr["LayerDM"].ToString().Trim());
            }
            return dic;

        }

        private void updateALayer(IFeatureClass aClass,string xiandm,string cdm)
        {


            IFeatureCursor updateCursor = aClass.Update(null, false);
            IFeature aFeature = null;
            int icount = 1;
            try
            {
                while ((aFeature = updateCursor.NextFeature()) != null)
                {
                    string bsm = xiandm + cdm + icount.ToString().PadLeft(8, '0');
                    icount++;
                    FeatureHelper.SetFeatureValue(aFeature, "BSM", bsm);
                    updateCursor.UpdateFeature(aFeature);

                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(updateCursor);
            }

        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (txtXian.Text.Trim().Length != 6)
            {
                return;
            }
            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("请稍等", "开始执行...");
            wait.Show();
            try
            {
               

                string serr = "";
                Dictionary<string, string> dicDm = getCDM();
                for (int i = 0; i < this.chkListFCs.CheckedItemsCount; i++)
                {
                    #region  
                    
                    //改用UPdate方式

                    string className = this.chkListFCs.CheckedItems[i].ToString();
                    className = RCIS.Utility.OtherHelper.GetLeftName(className);
                    string cdm = "0000";
                    if (dicDm.ContainsKey(className.ToUpper()))
                    {
                        cdm = dicDm[className.ToUpper()];
                    }
                    else
                    {
                        //找不到这个层
                        continue;
                    }
                    wait.SetCaption("处理" + className + "中...");                    
                    

                    IWorkspaceEdit pWsEdit = this.currWs as IWorkspaceEdit;
                    pWsEdit.StartEditing(false);
                    pWsEdit.StartEditOperation();

                    IFeatureClass pFC = (this.currWs as IFeatureWorkspace).OpenFeatureClass(className);                    
                   
                    try
                    {
                        updateALayer(pFC, this.txtXian.Text.Trim(), cdm);
                        pWsEdit.StopEditOperation();
                        pWsEdit.StopEditing(true);
                    }
                    catch (Exception ex)
                    {
                        pWsEdit.AbortEditOperation();
                        pWsEdit.StopEditing(false);
                        serr += className + "计算未成功！\r\n";
                    }
                   
                    #endregion 


                    

                }
                wait.Close();
                if (serr.Trim() == "")
                {
                    MessageBox.Show("计算成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("计算未成功！\r\n"+serr, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                
            }
            catch (Exception ex)
            {
                if (wait != null)
                    wait.Close();
            }
            
        }

        public IWorkspace currWs = null;

        private void BSMRebuildForm_Load(object sender, EventArgs e)
        {
            //获取所有要素类
            IFeatureWorkspace pFeaWs=this.currWs as IFeatureWorkspace;
            IFeatureDataset pFeaDS=pFeaWs.OpenFeatureDataset(RCIS.Global.AppParameters.DATASET_DEFAULT_NAME);
            List<IFeatureClass> allFC = DatabaseHelper.getAllFeatureClass(pFeaDS);
            this.chkListFCs.Items.Clear();
            foreach (IFeatureClass aClass in allFC)
            {
                string alias = aClass.AliasName;
                string className = (aClass as IDataset).Name;

                this.chkListFCs.Items.Add(className + "|" + alias, true);
            }

            //县代码
            IFeatureClass pXZQClass = null;
            string xzdm = "";
            try
            {
                pXZQClass = (this.currWs as IFeatureWorkspace).OpenFeatureClass("XZQ");
            }
            catch { }
            if (pXZQClass != null)
            {
                IFeature firstFea = GetFeaturesHelper.GetFirstFeature(pXZQClass, null);
                if (firstFea != null)
                {
                    xzdm = FeatureHelper.GetFeatureStringValue(firstFea, "XZQDM");
                    if (xzdm.Length > 6)
                    {
                        this.txtXian.Text = xzdm.Substring(0, 6);
                    }
                    else
                    {
                        this.txtXian.Text = xzdm;
                    }
                }
            }

        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < chkListFCs.Items.Count; i++)
            {
                chkListFCs.Items[i].CheckState = chkSelectAll.CheckState;
            }
        }

        private void btnLSYD_Click(object sender, EventArgs e)
        {
            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("正在计算临时用地数据……", "提示");
            wait.Show();
            IFeatureWorkspace pFeatureWorkspace = RCIS.Global.GlobalEditObject.GlobalWorkspace as IFeatureWorkspace;
            IFeatureClass pLSYDClass = pFeatureWorkspace.OpenFeatureClass("LSYD");
            IFeatureClass pDLTBClass = pFeatureWorkspace.OpenFeatureClass("DLTB");
            IFeatureCursor pUpdate = pLSYDClass.Update(null, true);
            IFeature pLSYD;
            int iBSM = pDLTBClass.FindField("BSM");
            int iGLBMS = pLSYDClass.FindField("GLTBBSM");
            while ((pLSYD = pUpdate.NextFeature()) != null)
            {
                wait.SetCaption("正在计算临时用地数据" + pLSYD.OID + "……");
                string bsm = "";
                IGeometry pGeo = pLSYD.ShapeCopy;
                List<IFeature> dltbs = RCIS.GISCommon.GetFeaturesHelper.getFeaturesByGeo(pDLTBClass, pGeo, esriSpatialRelEnum.esriSpatialRelIntersects);
                foreach (IFeature dltb in dltbs)
                {
                    IGeometry pIntersect = (pGeo as ITopologicalOperator).Intersect(dltb.ShapeCopy, esriGeometryDimension.esriGeometry2Dimension);
                    if (pIntersect != null && !pIntersect.IsEmpty)
                    {
                        bsm += dltb.get_Value(iBSM).ToString() + ",";
                    }
                }
                if (!string.IsNullOrWhiteSpace(bsm))
                {
                    pLSYD.set_Value(iGLBMS, bsm.Substring(0, bsm.Length - 1));
                    pUpdate.UpdateFeature(pLSYD);
                }
            }
            RCIS.Utility.OtherHelper.ReleaseComObject(pUpdate);
            wait.Close();
            MessageBox.Show("完成。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
