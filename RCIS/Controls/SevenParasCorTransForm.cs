using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;


namespace ElseTransform
{
    public partial class SevenParasCorTransForm : Form
    {
        public SevenParasCorTransForm()
        {
            InitializeComponent();
        }

        public ESRI.ArcGIS.Controls.AxMapControl m_MapControl;
        private string m_sPath, m_sSourceFeatureClassName, m_sTargetFeatureClassName;
        private bool m_FirstStep;
        private IGeographicCoordinateSystem m_SourceGeo;



        private void SevenParasCorTransForm_Load(object sender, EventArgs e)
        {
            //在dataGridView1内加入需要的参数:
            string[] row0 = { "1: X Axis Translation (meters) ", "0.0" };
            string[] row1 = { "2: Y Axis Translation (meters) ", "0.0" };
            string[] row2 = { "3: Z Axis Translation (meters) ", "0.0" };
            string[] row3 = { "4: X Axis Rotation (seconds) ", "0.0" };
            string[] row4 = { "5: Y Axis Rotation (seconds) ", "0.0" };
            string[] row5 = { "6: Z Axis Rotation (seconds) ", "0.0" };
            string[] row6 = { "7: Scale Difference (ppm) ", "0.0" };

            DataGridViewRowCollection rows = this.dataGridView1.Rows;
            rows.Add(row0);
            rows.Add(row1);
            rows.Add(row2);
            rows.Add(row3);
            rows.Add(row4);
            rows.Add(row5);
            rows.Add(row6);  
        }

