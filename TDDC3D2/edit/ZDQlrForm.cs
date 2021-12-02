using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RCIS.Database;
using RCIS.Utility;
using RCIS.GISCommon;
using System.Collections;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;

namespace TDDC3D.edit
{
    public partial class ZDQlrForm : Form
    {
        public ZDQlrForm()
        {
            InitializeComponent();
        }

        public IMap currMap = null;
        public IWorkspace currWS = null;

        private void LoadSelZds()
        {
            this.listAllZD.Items.Clear();
            ArrayList ar = LayerHelper.GetSelectedFeature(this.currMap, pZdLyr as IGeoFeatureLayer, ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon);
            foreach (IFeature aFea in ar)
            {
                string zddm = FeatureHelper.GetFeatureStringValue(aFea, "ZDDM");
                listAllZD.Items.Add(FeatureHelper.GetFeatureStringValue(aFea, "BSM") + "|" + zddm);
            }
        }

        private void LoadAllZd()
        {
            IIdentify identify = this.pZdLyr as IIdentify;
            IArray pArray = identify.Identify((this.pZdLyr.FeatureClass as IGeoDataset).Extent);
            if (pArray == null)
                return;
            this.listAllZD.Items.Clear();
            for (int i = 0; i < pArray.Count; i++)
            {
                IFeatureIdentifyObj obj = pArray.get_Element(i) as IFeatureIdentifyObj;
                IRowIdentifyObject aRow = obj as IRowIdentifyObject;
                IFeature aZD = aRow.Row as IFeature;
                listAllZD.Items.Add( FeatureHelper.GetFeatureStringValue(aZD,"BSM")+"|"+FeatureHelper.GetFeatureStringValue(aZD,"ZDDM"));
            }
        }

        IFeatureLayer pZdLyr = null;
        ITable zd_qlrTable = null;

        DataTable m_dataTable = null;
        private void ZDQlrForm_Load(object sender, EventArgs e)
        {
            
            pZdLyr = LayerHelper.QueryLayerByModelName(this.currMap, "ZD");
            if (pZdLyr == null)
            {
                MessageBox.Show("找不到宗地层！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                zd_qlrTable = (this.currWS as IFeatureWorkspace).OpenTable("ZD_QLR");
            }
            catch { }
            if (zd_qlrTable == null)
            {
                MessageBox.Show("找不到宗地权利人表！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;

            }
            this.DLRSFZJLX.Properties.Items.Clear();
            DataTable dt = LS_SetupMDBHelper.GetDataTable("select DM,MC from DIC_38证件类型代码表 ","tmp");
            foreach (DataRow dr in dt.Rows)
            {
                this.DLRSFZJLX.Properties.Items.Add(dr["DM"].ToString() + "|" + dr["MC"].ToString());
            }

            //加载宗地代码
            LoadSelZds();

            #region 构造字段
            this.m_dataTable = new DataTable();
            for (int fi = 0; fi < zd_qlrTable.Fields.FieldCount; fi++)
            {
                IField curFld =  zd_qlrTable.Fields.get_Field(fi);
                string fldName = curFld.Name.ToUpper();
                DataColumn dc = new DataColumn(curFld.Name);
                if (curFld.Type == esriFieldType.esriFieldTypeInteger
                    || curFld.Type == esriFieldType.esriFieldTypeSmallInteger
                    || curFld.Type == esriFieldType.esriFieldTypeOID)
                {
                    dc.DataType = typeof(Int32);
                }
                else if (curFld.Type == esriFieldType.esriFieldTypeSingle
                    || curFld.Type == esriFieldType.esriFieldTypeDouble)
                {
                    dc.DataType = typeof(Double);
                }
                else dc.DataType = typeof(string);
                this.m_dataTable.Columns.Add(dc);
            }
            #endregion

        }

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.radioGroup1.SelectedIndex == 0)
            {
                LoadSelZds();
            }
            else if (this.radioGroup1.SelectedIndex == 1)
            {
                LoadAllZd();
            }
        }

