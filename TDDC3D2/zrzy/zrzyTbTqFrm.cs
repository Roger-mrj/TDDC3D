using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.ADF;
using RCIS.GISCommon;
using RCIS.Utility;
namespace TDDC3D.zrzy
{
    public partial class zrzyTbTqFrm : Form
    {
        public zrzyTbTqFrm()
        {
            InitializeComponent();
        }
        public IMap currMap = null;
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void zrzyTbTqFrm_Load(object sender, EventArgs e)
        {
            LayerHelper.LoadLayer2Combox(this.cmbSrcLayer, this.currMap, esriGeometryType.esriGeometryPolygon);
            LayerHelper.LoadLayer2Combox(this.cmbSLLayer, this.currMap, esriGeometryType.esriGeometryPolygon);
            LayerHelper.LoadLayer2Combox(this.cmbSenLLayer, this.currMap, esriGeometryType.esriGeometryPolygon);
            LayerHelper.LoadLayer2Combox(this.cmbShLLayer, this.currMap, esriGeometryType.esriGeometryPolygon);
            LayerHelper.LoadLayer2Combox(this.cmbCyLayer, this.currMap, esriGeometryType.esriGeometryPolygon);
            LayerHelper.LoadLayer2Combox(this.cmbHdLayer, this.currMap, esriGeometryType.esriGeometryPolygon);
            LayerHelper.LoadLayer2Combox(this.cmbTTLayer, this.currMap, esriGeometryType.esriGeometryPolygon);

            ControlStyleHelper.FindComboxItem(this.cmbSrcLayer, "DLTB",true);
            ControlStyleHelper.FindComboxItem(this.cmbSLLayer, "SLZY",true);
            ControlStyleHelper.FindComboxItem(this.cmbSenLLayer, "SENLZY", true);
            ControlStyleHelper.FindComboxItem(this.cmbShLLayer, "SHLZY", true);
            ControlStyleHelper.FindComboxItem(this.cmbCyLayer, "CYZY", true);
            ControlStyleHelper.FindComboxItem(this.cmbHdLayer, "HCDZY", true);
            ControlStyleHelper.FindComboxItem(this.cmbTTLayer, "TTZY", true);
            

        }


