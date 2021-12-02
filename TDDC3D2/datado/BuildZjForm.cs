using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using RCIS.GISCommon;
using RCIS.Utility;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;

using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
namespace TDDC3D.datado
{
    public partial class BuildZjForm : Form
    {
        public BuildZjForm()
        {
            InitializeComponent();
        }

        public IWorkspace currWs = null;
        public IMap currMap = null;


        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        Dictionary<string, string> dicYsdmCdm = new Dictionary<string, string>();
        Dictionary<string, string> dicZjYsdmAliasname = new Dictionary<string, string>();
        Dictionary<string, string> dicClassNameYsdm = new Dictionary<string, string>();

        private void fillDictinary()
        {
            dicYsdmCdm.Clear();
            dicZjYsdmAliasname.Clear();

            string sql = "select * from SYS_YSDM    ";
            DataTable dt = RCIS.Database.LS_SetupMDBHelper.GetDataTable(sql, "tmp");
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (DataRow dr in dt.Rows)
            {
                dicYsdmCdm.Add(dr["YSDM"].ToString().Trim().ToUpper(), dr["LayerDM"].ToString().Trim());
                dicZjYsdmAliasname.Add(dr["YSDM"].ToString().ToUpper(), dr["ALIASNAME"].ToString().Trim());
                dicClassNameYsdm.Add(dr["CLASSNAME"].ToString().Trim(), dr["YSDM"].ToString());
            }
           

        }


        private void BuildZjForm_Load(object sender, EventArgs e)
        {

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

            fillDictinary();

            foreach (KeyValuePair<string, string> aItem in dicZjYsdmAliasname)
            {
                this.chkListYsdm.Items.Add(aItem.Key + "|" + aItem.Value,true);
            }

            this.cboSize.Text = "小五|9|3.1779";
            this.cboFont.Text = "宋体";
            ControlStyleHelper.LoadAllFont(this.cboFont);
            
        }

        #region 参数

        /// <summary>
        /// 字体
        /// </summary>
        public string  fontOpt
        {
            get
            {
                return this.cboFont.Text;
            }
        }

        public string colorOpt
        {
            get
            {
                return this.cbColor.Text;
            }
        }

        /// <summary>
        /// 磅数
        /// </summary>
        public int BS
        {
            get
            {
                int ibs = 24;
                string size = this.cboFont.Text;
                string[] sSize = size.Split('|');
                if (sSize.Length >2)
                {
                    int.TryParse(sSize[1], out ibs);
                    
                }
                return ibs;
            }
        }



        public double fontHeight
        {
            get
            {
                double  iHeight = 0;
                string size = this.cboSize.Text;
                string[] sSize = size.Split('|');
                if (sSize.Length > 2)
                {
                    double.TryParse(sSize[2], out iHeight);

                }
                return iHeight;
            }
        }

        public double fontWidth
        {
            get
            {
                double iHeight = 0;
                string size = this.cboSize.Text;
                string[] sSize = size.Split('|');
                if (sSize.Length > 2)
                {
                    double.TryParse(sSize[2], out iHeight);

                }
                return iHeight;
            }
        }

        public double fontJG
        {
            get
            {
                double jg = 0;
                double.TryParse(this.spinJg.Text, out jg);  //间隔
                return jg;
            }
        }

#endregion 



