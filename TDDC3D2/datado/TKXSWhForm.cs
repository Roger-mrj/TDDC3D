using System;
using System.Windows.Forms;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;

using RCIS.GISCommon;
using RCIS.Utility;

namespace TDDC3D.datado
{
    public partial class TKXSWhForm : Form
    {
        public TKXSWhForm()
        {
            InitializeComponent();
        }

        public IMap currMap = null;
        public IWorkspace currWS = null;
        IFeatureLayer dltbLayer = null;
        IFeatureLayer pdtLayer = null;


        public double d2TT
        {
            get {
                
                return (double)spinTT2.Value;
            }
        }
        public double d3TT
        {
            get
            {
                return (double)spinTT3.Value;
            }
        }

        public double d4TT
        {
            get
            {
                return (double)spinTT4.Value;
            }
        }

        public double d5TT
        {
            get
            {
                return (double)spinTT5.Value;
            }
        }

        public double d2PD
        {
            get
            {
                
                return (double) spinPd2.Value;
            }
        }

        public double d3PD
        {
            get
            {
                return (double)spinPd3.Value;
            }
        }
        public double d4PD
        {
            get
            {
                return (double)spinPd4.Value;
            }
        }

        public double d5PD
        {
            get
            {
               
                return (double)spinPd5.Value;
            }
        }
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        INIHelper ini = new INIHelper(RCIS.Global.AppParameters.ConfPath + "\\Setup.ini");
        

        private void TKXSWhForm_Load(object sender, EventArgs e)
        {
            //LayerHelper.LoadLayer2Combox(this.cmbPDTLayer, this.currMap,ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon);
            LayerHelper.LoadLayer2Combox(this.cmbDLTBLayer, this.currMap, ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon);
            int dltbIdx = 0; int pdtidx = 0;
            //for (int i = 0; i < this.cmbPDTLayer.Properties.Items.Count; i++)
            //{
            //    string str = this.cmbPDTLayer.Properties.Items[i].ToString();
            //    if (str.ToUpper().Trim().StartsWith("PDT"))
            //    {
            //        pdtidx = i;
            //        break;
            //    }
            //}
            //this.cmbPDTLayer.SelectedIndex = pdtidx;

            for (int i = 0; i < this.cmbDLTBLayer.Properties.Items.Count; i++)
            {
                string str = this.cmbDLTBLayer.Properties.Items[i].ToString();
                if (str.ToUpper().Trim().StartsWith("DLTB"))
                {
                    dltbIdx = i;
                    break;
                }
            }
            this.cmbDLTBLayer.SelectedIndex = dltbIdx;



            this.ReadIniTk();

        }

        private void SaveIniTK()
        {
            ini.IniWriteValue("KCXS", "JB2TT", this.d2TT.ToString());
            ini.IniWriteValue("KCXS", "JB2PD", this.d2PD.ToString());
            ini.IniWriteValue("KCXS", "JB3TT", this.d3TT.ToString());
            ini.IniWriteValue("KCXS", "JB3PD", this.d3PD.ToString());
            ini.IniWriteValue("KCXS", "JB4TT", this.d4TT.ToString());
            ini.IniWriteValue("KCXS", "JB4PD", this.d4PD.ToString());
            ini.IniWriteValue("KCXS", "JB5TT", this.d5TT.ToString());
            ini.IniWriteValue("KCXS", "JB5PD", this.d5PD.ToString());

        }
        private void ReadIniTk()
        {
            string tk2tt = ini.IniReadValue("KCXS", "JB2TT").Trim();
            if (tk2tt != "")
            {
                this.spinTT2.Value = decimal.Parse(tk2tt);
            }

            string tk2Pd = ini.IniReadValue("KCXS", "JB2PD").Trim();
            if (tk2Pd != "")
            {
                this.spinPd2.Value = decimal.Parse(tk2Pd);
            }


            string tk3tt = ini.IniReadValue("KCXS", "JB3TT").Trim();
            if (tk3tt != "")
            {
                this.spinTT3.Value = decimal.Parse(tk3tt);
            }

            string tk3Pd = ini.IniReadValue("KCXS", "JB3PD").Trim();
            if (tk3Pd != "")
            {
                this.spinPd3.Value = decimal.Parse( tk3Pd);
            }

            string tk4tt = ini.IniReadValue("KCXS", "JB4TT").Trim();
            if (tk4tt != "")
            {
                this.spinTT4.Value = decimal.Parse(tk4tt);
            }

            string tk4Pd = ini.IniReadValue("KCXS", "JB4PD").Trim();
            if (tk4Pd != "")
            {
                this.spinPd4.Value = decimal.Parse(tk4Pd);
            }

            string tk5tt = ini.IniReadValue("KCXS", "JB5TT").Trim();
            if (tk5tt != "")
            {
                this.spinTT5.Value = decimal.Parse(tk5tt);
            }

            string tk5Pd = ini.IniReadValue("KCXS", "JB5PD").Trim();
            if (tk5Pd != "")
            {
                this.spinPd5.Value =decimal.Parse( tk5Pd);
            }


        }

