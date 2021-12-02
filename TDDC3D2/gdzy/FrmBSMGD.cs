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
using ESRI.ArcGIS.Geometry;
using System.Collections;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;

namespace TDDC3D.gdzy
{
    public partial class FrmBSMGD : Form
    {
        public IWorkspace currWs = null;

        public FrmBSMGD()
        {
            InitializeComponent();
        }

        private void FrmBSMOpt_Load(object sender, EventArgs e)
        {
            //获取所有要素类
            IFeatureWorkspace pFeaWs = this.currWs as IFeatureWorkspace;
            IFeatureDataset pFeaDS = pFeaWs.OpenFeatureDataset("GDZY");
            List<IFeatureClass> allFC = DatabaseHelper.getAllFeatureClass(pFeaDS);
            this.chkListFCs.Items.Clear();
            foreach (IFeatureClass aClass in allFC)
            {
                string alias = aClass.AliasName;
                string className = (aClass as IDataset).Name;
                if (className.ToUpper().EndsWith("XZQ") || className == "XZQJX" || className == "ZJ") continue;
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private Dictionary<string, string> getCDM()
        {
            string sql = "select CLASSNAME,LayerDM from SYS_YSDM  where type in ('POINT','LINE','POLYGON')  ";
            DataTable dt = RCIS.Database.LS_SetupMDBHelper.GetDataTable(sql, "tmp");
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (DataRow dr in dt.Rows)
            {
                dic.Add(dr["CLASSNAME"].ToString().Trim().ToUpper(), dr["LayerDM"].ToString().Trim());
            }
            return dic;

        }

        private void btnOK_Click(object sender, EventArgs e)
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
                    
                    wait.SetCaption("处理" + className + "中...");

                    IWorkspaceEdit pWsEdit = this.currWs as IWorkspaceEdit;
                    pWsEdit.StartEditing(false);
                    pWsEdit.StartEditOperation();

                    IFeatureClass pFC = (this.currWs as IFeatureWorkspace).OpenFeatureClass(className);
                    try
                    {
                        string bsm = txtXian.Text + cdm + 1.ToString().PadLeft(8, '0');
                        long maxBSM = long.Parse(bsm);
                        //return;
                        using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
                        {
                            IFeatureCursor pUpdateCursor = pFC.Update(null, true);
                            comRel.ManageLifetime(pUpdateCursor);
                            IFeature pFeature;
                            while ((pFeature = pUpdateCursor.NextFeature()) != null)
                            {
                                if (className.ToUpper().EndsWith("FLDY") || className == "KCFLDY" || className == "FZTB")
                                {
                                    string bsmVal = pFeature.get_Value(pFeature.Fields.FindField("BSM")).ToString();
                                    bsmVal = bsmVal.Substring(0, 6) + cdm + bsmVal.Substring(10);
                                    pFeature.set_Value(pFC.FindField("BSM"), bsmVal);

                                }
                                else
                                {
                                    pFeature.set_Value(pFC.FindField("BSM"), maxBSM++);
                                }
                                pUpdateCursor.UpdateFeature(pFeature);                                
                            }
                        }

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
                    MessageBox.Show("计算未成功！\r\n" + serr, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
            catch (Exception ex)
            {
                if (wait != null)
                    wait.Close();
            }
        }


        private bool IsGeometryEquals(IFeature pFeature,IFeature pFea) 
        {
            try
            {
                IRelationalOperator pRelationalOper = (pFeature.ShapeCopy) as IRelationalOperator;
                if (pRelationalOper.Equals(pFea.ShapeCopy))
                    return true;
                else
                    return false;
            }
            catch 
            {
                return false;
            }
            
        }
    }
}