        private void BuildDltbZJ(IFeatureLayer srcLayer, IFeatureClass zjClass, IGeometry fwGeo,string ysdm)
        {
            IIdentify pIdentify = srcLayer as IIdentify;
            IArray pArray = null;
            if (fwGeo != null)
            {
                pArray = pIdentify.Identify(fwGeo);
            }
            else
            {
                pArray = pIdentify.Identify((srcLayer.FeatureClass as IGeoDataset).Extent);
            }
            if (pArray == null)
                return;
           
            IWorkspaceEdit pWSEdit = this.currWs as IWorkspaceEdit;
            pWSEdit.StartEditing(false);
            pWSEdit.StartEditOperation();

            IFeatureCursor pFCursor = zjClass.Insert(true);

            try
            {


                for (int i = 0; i < pArray.Count; i++)
                {
                    IFeatureIdentifyObj obj = pArray.get_Element(i) as IFeatureIdentifyObj;
                    IRowIdentifyObject aRow = obj as IRowIdentifyObject;
                    IFeature aFeature = aRow.Row as IFeature;

                    //获取 属性作为内容
                    string sTBBH = FeatureHelper.GetFeatureStringValue(aFeature, "TBBH");
                    string sQsxz = FeatureHelper.GetFeatureStringValue(aFeature, "QSXZ");
                    string sDL = FeatureHelper.GetFeatureStringValue(aFeature, "DLBM");

                    if (sQsxz.StartsWith("1") || sQsxz.StartsWith("2"))
                    {
                        sQsxz = "G";
                    }
                    else
                    {
                        sQsxz = "";
                    }
                    string zjnr = sTBBH + "/" + sDL;

                    IPoint pt = (aFeature.Shape as IArea).Centroid;
                    double x = pt.X;
                    double y = pt.Y;
                    
                    IFeatureBuffer Buffer = zjClass.CreateFeatureBuffer();
                    Buffer.Shape = pt;
                    FeatureHelper.SetFeatureBufferValue(Buffer, "YSDM", ysdm);
                    FeatureHelper.SetFeatureBufferValue(Buffer, "ZJNR", zjnr);
                    FeatureHelper.SetFeatureBufferValue(Buffer, "ZT", fontOpt);
                    FeatureHelper.SetFeatureBufferValue(Buffer, "YS", colorOpt);
                    FeatureHelper.SetFeatureBufferValue(Buffer, "BS", BS);
                    FeatureHelper.SetFeatureBufferValue(Buffer, "KD", this.fontWidth);
                    FeatureHelper.SetFeatureBufferValue(Buffer, "GD", this.fontHeight);
                    FeatureHelper.SetFeatureBufferValue(Buffer, "JG", this.fontJG);
                    FeatureHelper.SetFeatureBufferValue(Buffer, "ZJDZXJXZB", x);
                    FeatureHelper.SetFeatureBufferValue(Buffer, "ZJDZXJYZB", y);
                    FeatureHelper.SetFeatureBufferValue(Buffer, "ZJFX", 0);

                    pFCursor.InsertFeature(Buffer);//插入一条新的
                }
                pFCursor.Flush();
                pWSEdit.StopEditOperation();
                pWSEdit.StopEditing(true);
            }
            catch (Exception ex)
            {
                pWSEdit.AbortEditOperation();
                pWSEdit.StopEditing(true);
            }
            finally
            {
                OtherHelper.ReleaseComObject(pFCursor);
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }


        }