        private void TqSlzyTb(string zyclassName)
        {
            string ysdm = "";
            string mjField = "";
            string where = "";
            switch (zyclassName)
            {
                case "SLZY":
                    ysdm = "2007010100";
                    mjField = "SLMJ";
                    where = "DLBM in ('1101','1102','1103','1104')";
                    break;
                case "SENLZY":
                    ysdm = "2007010200";
                    mjField = "SLMJ";
                    where = "DLBM in ('0301','0302','0303','0304','0305','0306','0307')";
                    break;
                case "CYZY":
                    mjField = "CYMJ";
                    ysdm = "2007010400";
                    where = "DLBM in ('0401','0402','0403','0404')";
                    break;
                case "HCDZY":
                    mjField = "HDMJ";
                    ysdm = "2007010500";
                    where = "DLBM in ('1204''1205','1206','1207')";
                    break;
                case "TTZY":
                    mjField = "TTMJ";
                    ysdm = "2007010600";
                    where = "DLBM in ('1108','1105','1106')";
                    break;
            }

            IFeatureCursor insertCursor = null;
            IFeatureCursor queryCursor = null;
            IQueryFilter pQF = null;
            IFeatureBuffer pTargetBuffer = null;
            try
            {

                IFeatureClass slzyFC = (RCIS.Global.GlobalEditObject.GlobalWorkspace as IFeatureWorkspace).OpenFeatureClass(zyclassName);
                insertCursor = slzyFC.Insert(true);

                if (this.radioGFwOpt.SelectedIndex == 0)
                {
                    ISpatialFilter pSF = new SpatialFilterClass();
                    pSF.Geometry = (this.currMap as IActiveView).Extent;
                    pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                    pSF.WhereClause = where;
                    pQF = pSF as IQueryFilter;
                }
                else if (this.radioGFwOpt.SelectedIndex == 1)
                {
                    pQF = new QueryFilterClass();
                    pQF.WhereClause = where;
                }
                int nums = dltbClass.FeatureCount(pQF);
                queryCursor = dltbClass.Search(pQF, false);
                pTargetBuffer = slzyFC.CreateFeatureBuffer();

                IFeature aFeature = null;
                int iCount = 0;
                while ((aFeature = queryCursor.NextFeature()) != null)
                {
                    IGeometry srcShp = aFeature.ShapeCopy;
                    pTargetBuffer.Shape = srcShp;
                    FeatureHelper.SetFeatureBufferValue(pTargetBuffer, "YSDM", ysdm);                   
                    FeatureHelper.SetFeatureBufferValue(pTargetBuffer, mjField, FeatureHelper.GetFeatureDoubleValue(aFeature, "TBDLMJ"));
                    FeatureHelper.SetFeatureBufferValue(pTargetBuffer, "GLTBBSM", FeatureHelper.GetFeatureStringValue(aFeature, "BSM"));
                    insertCursor.InsertFeature(pTargetBuffer);

                    iCount++;
                    if (iCount % 200 == 0)
                    {
                        insertCursor.Flush();
                        lblstatus.Text = "当前已经提取到" + zyclassName + iCount + "个要素...";
                        Application.DoEvents();
                    }

                }
                insertCursor.Flush();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                OtherHelper.ReleaseComObject(pQF);
                OtherHelper.ReleaseComObject(pTargetBuffer);
                OtherHelper.ReleaseComObject(insertCursor);
                OtherHelper.ReleaseComObject(queryCursor);
                        
            }

        }
        IFeatureClass dltbClass = null;
        private void btnOk_Click(object sender, EventArgs e)
        {
            string dltbClassName = OtherHelper.GetLeftName(this.cmbSrcLayer.Text);
            try
            {
                dltbClass = (RCIS.Global.GlobalEditObject.GlobalWorkspace as IFeatureWorkspace).OpenFeatureClass(dltbClassName);
            }
            catch (Exception ex)
            {
            }
            if (dltbClass == null)
            {
                MessageBox.Show("获取地类图班图层出错！");
                return;
            }

            if (this.cmbSrcLayer.Text.Trim() == "") return;
            RCIS.Global.GlobalEditObject.CurrentEngineEditor.StartOperation();
            try
            {
                
                if (this.chkSLZY.Checked)
                {                    
                    this.TqSlzyTb(OtherHelper.GetLeftName(this.cmbSLLayer.Text));
                }
                if (this.chkSenLZY.Checked)
                {                    
                    this.TqSlzyTb(OtherHelper.GetLeftName(this.cmbSenLLayer.Text));
                }
                if (this.chkCyzy.Checked)
                {                    
                    this.TqSlzyTb(OtherHelper.GetLeftName(this.cmbCyLayer.Text));
                }
                if (this.chkHDzy.Checked)
                {                    
                    this.TqSlzyTb(OtherHelper.GetLeftName(this.cmbHdLayer.Text));
                }
                if (this.chkTTzy.Checked)
                {                    
                    this.TqSlzyTb(OtherHelper.GetLeftName(this.cmbTTLayer.Text));
                }
                RCIS.Global.GlobalEditObject.CurrentEngineEditor.StopOperation("tqzrzy");
                this.lblstatus.Text = "";
                MessageBox.Show("提取完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                RCIS.Global.GlobalEditObject.CurrentEngineEditor.AbortOperation();
                this.lblstatus.Text = "";
                MessageBox.Show(ex.Message);
            }

        }

        private void chkSLZY_CheckedChanged(object sender, EventArgs e)
        {
            this.cmbSLLayer.Enabled = this.chkSLZY.Checked;
        }

        private void chkSenLZY_CheckedChanged(object sender, EventArgs e)
        {
            this.cmbSenLLayer.Enabled = this.chkSenLZY.Checked;
        }

        private void chkShLZY_CheckedChanged(object sender, EventArgs e)
        {
            this.cmbShLLayer.Enabled = this.chkShLZY.Checked;
        }

        private void chkCyzy_CheckedChanged(object sender, EventArgs e)
        {
            this.cmbCyLayer.Enabled = this.chkCyzy.Checked;
        }

        private void chkHDzy_CheckedChanged(object sender, EventArgs e)
        {
            this.cmbHdLayer.Enabled = this.chkHDzy.Checked;
        }

        private void chkTTzy_CheckedChanged(object sender, EventArgs e)
        {
            this.cmbTTLayer.Enabled = this.chkTTzy.Checked;
        }
    }
}