        private void RefreshQLR(string zddm)
        {
            IQueryFilter pQF = new QueryFilterClass();
            pQF.WhereClause = "ZDDM='" + ZDDM.Text + "'";
            ICursor pCusor = zd_qlrTable.Search(pQF, false);
            IRow pRow = null;
            this.m_dataTable.Rows.Clear();
            try
            {
                while ((pRow = pCusor.NextRow()) != null)
                {

                    DataRow curRow = this.m_dataTable.NewRow();
                    for (int fi = 0; fi < zd_qlrTable.Fields.FieldCount; fi++)
                    {
                        IField curFld = zd_qlrTable.Fields.get_Field(fi);
                        object oValue = pRow.get_Value(fi);
                        if (curFld.Type == esriFieldType.esriFieldTypeSingle || curFld.Type == esriFieldType.esriFieldTypeDouble)
                        {
                            double dVal = 0.0;
                            double.TryParse(oValue.ToString(), out dVal);
                            curRow[fi] = dVal;

                        }
                        else
                        {
                            curRow[fi] = oValue;
                        }
                    }

                    this.m_dataTable.Rows.Add(curRow);
                    this.gridControl1.DataSource = this.m_dataTable;

                }
            }
            catch
            {
            }
            finally
            {

                RCIS.Utility.OtherHelper.ReleaseComObject(pCusor);
            }
        }

        private void listAllZD_DoubleClick(object sender, EventArgs e)
        {
            //显示权利人
            string txt = this.listAllZD.SelectedItem.ToString();
            if (txt == "") return;
            string zdddm = OtherHelper.GetRightName(txt);
            string zdbsm = OtherHelper.GetLeftName(txt);
            this.ZDDM.Text = zdddm;
            this.ZDBSM.Text = zdbsm;
            if (zdddm.Length>12)
            {
                this.QSDWDM.Text = ZDDM.Text.Substring(0, 12)+"0000000";
            }
            RefreshQLR(zdddm);
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (this.gridView1.SelectedRowsCount == 0)
                return;
            string oid = this.gridView1.GetRowCellValue(this.gridView1.FocusedRowHandle, "OBJECTID").ToString().Trim();
            if (oid == "") return;
            this.gridView1.DeleteSelectedRows();
            IWorkspaceEdit pWSEdt = this.currWS as IWorkspaceEdit;
            pWSEdt.StartEditOperation();
            try
            {
                IQueryFilter pQF = new QueryFilterClass();
                pQF.WhereClause = "OBJECTID=" + oid + "";
                zd_qlrTable.DeleteSearchedRows(pQF);
                pWSEdt.StopEditOperation();
            }
            catch(Exception x) {
                pWSEdt.AbortEditOperation();
            }
            
            

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //添加一行
            string txt = this.listAllZD.SelectedItem.ToString();
            if (txt == null || txt == "") return;
            string zdddm = OtherHelper.GetRightName(txt);
            string zdbsm = OtherHelper.GetLeftName(txt);

            IWorkspaceEdit pWSEdt = this.currWS as IWorkspaceEdit;
            pWSEdt.StartEditOperation();
            try
            {
                IRow newRow = zd_qlrTable.CreateRow();
                for (int i = 0; i < newRow.Fields.FieldCount; i++)
                {
                    IField pFld = newRow.Fields.get_Field(i);
                    string fldName = pFld.Name.ToString().Trim().ToUpper();
                    switch (fldName)
                    {
                        case "ZDDM":
                            newRow.set_Value(i, zdddm);
                            break;
                        case "ZDBSM":
                            newRow.set_Value(i, zdbsm);
                            break;
                        case "QSDWDM":
                            newRow.set_Value(i, QSDWDM.Text);
                            break;
                        case "QLRMC":
                            newRow.set_Value(i, QLRMC.Text);
                            break;
                        case "DLRXM":
                            newRow.set_Value(i, DLRXM.Text);
                            break;
                        case "DLRSFZJLX":
                            newRow.set_Value(i, OtherHelper.GetLeftName(DLRSFZJLX.Text));
                            break;
                        case "DLRSFZJH":
                            newRow.set_Value(i, DLRSFZJH.Text);
                            break;
                        case "DLRSFZMS":
                            newRow.set_Value(i, DLRSFZMS.Text);
                            break;
                        case "DLRDH":
                            newRow.set_Value(i, this.DLRDH.Text);
                            break;
                        case "DLRBCQZH":
                            newRow.set_Value(i, this.DLRBCQZH.Text);
                            break;
                        case "BZ":
                            newRow.set_Value(i, this.BZ.Text);
                            break;
                    }
                }
                newRow.Store();
                pWSEdt.StopEditOperation();

                RefreshQLR(zdddm);

            }
            catch (Exception x)
            {
                pWSEdt.AbortEditOperation();
            }
        }
    }
}
