using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace TDDC3D.datado
{
    public partial class FrmExcel2SHP : Form
    {
        DataTable xlsData = new DataTable();
        string excelFile = string.Empty;
        string shpFile = string.Empty;
        string spaFile = string.Empty;

        public FrmExcel2SHP()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtExcel_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.Filter = "Excel文件（*.xls，*.xlsx）|*.xls;*.xlsx";
            if (openfile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                excelFile = openfile.FileName;
                txtExcel.Text = excelFile;
                xlsData = RCIS.Database.LS_DBExcelHelper.Excel2Datatable(excelFile);
                cboWKTField.Properties.Items.Clear();
                for (int i = 0; i < xlsData.Columns.Count; i++)
                {
                    cboWKTField.Properties.Items.Add(xlsData.Columns[i].ColumnName);
                }
            }
        }

        private void txtSHP_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.Filter = "SHP格式数据（*.shp）|*.shp";
            if (savefile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                shpFile = savefile.FileName;
                txtSHP.Text = shpFile;
            }
        }

        private void txtSpatialReference_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "坐标参考文件|*.prj";
            dlg.InitialDirectory = Application.StartupPath + @"\srprj";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                spaFile = dlg.FileName;
                txtSpatialReference.Text = spaFile;
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(excelFile) ||
                string.IsNullOrWhiteSpace(shpFile) ||
                string.IsNullOrWhiteSpace(spaFile) ||
                string.IsNullOrWhiteSpace(cboWKTField.Text))
            {
                MessageBox.Show("参数不全，请先设置参数。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (xlsData == null || xlsData.Rows.Count == 0)
            {
                MessageBox.Show("Excel文件中没有数据，请确认。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("正在创建SHP数据……", "提示");
            wait.Show();
            if (System.IO.File.Exists(shpFile))
            {
                IDataset pDataset = RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(shpFile) as IDataset;
                pDataset.Delete();
            }
            ESRI.ArcGIS.Geometry.ISpatialReference pSR = RCIS.GISCommon.SpatialRefHelper.ConstructCoordinateSystem(true, spaFile);
            ESRI.ArcGIS.Geodatabase.IFeatureClass pFeatureClass = RCIS.GISCommon.WorkspaceHelper2.CreateSHP(shpFile, ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon, pSR);
            IFields pFields = pFeatureClass.Fields;
            IFieldsEdit pFieldsEdit = pFields as IFieldsEdit;
            IClass pClass = pFeatureClass as IClass;
            for (int i = 0; i < xlsData.Columns.Count; i++)
            {
                DataColumn dc = xlsData.Columns[i];
                if (dc.ColumnName != cboWKTField.Text)
                {
                    IField pField = new FieldClass();
                    IFieldEdit pFieldEdit = pField as IFieldEdit;
                    pFieldEdit.Name_2 = dc.ColumnName;
                    if (dc.DataType == typeof(double))
                    {
                        pFieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
                    }
                    else
                    {
                        pFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                        pFieldEdit.Length_2 = 254;
                    }
                    pClass.AddField(pField);
                }
            }

            IFeatureCursor pInsert = pFeatureClass.Insert(true);
            for (int j = 0; j < xlsData.Rows.Count; j++)
            {
                wait.SetCaption("正在添加第" + (j + 1).ToString() + "条数据……");
                IFeatureBuffer pFeatureBuffer = pFeatureClass.CreateFeatureBuffer();
                for (int n = 0; n < xlsData.Columns.Count; n++)
                {
                    DataColumn dc = xlsData.Columns[n];
                    if (dc.ColumnName != cboWKTField.Text)
                    {
                        pFeatureBuffer.set_Value(pFeatureClass.FindField(dc.ColumnName), xlsData.Rows[j][n]);
                    }
                    else
                    {
                        IGeometry pGeometry = RCIS.GISCommon.WKTHelper.ConvertWKTToGeometry(xlsData.Rows[j][n].ToString());
                        pFeatureBuffer.Shape = pGeometry;
                    }
                }
                pInsert.InsertFeature(pFeatureBuffer);
            }
            wait.Close();
            MessageBox.Show("完成", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