        //private void PDJBSetValue(IFeature aPdt, IFeatureClass dltbClass)
        //{
        //    string pdjb = FeatureHelper.GetFeatureStringValue(aPdt, "PDJB").Trim();
        //    SpatialFilter pSF = new SpatialFilterClass();
        //    pSF.Geometry = aPdt.ShapeCopy;
        //    pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects; //相交
        //    IQueryFilter pQF = pSF as IQueryFilter;
        //    pQF.WhereClause = "DLBM in ('0101','0102','0103') ";
        //    IFeatureCursor pCursor = dltbClass.Search(pQF, false);
        //    IFeature aDLTB = null;
        //    try
        //    {
        //        while ((aDLTB = pCursor.NextFeature()) != null)
        //        {
        //            FeatureHelper.SetFeatureValue(aDLTB, "GDPDJB", pdjb); //坡度级别
                    
        //            aDLTB.Store();

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    finally
        //    {
        //        OtherHelper.ReleaseComObject(pSF);
        //        OtherHelper.ReleaseComObject(pCursor);
        //    }
        //}

        //private void DoAPDT(IFeature aPdt,IFeatureClass dltbClass)
        //{
        //    string pdjb = FeatureHelper.GetFeatureStringValue(aPdt, "PDJB").Trim();

        //    ISpatialFilter pSF = new SpatialFilterClass();
        //    pSF.Geometry = aPdt.ShapeCopy;
        //    pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects; //相交
        //    IQueryFilter pQF= pSF as IQueryFilter;
        //    pQF.WhereClause ="DLBM in ('0101','0102','0103') ";
        //    IFeatureCursor pCursor = dltbClass.Search(pQF, false);
        //    IFeature aDLTB = null;
        //    try
        //    {
        //        while ((aDLTB = pCursor.NextFeature()) != null)
        //        {
        //            FeatureHelper.SetFeatureValue(aDLTB, "GDPDJB", pdjb); //坡度级别
        //            string gdlx = FeatureHelper.GetFeatureStringValue(aDLTB, "GDLX").Trim();
        //            if (gdlx.Trim() == "")
        //            {
        //                //FeatureHelper.SetFeatureValue(aDLTB, "GDLX", "TT");
        //                gdlx = "TT";
        //            }
        //            FeatureHelper.SetFeatureValue(aDLTB, "KCDLBM", "1203"); //默认 田坎 地类
        //            double tkxs = 0;
        //            if (pdjb == "2")
        //            {
        //                if (gdlx == "TT")
        //                {
        //                    tkxs = this.d2TT;
        //                }
        //                else
        //                {
        //                    tkxs = this.d2PD;
        //                }
        //            }
        //            else if (pdjb == "3")
        //            {
        //                if (gdlx == "TT")
        //                {
        //                    tkxs = this.d3TT;
        //                }
        //                else
        //                {
        //                    tkxs = this.d3PD;
        //                }
        //            }
        //            else if (pdjb == "4")
        //            {
        //                if (gdlx == "TT")
        //                {
        //                    tkxs = this.d4TT;
        //                }
        //                else
        //                {
        //                    tkxs = this.d4PD;
        //                }
        //            }
        //            else if (pdjb == "5")
        //            {
        //                if (gdlx == "TT")
        //                {
        //                    tkxs = this.d5TT;
        //                }
        //                else
        //                {
        //                    tkxs = this.d5PD;
        //                }
        //            }
        //            FeatureHelper.SetFeatureValue(aDLTB, "KCXS", tkxs);
        //            aDLTB.Store();

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    finally
        //    {
        //        OtherHelper.ReleaseComObject(pSF);
        //        OtherHelper.ReleaseComObject(pCursor);
        //    }
        //}

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (this.cmbDLTBLayer.Text.Trim() == "") return;

            