        /// <summary>
        /// 对该范围的 数据生成注记，填入到zj层
        /// </summary>
        /// <param name="srcClass"></param>
        /// <param name="fwGeo"></param>
        private void BuildXzqZJ(IFeatureLayer srcLayer, IFeatureClass zjClass, IGeometry  fwGeo,string fldName,string ysdm)
        {
            IIdentify pIdentify = srcLayer as IIdentify;
            IArray pArray=null;
            if (fwGeo != null)
            {
                pArray = pIdentify.Identify(fwGeo);
            }
            else
            {
                pArray = pIdentify.Identify((srcLayer.FeatureClass as IGeoDataset).Extent);
            }
            if (pArray == null)
                return;
            


            IWorkspaceEdit pWSEdit = this.currWs as IWorkspaceEdit;
            pWSEdit.StartEditing(false);
            pWSEdit.StartEditOperation();

            IFeatureCursor pFCursor = zjClass.Insert(true);

            try
            {


                for (int i = 0; i < pArray.Count; i++)
                {
                    IFeatureIdentifyObj obj = pArray.get_Element(i) as IFeatureIdentifyObj;
                    IRowIdentifyObject aRow = obj as IRowIdentifyObject;
                    IFeature aFeature = aRow.Row as IFeature;

                    //获取 属性作为内容
                    string zjnr = FeatureHelper.GetFeatureStringValue(aFeature, fldName);
                    IPoint pt = (aFeature.Shape as IArea).Centroid;
                    double x = pt.X;
                    double y = pt.Y;

                    IFeatureBuffer Buffer = zjClass.CreateFeatureBuffer();
                    Buffer.Shape = pt;
                    FeatureHelper.SetFeatureBufferValue( Buffer, "YSDM", ysdm);
                    FeatureHelper.SetFeatureBufferValue(Buffer,"ZJNR", zjnr);
                    FeatureHelper.SetFeatureBufferValue(Buffer, "ZT", fontOpt);
                    FeatureHelper.SetFeatureBufferValue(Buffer, "YS", colorOpt);
                    FeatureHelper.SetFeatureBufferValue(Buffer, "BS", BS);
                    FeatureHelper.SetFeatureBufferValue(Buffer, "KD", this.fontWidth);
                    FeatureHelper.SetFeatureBufferValue(Buffer, "GD", this.fontHeight);
                    FeatureHelper.SetFeatureBufferValue(Buffer, "JG", this.fontJG);
                    FeatureHelper.SetFeatureBufferValue(Buffer, "ZJDZXJXZB", x);
                    FeatureHelper.SetFeatureBufferValue(Buffer, "ZJDZXJYZB", y);
                    FeatureHelper.SetFeatureBufferValue(Buffer, "ZJFX", 0);

                    pFCursor.InsertFeature(Buffer);//插入一条新的
                }
                pFCursor.Flush();
                pWSEdit.StopEditOperation();
                pWSEdit.StopEditing(true);
            }
            catch (Exception ex)
            {
                pWSEdit.AbortEditOperation();
                pWSEdit.StopEditing(true);
            }
            finally
            {
                OtherHelper.ReleaseComObject(pFCursor);
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            

        }

        private void buildBSM(IFeatureClass aClass, string xiandm,string ysdm )
        {
            string cdm = dicYsdmCdm[ysdm];
            
            IWorkspaceEdit pWSEdit = this.currWs as IWorkspaceEdit;
            pWSEdit.StartEditing(false);
            pWSEdit.StartEditOperation();

            IQueryFilter pQF = new QueryFilterClass();
            pQF.WhereClause = "YSDM='" + ysdm + "'";
            IFeatureCursor updateCursor = aClass.Update(pQF, false);

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


                pWSEdit.StopEditOperation();
                pWSEdit.StopEditing(true);

            }
            catch (Exception ex)
            {
                pWSEdit.StartEditing(false);
                pWSEdit.StartEditOperation();
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(updateCursor);
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
           
           
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (txtXian.Text.Trim().Length != 6)
            {
                MessageBox.Show("县代码不符合要求！");
                return;
            }
            string xianDm=this.txtXian.Text.Trim();

            IFeatureLayer xzqLayer = LayerHelper.QueryLayerByModelName(this.currMap, "XZQ");
            IFeatureLayer dltbLayer = LayerHelper.QueryLayerByModelName(this.currMap, "DLTB");
            IFeatureLayer cjdcqLayer=LayerHelper.QueryLayerByModelName(this.currMap,"CJDCQ");
            IFeatureClass zjClass = null;
            
            try
            {
                zjClass=(this.currWs as IFeatureWorkspace).OpenFeatureClass("ZJ");
               
            }
            catch 
            {
            }

            if (dltbLayer == null || xzqLayer == null || zjClass==null)
            {
                MessageBox.Show("请先加在必备图层！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("正在生层内容，请稍等...","请等待");
            wait.Show();
            try
            {

                string xzqzjYsdm = dicClassNameYsdm["XZQZJ"];
                //行政区注记
                BuildXzqZJ(xzqLayer, zjClass, null, "XZQMC", xzqzjYsdm);
                wait.SetCaption("XZQZJ生成完成...");
                IIdentify pIdentify = xzqLayer as IIdentify;
                IArray arrXZQ = pIdentify.Identify((xzqLayer.FeatureClass as IGeoDataset).Extent);
                if (arrXZQ != null)
                {
                    for (int i = 0; i < arrXZQ.Count; i++)
                    {
                        IFeatureIdentifyObj obj = arrXZQ.get_Element(i) as IFeatureIdentifyObj;
                        IRowIdentifyObject aRow = obj as IRowIdentifyObject;
                        IFeature aFeature = aRow.Row as IFeature;

                        string xzqdm = FeatureHelper.GetFeatureStringValue(aFeature, "XZQDM");

                        string dltbzjYsdm = dicClassNameYsdm["DLTBZJ"];

                        this.BuildDltbZJ(dltbLayer, zjClass, aFeature.Shape,dltbzjYsdm);
                        wait.SetCaption(xzqdm + "包含的地类图斑注记生成完成！");
                    }
                }

                this.BuildXzqZJ(cjdcqLayer, zjClass, null, "ZLDWMC",dicClassNameYsdm["CJDCQZJ"]);

                wait.SetCaption("正在生成BSM");
                //开始重计算bsm
                buildBSM(zjClass, xianDm, dicClassNameYsdm["XZQZJ"]);
                buildBSM(zjClass, xianDm, dicClassNameYsdm["DLTBZJ"]);

                wait.Close();
                MessageBox.Show("生成完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                wait.Close();
                MessageBox.Show(ex.Message);
            }

           


        }
    }
}
