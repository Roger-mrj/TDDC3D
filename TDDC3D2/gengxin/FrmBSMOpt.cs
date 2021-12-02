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

namespace TDDC3D.gengxin
{
    public partial class FrmBSMOpt : Form
    {
        public IWorkspace currWs = null;

        public FrmBSMOpt()
        {
            InitializeComponent();
        }

        private void FrmBSMOpt_Load(object sender, EventArgs e)
        {
            //获取所有要素类
            IFeatureWorkspace pFeaWs = this.currWs as IFeatureWorkspace;
            IFeatureDataset pFeaDS = pFeaWs.OpenFeatureDataset("TDGX");
            List<IFeatureClass> allFC = DatabaseHelper.getAllFeatureClass(pFeaDS);
            this.chkListFCs.Items.Clear();
            foreach (IFeatureClass aClass in allFC)
            {
                string alias = aClass.AliasName;
                string className = (aClass as IDataset).Name;
                if (className.ToUpper().EndsWith("GXGC") || className == "XZQJXGX" || className == "CJDCQJXGX") continue;
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
                //IFeature firstFea = GetFeaturesHelper.GetFirstFeature(pXZQClass, null);
                //if (firstFea != null)
                //{
                //    xzdm = FeatureHelper.GetFeatureStringValue(firstFea, "XZQDM");
                //    if (xzdm.Length > 6)
                //    {
                //        this.txtXian.Text = xzdm.Substring(0, 6);
                //    }
                //    else
                //    {
                //        this.txtXian.Text = xzdm;
                //    }
                //}
                List<string> dms = FeatureHelper.GetDMMCDicByQueryDef(this.currWs as IFeatureWorkspace, "XZQ", "XZQDM", 6);
                txtXian.Properties.Items.Clear();
                foreach (string dm in dms)
                {
                    txtXian.Properties.Items.Add(dm);
                }
                if (txtXian.Properties.Items.Count > 0) txtXian.SelectedIndex = 0;
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
                    string classbase = className.Substring(0, className.Length - 2);
                    string cdm = "0000";
                    if (dicDm.ContainsKey(className.ToUpper()))
                    {
                        cdm = dicDm[className.ToUpper()];
                    }
                    else
                    {
                        if (dicDm.ContainsKey(classbase))
                        {
                            cdm = dicDm[className.ToUpper()];
                        }
                    }
                    wait.SetCaption("处理" + className + "中...");

                    IWorkspaceEdit pWsEdit = this.currWs as IWorkspaceEdit;
                    pWsEdit.StartEditing(false);
                    pWsEdit.StartEditOperation();

                    IFeatureClass pFC = (this.currWs as IFeatureWorkspace).OpenFeatureClass(className);
                    if (classbase == "CZKFBJ")
                        classbase = "CSKFBJ";
                    if (classbase.ToUpper() == "HAX")
                        continue;
                    IFeatureClass pBC = (this.currWs as IFeatureWorkspace).OpenFeatureClass(classbase);
                    
                    try
                    {
                        string where = "";
                        if (pBC.FindField("XZQDM") != -1)
                            where = "XZQDM like '"+txtXian.Text+"%'";
                        else if (pBC.FindField("ZLDWDM") != -1)
                            where = "ZLDWDM LIKE '"+txtXian.Text+"%'";
                        else if (pBC.FindField("CZCDM") != -1)
                            where = "CZCDM LIKE '" + txtXian.Text + "%'";
                        else
                            where = "BSM  LIKE '" + txtXian.Text + "%'";
                        string max = RCIS.GISCommon.FeatureHelper.GetMaxStringOrderBy(pBC, "BSM", where);
                        long maxBSM = 0;
                        if (max.Length > 8)
                        {
                            maxBSM = long.Parse(max.Substring(10, 8));
                            cdm = max.Substring(0, 10);
                        }
                        else
                        {
                            cdm = txtXian.Text.Trim() + cdm;
                        }
                        maxBSM++;
                        using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
                        {
                            IFeatureCursor pUpdateCursor = pFC.Update(null, true);
                            comRel.ManageLifetime(pUpdateCursor);
                            IFeature pFeature;
                            IFeatureLayer pLayer = new FeatureLayerClass();
                            pLayer.FeatureClass = pBC;
                            IIdentify pIdentify = pLayer as IIdentify;
                            if (classbase.ToUpper() == "DLTB" || classbase.ToUpper() == "XZQ" || classbase.ToUpper() == "CJDCQ")
                            {
                                while ((pFeature = pUpdateCursor.NextFeature()) != null)
                                {
                                    IRelationalOperator pRel = pFeature.ShapeCopy as IRelationalOperator;
                                    bool b = false;
                                    IArray pArry = pIdentify.Identify(pFeature.ShapeCopy);
                                    if (pArry != null)
                                    {
                                        for (int m = 0; m < pArry.Count; m++)
                                        {
                                            IFeatureIdentifyObj idObj = pArry.get_Element(m) as IFeatureIdentifyObj;
                                            IRowIdentifyObject pRow = idObj as IRowIdentifyObject;
                                            IFeature pFea = pRow.Row as IFeature;
                                            if (pRel.Equals(pFea.ShapeCopy))
                                            {
                                                b = true;

                                                string bgqbsm = pFea.get_Value(pFea.Fields.FindField("BSM")).ToString().Trim();
                                                if (bgqbsm.Substring(0, 6) == txtXian.Text.Trim())
                                                    pFeature.set_Value(pFeature.Fields.FindField("BSM"), bgqbsm);
                                                else
                                                {
                                                    pFeature.set_Value(pFeature.Fields.FindField("BSM"), cdm + maxBSM++.ToString().PadLeft(8, '0'));
                                                    pFeature.set_Value(pFeature.Fields.FindField("BZ"), "S");
                                                }
                                                //if (txtXian.Text != pFeature.get_Value(pFeature.Fields.FindField("ZLDWDM")).ToString().Substring(0, 6))
                                                //{
                                                //    pFeature.set_Value(pFeature.Fields.FindField("BSM"), maxBSM++);
                                                //    pFeature.set_Value(pFeature.Fields.FindField("BZ"), "S");
                                                //}
                                                //else
                                                //{
                                                //    pFeature.set_Value(pFeature.Fields.FindField("BSM"), pFea.get_Value(pFea.Fields.FindField("BSM")));
                                                //}
                                                break;
                                            }
                                        }
                                    }
                                    if (b == false)
                                    {
                                        pFeature.set_Value(pFeature.Fields.FindField("BSM"), cdm + maxBSM++.ToString().PadLeft(8, '0'));

                                        //pFeature.set_Value(pFeature.Fields.FindField("BSM"), maxBSM);
                                        //if (maxBSM.ToString().Length < 8)
                                        //{
                                        //    string bsm = txtXian.Text.Trim() + cdm + maxBSM.ToString().PadLeft(8, '0');
                                        //    pFeature.set_Value(pFeature.Fields.FindField("BSM"), bsm);

                                        //}
                                        //else
                                        //{
                                        //    pFeature.set_Value(pFeature.Fields.FindField("BSM"), maxBSM);
                                        //}
                                        //maxBSM++;
                                    }
                                    pUpdateCursor.UpdateFeature(pFeature);
                                }
                            }
                            else
                            {
                                while ((pFeature = pUpdateCursor.NextFeature()) != null)
                                {
                                    pFeature.set_Value(pFeature.Fields.FindField("BSM"), cdm + maxBSM++.ToString().PadLeft(8, '0'));
                                    //if (maxBSM.ToString().Length < 8)
                                    //{
                                    //    string bsm = txtXian.Text.Trim() + cdm + maxBSM.ToString().PadLeft(8, '0');
                                    //    pFeature.set_Value(pFeature.Fields.FindField("BSM"), bsm);

                                    //}
                                    //else
                                    //{
                                    //    pFeature.set_Value(pFeature.Fields.FindField("BSM"), maxBSM);
                                    //}
                                    //maxBSM++;

                                    //pFeature.set_Value(pFC.FindField("BSM"), maxBSM++);
                                    pUpdateCursor.UpdateFeature(pFeature);
                                }
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