            //IFeatureLayer dltbLayer = LayerHelper.QueryLayerByModelName(this.currMap, OtherHelper.GetLeftName( this.cmbDLTBLayer.Text.Trim()));
            //if (dltbLayer == null) return;
            //IFeatureClass dltbClass = dltbLayer.FeatureClass;
            //string className = (dltbClass as IDataset).Name;
            string name = RCIS.Utility.OtherHelper.GetLeftName(cmbDLTBLayer.Text.Trim());
            try
            {
               
                string sql = "update "+name+" set KCXS=" + this.d2PD + " where GDPDJB='2' and GDLX='PD' ";
                this.currWS.ExecuteSQL(sql);
                sql = "update " + name + " set KCXS=" + this.d2TT + " where GDPDJB='2' and GDLX='TT' ";
                this.currWS.ExecuteSQL(sql);
                sql = "update " + name + " set KCXS=" + this.d3PD + " where GDPDJB='3' and GDLX='PD' ";
                this.currWS.ExecuteSQL(sql);
                sql = "update " + name + " set KCXS=" + this.d3TT + " where GDPDJB='3' and GDLX='TT' ";
                this.currWS.ExecuteSQL(sql);
                sql = "update " + name + " set KCXS=" + this.d4PD + " where GDPDJB='4' and GDLX='PD' ";
                this.currWS.ExecuteSQL(sql);
                sql = "update " + name + " set KCXS=" + this.d4TT + " where GDPDJB='4' and GDLX='TT' ";
                this.currWS.ExecuteSQL(sql);
                sql = "update " + name + " set KCXS=" + this.d5PD + " where GDPDJB='5' and GDLX='PD' ";
                this.currWS.ExecuteSQL(sql);
                sql = "update " + name + " set KCXS=" + this.d5TT + " where GDPDJB='5' and GDLX='TT' ";
                this.currWS.ExecuteSQL(sql);

                MessageBox.Show("处理完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message);
            }
        }

        private void groupControl2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void TKXSWhForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            SaveIniTK();
        }


        

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            //if (this.cmbDLTBLayer.Text.Trim() == "") return;
            //if (this.cmbPDTLayer.Text.Trim() == "") return;

            //IFeatureLayer dltbLayer = LayerHelper.QueryLayerByModelName(this.currMap, OtherHelper.GetLeftName(this.cmbDLTBLayer.Text.Trim()));
            //IFeatureLayer pdtLayer = LayerHelper.QueryLayerByModelName(this.currMap, OtherHelper.GetLeftName(this.cmbPDTLayer.Text.Trim()));
            //if (dltbLayer == null || pdtLayer == null) return;

            //IFeatureClass dltbClass = dltbLayer.FeatureClass;
            //IFeatureClass pdtClass = pdtLayer.FeatureClass;

            //IQueryFilter pQF = new QueryFilterClass();
            //pQF.WhereClause="DLBM in ('0101','0102','0103') ";
            //if (dltbClass.FeatureCount(pQF) == 0)
            //{
            //    MessageBox.Show("地类图斑没有耕地!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return;
            //}


