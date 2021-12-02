using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;

namespace TDDC3D.gengxin
{
    public partial class FrmSpatialUpdate : Form
    {
        public IWorkspace currWS = null;

        public FrmSpatialUpdate()
        {
            InitializeComponent();
        }

        private void gridDestFields_Scroll(object sender, ScrollEventArgs e)
        {
            gridSrcFields.FirstDisplayedScrollingRowIndex = gridDestFields.FirstDisplayedScrollingRowIndex;
            gridSrcFields.HorizontalScrollingOffset = gridDestFields.HorizontalScrollingOffset;
        }

        private void gridSrcFields_Scroll(object sender, ScrollEventArgs e)
        {
            gridSrcFields.FirstDisplayedScrollingRowIndex = gridDestFields.FirstDisplayedScrollingRowIndex;
            gridSrcFields.HorizontalScrollingOffset = gridDestFields.HorizontalScrollingOffset;
        }

        private void FrmSpatialUpdate_Load(object sender, EventArgs e)
        {
            if (currWS != null)
            {
                cboSource.Properties.Items.Clear();
                cboTarget.Properties.Items.Clear();
                List<IFeatureClass> allFeatureClass = RCIS.GISCommon.DatabaseHelper.getAllFeatureClass(currWS);
                foreach (IFeatureClass feaClass in allFeatureClass)
                {
                    IDataset pDataset = feaClass as IDataset;
                    cboSource.Properties.Items.Add(pDataset.Name + "|" + feaClass.AliasName);
                    cboTarget.Properties.Items.Add(pDataset.Name + "|" + feaClass.AliasName);
                }
            }
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cboTarget_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(cboSource.Text) && !string.IsNullOrWhiteSpace(cboTarget.Text))
            {
                string sourClassName = RCIS.Utility.OtherHelper.GetLeftName(cboSource.Text);
                string tarClassName = RCIS.Utility.OtherHelper.GetLeftName(cboTarget.Text);
                IFeatureClass sourClass = null;
                IFeatureClass tarClass = null;
                try
                {
                    sourClass = (currWS as IFeatureWorkspace).OpenFeatureClass(sourClassName);
                    tarClass = (currWS as IFeatureWorkspace).OpenFeatureClass(tarClassName);
                }
                catch{}
                if (sourClass != null && tarClass != null)
                {
                    LoadTarFeatureClassField(tarClass);
                }
            }
        }

        private void LoadSourFeatureClassField(IFeatureClass sourFeatureClass)
        {
            ColumnSrcFields.Items.Clear();
            ColumnSrcFields.Items.Add("");
            ITable pTable = sourFeatureClass as ITable;
            for (int i = 0; i < pTable.Fields.FieldCount; i++)
            {
                IField pField = pTable.Fields.get_Field(i);
                if ((pField.Type != esriFieldType.esriFieldTypeBlob) &&
                     (pField.Type != esriFieldType.esriFieldTypeGeometry) &&
                    (pField.Type != esriFieldType.esriFieldTypeOID) &&
                    (pField.Type != esriFieldType.esriFieldTypeGlobalID) &&
                    (pField.Type != esriFieldType.esriFieldTypeGUID) &&
                    (pField.Type != esriFieldType.esriFieldTypeOID) &&
                    (pField.Type != esriFieldType.esriFieldTypeRaster) &&
                    (pField.Name.ToUpper() != "SHAPE_LENGTH") &&
                    (pField.Name.ToUpper() != "SHAPE_AREA"))
                {
                    ColumnSrcFields.Items.Add(pField.Name.ToString() + "|" + pField.Type.ToString());
                }
            }
        }

        private void LoadTarFeatureClassField(IFeatureClass tarFeatureClass)
        {
            try
            {
                this.gridDestFields.Rows.Clear();
                this.gridSrcFields.Rows.Clear();

                ITable pTable = tarFeatureClass as ITable;
                int aOIDIdx = -1;
                try
                {
                    if (pTable.HasOID)
                    {
                        aOIDIdx = pTable.FindField(pTable.OIDFieldName);
                    }
                }
                catch { }
                int j = 0;
                for (int i = 0; i < pTable.Fields.FieldCount; i++)
                {
                    IField pField = pTable.Fields.get_Field(i);
                    if (i != aOIDIdx &&
                        (pField.Type != esriFieldType.esriFieldTypeBlob) &&
                         (pField.Type != esriFieldType.esriFieldTypeGeometry) &&
                        (pField.Type != esriFieldType.esriFieldTypeOID) &&
                        (pField.Type != esriFieldType.esriFieldTypeRaster) &&
                        (pField.Name.ToUpper() != "SHAPE_LENGTH") &&
                        (pField.Name.ToUpper() != "SHAPE_AREA"))
                    {
                        gridDestFields.AllowUserToAddRows = true;
                        this.gridSrcFields.AllowUserToAddRows = true;
                        int iSrcRowIndex = this.gridSrcFields.Rows.Add(1);
                        this.gridDestFields.Rows.Add(1);
                        this.gridDestFields[0, j].Value = pField.Name.ToString().Trim();
                        this.gridDestFields[1, j].Value = pField.Type.ToString();

                        for (int idx = 0; idx < this.ColumnSrcFields.Items.Count; idx++)
                        {

                            string sValue = this.ColumnSrcFields.Items[idx].ToString().Trim();
                            string strSrc = sValue;
                            if (sValue.IndexOf('|') > 0)
                            {
                                strSrc = strSrc.Substring(0, strSrc.IndexOf('|'));
                            }
                            string strDest = pField.Name.ToString().Trim();
                            if (strSrc.ToUpper() == strDest.ToUpper())
                            {

                                this.gridSrcFields[0, iSrcRowIndex].Value = sValue;
                                break;
                                // this.gridSrcFields[1, iSrcRowIndex].Value = pField.Type.ToString();
                            }
                        }

                        this.gridDestFields.AllowUserToAddRows = false;
                        this.gridSrcFields.AllowUserToAddRows = false;
                        j++;
                    }
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show("" + ee.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void cboSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(cboSource.Text) && !string.IsNullOrWhiteSpace(cboTarget.Text))
            {
                string sourClassName = RCIS.Utility.OtherHelper.GetLeftName(cboSource.Text);
                string tarClassName = RCIS.Utility.OtherHelper.GetLeftName(cboTarget.Text);
                IFeatureClass sourClass = null;
                IFeatureClass tarClass = null;
                try
                {
                    sourClass = (currWS as IFeatureWorkspace).OpenFeatureClass(sourClassName);
                    tarClass = (currWS as IFeatureWorkspace).OpenFeatureClass(tarClassName);
                }
                catch { }
                if (sourClass != null)
                {
                    LoadSourFeatureClassField(sourClass);
                }
                if (tarClass != null)
                {
                    LoadTarFeatureClassField(tarClass);
                }
            }
        }
    }
}