        private void btnSelectSrcShape_Click(object sender, EventArgs e)
        {
            //选择源数据集: shape文件, 之后得到该SHP的坐标系统放置到列表:
            m_FirstStep = true;     //标识第一步是否完成
            try
            {
                string sFilter = "SHAPE文件(*.shp)|*.shp";
                openFileDialog1.Filter = sFilter;
                openFileDialog1.InitialDirectory = Application.StartupPath;
                if (openFileDialog1.ShowDialog() != DialogResult.OK)
                {
                    m_FirstStep = false;
                    return;
                }

                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                string sSHPFile = openFileDialog1.FileName;
                int nPos = sSHPFile.LastIndexOf("\\");
                string sPath = sSHPFile.Substring(0, nPos);
                string sFileName = sSHPFile.Substring(nPos + 1);
                nPos = sFileName.LastIndexOf(".");
                string sFeatureClass = sFileName.Substring(0, nPos);

                textBox1.Text = sSHPFile;
                string sNewFeatureClass = sFeatureClass + "_Project";
                int nGS = 1;
                for (; ; )
                {
                    //检查存在:
                    string ss = sPath + "\\" + sNewFeatureClass + ".shp";
                    if (File.Exists(ss) == true)
                    {
                        sNewFeatureClass = sFeatureClass + "_Project" + nGS.ToString();
                        nGS++;
                    }
                    else break;
                }
                textBox2.Text = sPath + "\\" + sNewFeatureClass;
                m_sPath = sPath;
                m_sSourceFeatureClassName = sFeatureClass;
                m_sTargetFeatureClassName = sNewFeatureClass;


                IWorkspaceFactory wsf = new ShapefileWorkspaceFactoryClass();
                IFeatureWorkspace ws = (IFeatureWorkspace)wsf.OpenFromFile(sPath, 0);
                IFeatureClass pFeatureClass = ws.OpenFeatureClass(sFeatureClass);

                //得到源数据坐标系统->comboBox1:
                IGeoDataset geoDS = pFeatureClass as IGeoDataset;
                ISpatialReference pSR = geoDS.SpatialReference;
                comboBox1.Items.Clear();
                double dCenterJ = 0.0;	//DU
                double dFalse_Easting = 0.0, dFalse_North = 0.0;
                if (pSR is IProjectedCoordinateSystem)
                {
                    IProjectedCoordinateSystem pProjectedCoordSys = pSR as IProjectedCoordinateSystem;
                    string sPRJName = "投影名称: " + pProjectedCoordSys.Projection.Name;
                    string sCentral_Meridian = "中央经线: " + pProjectedCoordSys.get_CentralMeridian(true).ToString();
                    dCenterJ = pProjectedCoordSys.get_CentralMeridian(true);
                    string sFalse_Easting = "False East: " + pProjectedCoordSys.FalseEasting.ToString();
                    dFalse_Easting = pProjectedCoordSys.FalseEasting;
                    string sFalse_Northing = "False North: " + pProjectedCoordSys.FalseNorthing.ToString();
                    dFalse_North = pProjectedCoordSys.FalseNorthing;
                    string sUnit = "坐标单位: " + pProjectedCoordSys.CoordinateUnit.Name;

                    IGeographicCoordinateSystem pGeo = pProjectedCoordSys.GeographicCoordinateSystem;
                    m_SourceGeo = pGeo;
                    string sGeoName = pGeo.Name;

                    comboBox1.Items.Add("投影坐标系:");
                    comboBox1.Items.Add(sPRJName);
                    comboBox1.Items.Add(sCentral_Meridian);
                    comboBox1.Items.Add(sUnit);
                    comboBox1.Items.Add(sFalse_Easting);
                    comboBox1.Items.Add(sFalse_Northing);
                    comboBox1.Items.Add("-------------------");
                    comboBox1.Items.Add("地理坐标系:" + sGeoName);

                    comboBox1.SelectedIndex = 0;
                }
                else
                {
                    m_FirstStep = false;
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                    MessageBox.Show("错误: 待转换数据集的坐标系需要是投影坐标系、操作被取消!");
                    return;
                }

                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            catch (Exception E)
            {
                m_FirstStep = false;
                this.Cursor = System.Windows.Forms.Cursors.Default;
                MessageBox.Show("发生错误: " + E.Message + " !");
                return;
            }
        }
        private bool m_SecondStep;
        private IGeographicCoordinateSystem m_TargetGeo;
        private IProjectedCoordinateSystem m_TargetPrj;

        private void btnoutputSpatialRefrence_Click(object sender, EventArgs e)
        {
            //选择输出坐标系统:
            m_SecondStep = true;
            try
            {
                if (m_FirstStep == null || m_FirstStep == false)
                {
                    m_SecondStep = false;
                    MessageBox.Show("请先完成第一步!");
                    return;
                }

                string sFilter = "投影文件(*.prj)|*.prj";
                openFileDialog1.Filter = sFilter;
                openFileDialog1.InitialDirectory = Application.StartupPath+"\\srprj\\CGCS2000";
                openFileDialog1.FileName = "";
                if (openFileDialog1.ShowDialog() != DialogResult.OK)
                    return;

                //在是投影坐标的前提下、取出需要的地理坐标系:
                string sPrjFile = openFileDialog1.FileName;
                StreamReader sr = new StreamReader(sPrjFile);
                string sPrjInfo = sr.ReadToEnd();
                sr.Close();
                textBox3.Text = sPrjFile;

                ISpatialReference pSR = null;
                ISpatialReferenceFactory pSRF = new SpatialReferenceEnvironmentClass();
                int nBytesRead = 0;
                pSRF.CreateESRISpatialReference(sPrjInfo, out pSR, out nBytesRead);
                if (pSR is IProjectedCoordinateSystem)
                {
                    //填充comboBox2:
                    IProjectedCoordinateSystem pProjectedCoordSys = pSR as IProjectedCoordinateSystem;
                    m_TargetPrj = pProjectedCoordSys;
                    string sPRJName = "投影名称: " + pProjectedCoordSys.Projection.Name;
                    string sCentral_Meridian = "中央经线: " + pProjectedCoordSys.get_CentralMeridian(true).ToString();
                    string sFalse_Easting = "False East: " + pProjectedCoordSys.FalseEasting.ToString();
                    string sFalse_Northing = "False North: " + pProjectedCoordSys.FalseNorthing.ToString();
                    string sUnit = "坐标单位: " + pProjectedCoordSys.CoordinateUnit.Name;

                    IGeographicCoordinateSystem pGeo = pProjectedCoordSys.GeographicCoordinateSystem;
                    m_TargetGeo = pGeo;
                    string sGeoName = pGeo.Name;

                    comboBox2.Items.Clear();
                    comboBox2.Items.Add("投影坐标系:");
                    comboBox2.Items.Add(sPRJName);
                    comboBox2.Items.Add(sCentral_Meridian);
                    comboBox2.Items.Add(sUnit);
                    comboBox2.Items.Add(sFalse_Easting);
                    comboBox2.Items.Add(sFalse_Northing);
                    comboBox2.Items.Add("-------------------");
                    comboBox2.Items.Add("地理坐标系:" + sGeoName);
                    comboBox2.SelectedIndex = 0;
                }
                else
                {
                    m_SecondStep = false;
                    MessageBox.Show("错误: 选择的不是投影坐标系统、操作被取消!");
                    return;
                }
            }
            catch (Exception E)
            {
                m_SecondStep = false;
                MessageBox.Show("错误: " + E.Message + " !");
                return;
            }

        }

        private double[] m_Paras;

        private void btnTranslateOrd_Click(object sender, EventArgs e)
        {
            //执行转换:
            try
            {
                if (m_FirstStep == null || m_FirstStep == false)
                {
                    MessageBox.Show("先执行[01]步!");
                    return;
                }
                if (m_SecondStep == null || m_SecondStep == false)
                {
                    MessageBox.Show("先执行[02]步!");
                    return;
                }

                m_Paras = new double[7];
                DataGridViewRowCollection rows = dataGridView1.Rows;
                for (int i = 0; i < rows.Count; i++)
                {
                    DataGridViewRow curRow = rows[i];
                    DataGridViewCell cell = curRow.Cells["Column1"];
                    string sName = cell.Value.ToString();
                    cell = curRow.Cells["Column2"];
                    string sValue = cell.Value.ToString();
                    if (sName == null || sValue == null)
                        continue;

                    bool bOK = true;
                    for (int k = 0; k < sValue.Length; k++)
                    {
                        if (sValue[k] == '1' || sValue[k] == '2' || sValue[k] == '3' || sValue[k] == '4' || sValue[k] == '5' || sValue[k] == '6' ||
                           sValue[k] == '7' || sValue[k] == '8' || sValue[k] == '9' || sValue[k] == '0' || sValue[k] == '+' || sValue[k] == '-' || sValue[k] == '.') ;
                        else
                        {
                            bOK = false;
                            break;
                        }
                    }
                    if (bOK == false)
                    {
                        MessageBox.Show("7参数中存在非法数字、请检查!");
                        return;
                    }

                    string sNO = sName.Substring(0, 1);
                    int nNO = Convert.ToInt16(sNO);
                    m_Paras[nNO - 1] = Convert.ToDouble(sValue);
                } //for(int i=0;...


                //构造地理转换参数:
                IWorkspaceFactory pWSF = new ShapefileWorkspaceFactoryClass();
                IFeatureWorkspace pWS = pWSF.OpenFromFile(m_sPath, 0) as IFeatureWorkspace;
                IFeatureClass sourceFC = pWS.OpenFeatureClass(m_sSourceFeatureClassName);
                esriGeometryType geoType = sourceFC.ShapeType;
                IFields pFields = new ESRI.ArcGIS.Geodatabase.FieldsClass();
                IFieldsEdit pFieldsEdit = pFields as IFieldsEdit;
                IField pField = new ESRI.ArcGIS.Geodatabase.FieldClass();
                IFieldEdit pFieldEdit = pField as IFieldEdit;
                pFieldEdit.Name_2 = "OBJECTID";
                pFieldEdit.Type_2 = esriFieldType.esriFieldTypeOID;
                pFieldsEdit.AddField(pField);
                pField = new FieldClass();
                pFieldEdit = pField as IFieldEdit;
                pFieldEdit.Name_2 = "Shape";
                pFieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;
                IGeometryDef pGeoDef = new GeometryDefClass();
                IGeometryDefEdit pGeoDefEdit = pGeoDef as IGeometryDefEdit;
                pGeoDefEdit.SpatialReference_2 = m_TargetPrj;
                pGeoDefEdit.GridCount_2 = 1;
                pGeoDefEdit.set_GridSize(0, 0.5);
                pGeoDefEdit.AvgNumPoints_2 = 2;
                pGeoDefEdit.HasM_2 = false;
                pGeoDefEdit.HasZ_2 = false;
                pGeoDefEdit.GeometryType_2 = geoType;
                pFieldEdit.GeometryDef_2 = pGeoDef;
                pFieldsEdit.AddField(pField);
                string sShapeFieldName = "";
                for (int i = 0; i < pFields.FieldCount; i++)
                {
                    if (pFields.get_Field(i).Type == esriFieldType.esriFieldTypeGeometry)
                    {
                        sShapeFieldName = pFields.get_Field(i).Name;
                        break;
                    }
                }
                for (int i = 0; i < sourceFC.Fields.FieldCount; i++)
                {
                    if (sourceFC.Fields.get_Field(i).Type != esriFieldType.esriFieldTypeGeometry &&
                        sourceFC.Fields.get_Field(i).Type != esriFieldType.esriFieldTypeOID)
                    {
                        pFieldsEdit.AddField(sourceFC.Fields.get_Field(i));
                    }
                }
                IFeatureClass targetFC = pWS.CreateFeatureClass(m_sTargetFeatureClassName, pFields, null, null, esriFeatureType.esriFTSimple, sShapeFieldName, "");

                //:转换数据
                CoordinateFrameTransformationClass trans = new CoordinateFrameTransformationClass();
                ((ICoordinateFrameTransformation)trans).PutSpatialReferences(m_SourceGeo, m_TargetGeo);
                ((ICoordinateFrameTransformation)trans).PutParameters(m_Paras[0], m_Paras[1], m_Paras[2], m_Paras[3], m_Paras[4], m_Paras[5], m_Paras[6]);
                IGeoTransformation geoTransformation = trans as IGeoTransformation;

                IWorkspaceEdit pWSE = pWS as IWorkspaceEdit;
                if (pWSE.IsBeingEdited() == false)
                    pWSE.StartEditing(true);

                int nRcds = 1;
                ITable pTable1 = pWS.OpenTable(m_sSourceFeatureClassName);
                int nSumRcds = pTable1.RowCount(null);

                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                int nGS = 0;
                ITable pTable2 = pWS.OpenTable(m_sTargetFeatureClassName);
                IQueryFilter qry = new QueryFilterClass();
                ICursor pRows = pTable1.Search(qry, false);
                IRow pCurRow = pRows.NextRow();
                while (pCurRow != null)
                {
                    ICursor pCursor = pTable2.Insert(true);
                    IRowBuffer pBuffer = pTable2.CreateRowBuffer();

                    IFields flds1 = pCurRow.Fields;
                    IFields flds2 = pBuffer.Fields;
                    for (int j = 0; j < flds1.FieldCount; j++)
                    {
                        IField fld1 = flds1.get_Field(j);
                        if (fld1.Type == esriFieldType.esriFieldTypeGeometry)
                        {
                            object oo = pCurRow.get_Value(j);
                            if (oo is System.DBNull)
                                continue;
                            IGeometry Geo = oo as IGeometry;
                            if (Geo.IsEmpty == true)
                                continue;

                            IGeometry2 geo = oo as IGeometry2;
                            geo.ProjectEx(m_TargetPrj, esriTransformDirection.esriTransformForward, geoTransformation, false, 0, 0);

                            int nShapePos = flds2.FindField("SHAPE");
                            pBuffer.set_Value(nShapePos, (object)(oo));
                            //pWSE.StartEditOperation();
                            //int nNewOID = (int)pCursor.InsertRow(pBuffer);
                            //pWSE.StopEditOperation();

                            //break;
                        }
                        else
                        {
                            if (fld1.Type != esriFieldType.esriFieldTypeOID)
                            {
                                int nShapePos = flds2.FindField(fld1.Name);
                                if (nShapePos>0)
                                    pBuffer.set_Value(nShapePos, pCurRow.get_Value(j));
                                
                            }
                        }
                    } //for(int j...
                    pWSE.StartEditOperation();
                    int nNewOID = (int)pCursor.InsertRow(pBuffer);
                    pWSE.StopEditOperation();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                    pCurRow = pRows.NextRow();

                    nGS++;
                    this.Text = "7参数坐标变换: " + nGS.ToString() + "|" + nSumRcds.ToString();
                    Application.DoEvents();
                } //while(pRow!=null)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pRows);

                pWSE.StopEditing(true);
                this.Text = "7参数坐标变换: ";
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            catch (Exception E)
            {
                this.Text = "7参数坐标变换: ";
                this.Cursor = System.Windows.Forms.Cursors.Default;
                MessageBox.Show("错误:" + E.Message + "!");
                return;
            }

            MessageBox.Show("转换完毕");
        }

        private void btnLoadparm_Click(object sender, EventArgs e)
        {
            //装入保存的7参数:
            try
            {
                string sNeedFileName = "";
                string sFilter = "7参数文件(*.SevenParas)|*.SevenParas";
                openFileDialog1.Filter = sFilter;
                openFileDialog1.InitialDirectory = Application.StartupPath;
                openFileDialog1.FileName = "";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    sNeedFileName = openFileDialog1.FileName;
                else
                    return;

                double[] dParas = new double[7];
                System.IO.FileStream fs = new System.IO.FileStream(sNeedFileName, FileMode.Open, FileAccess.Read);
                StreamReader myR = new StreamReader(fs);
                for (int i = 0; i < 7; i++)
                {
                    string sline=myR.ReadLine();
                    double d = 0;
                    double.TryParse(sline,out d);
                    dParas[i] = d;
                }
                //BinaryReader myR = new BinaryReader(fs);
                //for (int i = 0; i < 7; i++)
                //{
                //    double dd = myR.ReadDouble();
                //    dParas[i] = dd;
                //}
                myR.Close();
                fs.Close();

                DataGridViewRowCollection rows = dataGridView1.Rows;
                for (int i = 0; i < rows.Count; i++)
                {
                    DataGridViewRow curRow = rows[i];
                    DataGridViewCell cell = curRow.Cells["Column1"];
                    string sName = (string)cell.Value;
                    cell = curRow.Cells["Column2"];
                    string sValue = (string)cell.Value;
                    if (sName == null || sValue == null)
                        continue;

                    string sNO = sName.Substring(0, 1);
                    int nNO = Convert.ToInt16(sNO);
                    double dValue = dParas[nNO - 1];
                    cell.Value = dValue;
                } //for(int i=0;...
                dataGridView1.Refresh();

                MessageBox.Show("成功加载文件: '" + sNeedFileName + "' !");
            }
            catch (Exception E)
            {
                MessageBox.Show("发生错误: " + E.Message + " !");
                return;
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            //保存7参数到文件:
            try
            {
                string sNeedFileName = "";
                string sFilter = "7参数文件(*.SevenParas)|*.SevenParas";
                saveFileDialog1.Filter = sFilter;
                saveFileDialog1.InitialDirectory = Application.StartupPath;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    sNeedFileName = saveFileDialog1.FileName;
                else
                    return;

                double[] dParas = new double[7];
                DataGridViewRowCollection rows = dataGridView1.Rows;
                for (int i = 0; i < rows.Count; i++)
                {
                    DataGridViewRow curRow = rows[i];
                    DataGridViewCell cell = curRow.Cells["Column1"];
                    string sName = (string)cell.Value;
                    cell = curRow.Cells["Column2"];
                    string sValue = (string)cell.Value;
                    if (sName == null || sValue == null)
                        continue;

                    string sNO = sName.Substring(0, 1);
                    int nNO = Convert.ToInt16(sNO);
                    double dValue = Convert.ToDouble(sValue);
                    dParas[nNO - 1] = dValue;
                } //for(int i=0;...

                System.IO.FileStream fs = new System.IO.FileStream(sNeedFileName, FileMode.Create);
                BinaryWriter myW = new BinaryWriter(fs);
                for (int i = 0; i < 7; i++)
                    myW.Write((double)dParas[i]);
                myW.Close();
                fs.Close();

                MessageBox.Show("保存为文件: '" + sNeedFileName + "' !");
            }
            catch (Exception E)
            {
                MessageBox.Show("发生错误: " + E.Message + " !");
                return;
            }
        }



    }
}