            //if (pdtClass.FeatureCount(null) == 0)
            //{
            //    MessageBox.Show("坡度图没信息!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return;
            //}

            //IIdentify pdtIds = dltbLayer as IIdentify;
            //IArray arDltb = pdtIds.Identify((pdtClass as IGeoDataset).Extent);
            //if (arDltb == null)
            //{
               
            //    return;
            //}

            //DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("正在处理", "请等待...");
            //wait.Show();

            //IWorkspaceEdit pWSEdit = this.currWS as IWorkspaceEdit;
            //pWSEdit.StartEditing(false);
            //pWSEdit.StartEditOperation();
            //int icount = 0;

            //try
            //{
            //    IFeatureCursor pDltbCursor = dltbClass.Update(pQF, true);
            //    IFeature aDltb = null;
            //    try
            //    {
            //        while ((aDltb = pDltbCursor.NextFeature()) != null)
            //        {
            //            string gdpdjb = this.getPdjb(aDltb.Shape, pdtClass);
            //            if (gdpdjb.Trim() != "")
            //            {
            //                FeatureHelper.SetFeatureValue(aDltb, "GDPDJB", gdpdjb); //坡度级别
            //                pDltbCursor.UpdateFeature(aDltb);


            //                icount++;
            //                if (icount % 100 == 0)
            //                {
            //                    wait.SetCaption("已处理" + icount + "个...");
            //                }
                            
            //            }
                        
                        
            //        }

            //    }
            //    catch (Exception cex)
            //    {
            //    }
            //    finally
            //    {
            //        System.Runtime.InteropServices.Marshal.ReleaseComObject(pDltbCursor);
            //    }

            //    pWSEdit.StopEditOperation();
            //    pWSEdit.StopEditing(true);

            //    wait.Close();
            //    lblstatus.Text = "";
            //    MessageBox.Show("处理完毕,共处理"+icount+"个！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
            //catch (Exception ex)
            //{
            //    wait.Close();
            //    pWSEdit.AbortEditOperation();
            //    pWSEdit.StopEditing(false);
            //    lblstatus.Text = "";
            //    MessageBox.Show(ex.Message);
            //}
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            if (this.cmbDLTBLayer.Text.Trim() == "") return;
            IFeatureLayer dltbLayer = LayerHelper.QueryLayerByModelName(this.currMap, OtherHelper.GetLeftName(this.cmbDLTBLayer.Text.Trim()));
            if (dltbLayer == null) return;
            IFeatureClass dltbClass = dltbLayer.FeatureClass;

            IFeatureCursor pFeaCursor = null;
            IWorkspaceEdit pWSEdit = this.currWS as IWorkspaceEdit;
            pWSEdit.StartEditing(true);
            pWSEdit.StartEditOperation();

            try
            {

                int icount = 0;
                IFeature aFea;
                pFeaCursor = dltbClass.Update(null, false);
                while ((aFea=pFeaCursor.NextFeature())!=null)
                {

                    double kcxs = FeatureHelper.GetFeatureDoubleValue(aFea, "KCXS");
                    if (kcxs > 0.0000001)
                    {
                        kcxs = MathHelper.Round(kcxs/100, 4);
                        FeatureHelper.SetFeatureValue(aFea, "KCXS", kcxs);
                        pFeaCursor.UpdateFeature(aFea);
                        icount++;
                    }                   

                }
                

                this.Cursor = Cursors.Default;
              
                pWSEdit.StopEditOperation();
                pWSEdit.StopEditing(true);

                MessageBox.Show("计算完毕,共更新"+icount+"条数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                pWSEdit.AbortEditOperation();
                pWSEdit.StopEditing(false);
              
                this.Cursor = Cursors.Default;
            }
            finally
            {
                //System.Runtime.InteropServices.Marshal.ReleaseComObject(pQF);               
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeaCursor);
            }


        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            try
            {
                string sql = "update DLTB set GDPDJB='' where DLBM not  in ('0101','0102','0103') ";
                this.currWS.ExecuteSQL(sql);
                MessageBox.Show("处理完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }

    
}
