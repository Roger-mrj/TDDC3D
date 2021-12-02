using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using System.IO;
using ESRI.ArcGIS.Geoprocessor;
using ESRI.ArcGIS.Geoprocessing;
using DevExpress.Utils;
using RCIS.GISCommon;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
using RCIS.Utility;

namespace TDDC3D.edit
{
    public partial class PickChangeDLTB : Form
    {
        public IWorkspace currWs;
        public IMap currMap;

        public PickChangeDLTB()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtSHPED_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.Filter = "SHP格式文件|*.SHP";
            if (openfile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtSHPED.Text = openfile.FileName;
                IWorkspace sourWorkspace = RCIS.GISCommon.WorkspaceHelper2.GetShapefileWorkspace(System.IO.Path.GetDirectoryName(txtSHPED.Text));
                IFeatureClass pFeatureClass1 = (sourWorkspace as IFeatureWorkspace).OpenFeatureClass(System.IO.Path.GetFileName(txtSHPED.Text));
                if (pFeatureClass1.Fields.FindField("dlbm") == -1)
                {
                    MessageBox.Show("没有找到地类字段，请确认当前数据是否正确。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtSHPED.Text = "";
                    return;
                }
                txtSHPResult.Text = System.IO.Path.GetDirectoryName(openfile.FileName) + "\\";
                btnPick.Focus();
            }
        }

        private string ej = "'051','052','053','054','061','062','063','071','072','081','082','083','084','085','086','087','088','091','092','093','094','095','101','102','103','105','106','107','113','118','121','201','202','203','204','205'";
        private string en = "'011','012','013','021','022','023','031','032','033','041','042','104','114','117','122','123'";
        private string ew = "'111','112','115','116','119','043','124','125','126','127'";
        private string sj = "'05H1','0508','0601','0602','0602','0603','0701','0702','08H1','08H2','0809','0810','09','1001','1002','1003','1004','1005','1007','1008','1009','1109','1201'";
        private string sn = "'0101','0102','0103','0201','0202','0203','0204','0301','0302','0303','0304','0305','0306','0307','0401','0402','0403','1006','1103','1104','1107','1202','1203'";
        private string sw = "'0404','1101','1102','1105','1106','1108','1110','1204','1205','1206','1207'";

        private struct dlmj
        {
            public string dlbm;
            public string dlmc;
            public double bl;
            public double mj;
            public IFeature fea;
            public IGeometry geo;
        }

        private void btnPick_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSHPED.Text) ||
                string.IsNullOrWhiteSpace(txtSHPResult.Text) ||
                string.IsNullOrWhiteSpace(txtGJJZ.Text) )
            {
                MessageBox.Show("请设置好各种数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtjs.Text) ||
                string.IsNullOrWhiteSpace(txt1202.Text) ||
                string.IsNullOrWhiteSpace(txtgd.Text) ||
                string.IsNullOrWhiteSpace(txtwy.Text) ||
                string.IsNullOrWhiteSpace(txtyj.Text) ||
                string.IsNullOrWhiteSpace(txt12022.Text) ||
                string.IsNullOrWhiteSpace(txt12023.Text) ||
                string.IsNullOrWhiteSpace(txtcl.Text) ||
                string.IsNullOrWhiteSpace(txt1206.Text) ||
                string.IsNullOrWhiteSpace(txtjs0602.Text) ||
                string.IsNullOrWhiteSpace(txt0602.Text) ||
                string.IsNullOrWhiteSpace(txtE.Text) ||
                string.IsNullOrWhiteSpace(txtS.Text))
            {
                MessageBox.Show("请设置好各种参数！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            btnPick.Enabled = false;
            btnClose.Enabled = false;
            WaitDialogForm wait = new WaitDialogForm("开始处理。", "提示");
            wait.Show();
            //创建新SHP文件
            string sdName = "地类图斑_三调";
            string bhName = "地类图斑_变化";
            IWorkspace sourWorkspace = currWs;
            IWorkspace tarWorkspace = WorkspaceHelper2.GetShapefileWorkspace(System.IO.Path.GetDirectoryName(txtSHPED.Text));
            IFeatureWorkspace pFeatureWorkspace = currWs as IFeatureWorkspace;
            IFeatureClass pDLTB = pFeatureWorkspace.OpenFeatureClass("dltb");
            IQueryFilter pcheckFilter = new QueryFilterClass();
            pcheckFilter.WhereClause = "dlbm is null or dlbm = ''";
            IFeatureCursor pcheckCursor = pDLTB.Search(pcheckFilter, true);
            IFeature pcheckFeature = pcheckCursor.NextFeature();
            if (pcheckFeature != null)
            {
                wait.Close();
                MessageBox.Show("发现图斑中有地类为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if ((tarWorkspace as IWorkspace2).get_NameExists(esriDatasetType.esriDTFeatureClass, sdName))
            {
                IDataset pDataset = (tarWorkspace as IFeatureWorkspace).OpenFeatureClass(sdName) as IDataset;
                pDataset.Delete();
            }
            if ((tarWorkspace as IWorkspace2).get_NameExists(esriDatasetType.esriDTFeatureClass, bhName))
            {
                IDataset pDataset = (tarWorkspace as IFeatureWorkspace).OpenFeatureClass(bhName) as IDataset;
                pDataset.Delete();
            }
            

            wait.SetCaption("正在创建新SHP文件");
            //创建变化shp
            IField pField = new FieldClass();
            IFieldEdit pFieldEdit = pField as IFieldEdit;
            //设置字段集 
            IFields pFields = new FieldsClass();
            IFieldsEdit pFieldsEdit = (IFieldsEdit)pFields;
            //设置字段 
            pField = new FieldClass();
            pFieldEdit = (IFieldEdit)pField;
            //创建类型为几何类型的字段 
            pFieldEdit.Name_2 = "shape";
            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;
            IGeometryDef pGeoDef = new GeometryDefClass(); //The geometry definition for the field if IsGeometry is TRUE.
            //创建地理坐标系对象
            ISpatialReference spatialReference = (pDLTB as IGeoDataset).SpatialReference;
            IGeometryDefEdit pGeoDefEdit = (IGeometryDefEdit)pGeoDef;
            pGeoDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolygon;
            pGeoDefEdit.SpatialReference_2 = spatialReference;
            pFieldEdit.GeometryDef_2 = pGeoDef;
            pFieldsEdit.AddField(pField);
            //添加其他的字段 
            pField = new FieldClass();
            pFieldEdit = (IFieldEdit)pField;
            pFieldEdit.Name_2 = "dlbm_2";
            pFieldEdit.AliasName_2 = "二调地类编码";
            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            pFieldEdit.Length_2 = 10;
            pFieldsEdit.AddField(pField);
            pField = new FieldClass();
            pFieldEdit = (IFieldEdit)pField;
            pFieldEdit.Name_2 = "dlbm_3";
            pFieldEdit.AliasName_2 = "三调地类编码";
            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            pFieldEdit.Length_2 = 10;
            pFieldsEdit.AddField(pField);
            pField = new FieldClass();
            pFieldEdit = (IFieldEdit)pField;
            pFieldEdit.Name_2 = "BSM";
            pFieldEdit.AliasName_2 = "三调标识码";
            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            pFieldEdit.Length_2 = 20;
            pFieldsEdit.AddField(pField);
            pField = new FieldClass();
            pFieldEdit = (IFieldEdit)pField;
            pFieldEdit.Name_2 = "TBYBH";
            pFieldEdit.AliasName_2 = "图斑预编号";
            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            pFieldEdit.Length_2 = 20;
            pFieldsEdit.AddField(pField);
            pField = new FieldClass();
            pFieldEdit = (IFieldEdit)pField;
            pFieldEdit.Name_2 = "TBBH";
            pFieldEdit.AliasName_2 = "图斑编号";
            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            pFieldEdit.Length_2 = 20;
            pFieldsEdit.AddField(pField);
            IFeatureClass bhFeatureClass = (tarWorkspace as IFeatureWorkspace).CreateFeatureClass(bhName, pFields, null, null, esriFeatureType.esriFTSimple, "shape", "");
            IFeatureCursor bhinsertCursor = bhFeatureClass.Insert(true);

            //创建三调shp
            //创建变化shp
            pField = new FieldClass();
            pFieldEdit = pField as IFieldEdit;
            //设置字段集 
            pFields = new FieldsClass();
            pFieldsEdit = (IFieldsEdit)pFields;
            //设置字段 
            pField = new FieldClass();
            pFieldEdit = (IFieldEdit)pField;
            //创建类型为几何类型的字段 
            pFieldEdit.Name_2 = "shape";
            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;
            pGeoDef = new GeometryDefClass(); //The geometry definition for the field if IsGeometry is TRUE.
            //创建地理坐标系对象
            pGeoDefEdit = (IGeometryDefEdit)pGeoDef;
            pGeoDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolygon;
            pGeoDefEdit.SpatialReference_2 = spatialReference;
            pFieldEdit.GeometryDef_2 = pGeoDef;
            pFieldsEdit.AddField(pField);
            //添加其他的字段 
            pField = new FieldClass();
            pFieldEdit = (IFieldEdit)pField;
            pFieldEdit.Name_2 = "TBYBH_3D";
            pFieldEdit.AliasName_2 = "三调图斑预编号";
            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            pFieldEdit.Length_2 = 20;
            pFieldsEdit.AddField(pField);
            pField = new FieldClass();
            pFieldEdit = (IFieldEdit)pField;
            pFieldEdit.Name_2 = "DLBM_3D";
            pFieldEdit.AliasName_2 = "三调地类编码";
            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            pFieldEdit.Length_2 = 10;
            pFieldsEdit.AddField(pField);
            pField = new FieldClass();
            pFieldEdit = (IFieldEdit)pField;
            pFieldEdit.Name_2 = "JSMJ_3D";
            pFieldEdit.AliasName_2 = "三调计算面积";
            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
            pFieldsEdit.AddField(pField);
            pField = new FieldClass();
            pFieldEdit = (IFieldEdit)pField;
            pFieldEdit.Name_2 = "ZDMS";
            pFieldEdit.AliasName_2 = "汇总面积以及比例";
            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            pFieldEdit.Length_2 = 200;
            pFieldsEdit.AddField(pField);
            pField = new FieldClass();
            pFieldEdit = (IFieldEdit)pField;
            pFieldEdit.Name_2 = "ZDL";
            pFieldEdit.AliasName_2 = "二调主地类";
            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            pFieldEdit.Length_2 = 50;
            pFieldsEdit.AddField(pField);
            pField = new FieldClass();
            pFieldEdit = (IFieldEdit)pField;
            pFieldEdit.Name_2 = "FHMJDL";
            pFieldEdit.AliasName_2 = "符合控制面积的地类";
            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            pFieldEdit.Length_2 = 200;
            pFieldsEdit.AddField(pField);
            pField = new FieldClass();
            pFieldEdit = (IFieldEdit)pField;
            pFieldEdit.Name_2 = "GZMS";
            pFieldEdit.AliasName_2 = "规则描述";
            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            pFieldEdit.Length_2 = 100;
            pFieldsEdit.AddField(pField);
            pField = new FieldClass();
            pFieldEdit = (IFieldEdit)pField;
            pFieldEdit.Name_2 = "JXJZLX";
            pFieldEdit.AliasName_2 = "机选举证类型";
            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            pFieldEdit.Length_2 = 50;
            pFieldsEdit.AddField(pField);
            pField = new FieldClass();
            pFieldEdit = (IFieldEdit)pField;
            pFieldEdit.Name_2 = "BZGZ";
            pFieldEdit.AliasName_2 = "备注规则";
            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            pFieldEdit.Length_2 = 100;
            pFieldsEdit.AddField(pField);
            pField = new FieldClass();
            pFieldEdit = (IFieldEdit)pField;
            pFieldEdit.Name_2 = "JXBZ";
            pFieldEdit.AliasName_2 = "机选备注";
            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            pFieldEdit.Length_2 = 50;
            pFieldsEdit.AddField(pField);
            pField = new FieldClass();
            pFieldEdit = (IFieldEdit)pField;
            pFieldEdit.Name_2 = "JZXZ";
            pFieldEdit.AliasName_2 = "举证性质";
            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            pFieldEdit.Length_2 = 20;
            pFieldsEdit.AddField(pField);
            pField = new FieldClass();
            pFieldEdit = (IFieldEdit)pField;
            pFieldEdit.Name_2 = "GJBZ";
            pFieldEdit.AliasName_2 = "举证说明";
            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            pFieldEdit.Length_2 = 20;
            pFieldsEdit.AddField(pField);
            IFeatureClass sdFeatureClass = (tarWorkspace as IFeatureWorkspace).CreateFeatureClass(sdName, pFields, null, null, esriFeatureType.esriFTSimple, "shape", "");
            //IFeatureClass sdFeatureClass = (tarWorkspace as IFeatureWorkspace).OpenFeatureClass(sdName + ".shp");
            IFeatureCursor sdinsertCursor = sdFeatureClass.Insert(true);
            
            ISpatialFilter pSpatialFilter = null;
            //根据选择获取当前地图图斑
            if (rdoSelectTB.SelectedIndex == 1)
            {
                IGeometry mapextent = (this.currMap as IActiveView).Extent;
                pSpatialFilter = new SpatialFilterClass();
                pSpatialFilter.Geometry = mapextent;
                pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            }
            long bhcount = 0;
            long sdcount = 0;
            IFeatureCursor pDLTBCursor = pDLTB.Search(pSpatialFilter, false);
            IFeatureClass edFeatureClass = (tarWorkspace as IFeatureWorkspace).OpenFeatureClass(System.IO.Path.GetFileName(txtSHPED.Text));

            wait.SetCaption("正在计算相交的面积。");
            string datetime = NowDateTime();
            string tempmdb = txtSHPResult.Text + @"temp" + datetime + ".mdb";
            IWorkspace tempWorkspace = CreateMDBWorkspace(tempmdb);
            GP_TabulateIntersection(currWs.PathName + "\\TDDC\\DLTB", "ObjectID", txtSHPED.Text, "dlbm", tempmdb + "\\dltbTable");
            wait.SetCaption("正在计算相交的图形。");
            GP_Union(pDLTB, edFeatureClass, txtSHPResult.Text + "dltbInter" + datetime + ".shp");
            ITable pTable = (tempWorkspace as IFeatureWorkspace).OpenTable("dltbTable");
            CreateIndex(pTable, "ObjectID_1");
            IFeatureClass pInterFeatureClass = RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(txtSHPResult.Text + "dltbInter" + datetime + ".shp");
            CreateIndex(pInterFeatureClass, "FID_DLTB");
            CreateIndex(pInterFeatureClass, "DLBM_1");

            IFeature dltb;
            while ((dltb = pDLTBCursor.NextFeature()) != null)
            {
                wait.SetCaption("正在计算三调图斑" + dltb.OID.ToString());
                //IGeometry sdGeometry = dltb.ShapeCopy;
                //ITopologicalOperator pToposd = sdGeometry as ITopologicalOperator;
                //if (!pToposd.IsKnownSimple) pToposd.Simplify();
                //pSpatialFilter = new SpatialFilterClass();
                //pSpatialFilter.Geometry = sdGeometry;
                //pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                //IFeatureCursor edCursor = edFeatureClass.Search(pSpatialFilter, false);
                //IFeature edFeature;
                List<dlmj> intersectmj = new List<dlmj>();
                //while ((edFeature = edCursor.NextFeature()) != null)
                //{
                //    IGeometry edGeometry = edFeature.ShapeCopy;

                //    ITopologicalOperator pTopoed = edGeometry as ITopologicalOperator;
                //    pTopoed.Simplify();
                //    edGeometry.Project(sdGeometry.SpatialReference);
                //    IGeometry pIntesect = pTopoed.Intersect(sdGeometry, esriGeometryDimension.esriGeometry2Dimension);
                //    if (pIntesect != null && pIntesect.GeometryType == esriGeometryType.esriGeometryPolygon)
                //    {
                //        pTopoed = pIntesect as ITopologicalOperator;
                //        if (!pTopoed.IsKnownSimple) pTopoed.Simplify();
                //        IArea pArea = pIntesect as IArea;
                //        if (pArea.Area != 0)
                //        {
                //            dlmj intermj = new dlmj();
                //            intermj.fea = edFeature;
                //            intermj.geo = pIntesect;
                //            intermj.dlbm = edFeature.get_Value(edFeature.Fields.FindField("dlbm")).ToString();
                //            intermj.dlmc = edFeature.get_Value(edFeature.Fields.FindField("dlmc")).ToString();
                //            intermj.bl = Math.Round(pArea.Area / (sdGeometry as IArea).Area * 100, 2);
                //            intermj.mj = Math.Round(pArea.Area, 2);
                //            intersectmj.Add(intermj);
                //        }
                //    }
                //}
                IQueryFilter pQueryFilter = new QueryFilterClass();
                pQueryFilter.WhereClause = "ObjectID_1 = " + dltb.OID.ToString();
                ICursor pCurosr = pTable.Search(pQueryFilter,true);
                IRow pRow;
                while ((pRow = pCurosr.NextRow()) != null)
                {
                    dlmj intermj = new dlmj();
                    intermj.dlbm = pRow.get_Value(pRow.Fields.FindField("dlbm")).ToString();
                    intermj.mj = Math.Round(double.Parse(pRow.get_Value(pRow.Fields.FindField("AREA")).ToString()), 2);
                    intermj.bl = Math.Round(double.Parse(pRow.get_Value(pRow.Fields.FindField("PERCENTAGE")).ToString()), 2);
                    intersectmj.Add(intermj);
                }
                OtherHelper.ReleaseComObject(pCurosr);

                var groupList = intersectmj.OrderByDescending(f => f.mj).ToList();
                string bz = string.Empty;
                string dls = string.Empty;
                foreach (var item in groupList)
                {
                    bz += item.dlbm + "=" + Math.Round(item.mj,2) + " " + item.bl + "%";
                    if (item.mj > double.Parse(txtS.Text)) dls += item.dlbm + "=" + Math.Round(item.mj,2) + " " + item.bl + "%";
                }

                IFeatureBuffer sdfeatureBuffer = sdFeatureClass.CreateFeatureBuffer();
                IFeatureBuffer bhfeatureBuffer = bhFeatureClass.CreateFeatureBuffer();
                sdfeatureBuffer.Shape = dltb.ShapeCopy; 
                if (!string.IsNullOrWhiteSpace(dltb.get_Value(dltb.Fields.FindField("TBYBH")).ToString())) sdfeatureBuffer.set_Value(sdfeatureBuffer.Fields.FindField("TBYBH_3D"), dltb.get_Value(dltb.Fields.FindField("TBYBH")).ToString());
                if (!string.IsNullOrWhiteSpace(dltb.get_Value(dltb.Fields.FindField("DLBM")).ToString())) sdfeatureBuffer.set_Value(sdfeatureBuffer.Fields.FindField("DLBM_3D"), dltb.get_Value(dltb.Fields.FindField("DLBM")).ToString());
                sdfeatureBuffer.set_Value(sdfeatureBuffer.Fields.FindField("JSMJ_3D"), Math.Round((dltb.ShapeCopy as IArea).Area, 2));
                sdfeatureBuffer.set_Value(sdfeatureBuffer.Fields.FindField("ZDMS"), bz);
                sdfeatureBuffer.set_Value(sdfeatureBuffer.Fields.FindField("ZDL"), groupList.Count == 0 ? "" : groupList.ToArray()[0].dlbm + "=" + Math.Round(groupList.ToArray()[0].mj, 2) + " " + groupList.ToArray()[0].bl + "%");
                sdfeatureBuffer.set_Value(sdfeatureBuffer.Fields.FindField("FHMJDL"), dls);

                //根据规则进行判断
                string sddl = dltb.get_Value(dltb.Fields.FindField("dlbm")).ToString();
                if (string.IsNullOrWhiteSpace(sddl)) continue;
                if (!string.IsNullOrWhiteSpace(sddl) && sddl.Substring(0, 2) != "10" && sddl != "1101" && sddl != "1107")
                {
                    //=============================================================================================
                    //二调全地类（不含203、204）流向 三调建设用地
                    if (sj.Contains(sddl))
                    {
                        foreach (var item in groupList)
                        {
                            if (item.bl > double.Parse(txtjs.Text) && item.dlbm != "201" && item.dlbm != "202" && item.dlbm != "203" && item.dlbm != "204")
                            {
                                Boolean ischange = false;
                                if (chkS.Checked == true && item.mj > double.Parse(txtS.Text))
                                {
                                    //var bhlist = intersectmj.Where(a => a.dlbm == item.dlbm).
                                    //    Select(b => new
                                    //    {
                                    //        b.dlbm,
                                    //        b.geo,
                                    //        b.fea
                                    //    });
                                    pQueryFilter = new QueryFilterClass();
                                    pQueryFilter.WhereClause = "FID_DLTB = " + dltb.OID.ToString() + " and DLBM_1 = '" + item.dlbm + "'";
                                    IFeatureCursor pbhCursor = pInterFeatureClass.Search(pQueryFilter, true);
                                    IFeature bhtb;
                                    while ((bhtb = pbhCursor.NextFeature()) != null)
                                    {
                                        IArea pA = bhtb.ShapeCopy as IArea;
                                        double s = pA.Area;
                                        IPolyline pL = Polygon2Polyline(bhtb.ShapeCopy as IPolygon);
                                        double l = pL.Length;
                                        if (chkS.Checked == true && s < double.Parse(txtS.Text)) continue;
                                        if (chkE.Checked == true && (2 * l / s) > double.Parse(txtE.Text)) continue;
                                        bhfeatureBuffer.Shape = bhtb.ShapeCopy;
                                        if (!string.IsNullOrWhiteSpace(bhtb.get_Value(bhtb.Fields.FindField("DLBM_1")).ToString())) bhfeatureBuffer.set_Value(bhfeatureBuffer.Fields.FindField("dlbm_2"), bhtb.get_Value(bhtb.Fields.FindField("DLBM_1")));
                                        if (!string.IsNullOrWhiteSpace(dltb.get_Value(dltb.Fields.FindField("dlbm")).ToString())) bhfeatureBuffer.set_Value(bhfeatureBuffer.Fields.FindField("dlbm_3"), dltb.get_Value(dltb.Fields.FindField("dlbm")));
                                        if (!string.IsNullOrWhiteSpace(dltb.get_Value(dltb.Fields.FindField("bsm")).ToString())) bhfeatureBuffer.set_Value(bhfeatureBuffer.Fields.FindField("bsm"), dltb.get_Value(dltb.Fields.FindField("bsm")));
                                        if (!string.IsNullOrWhiteSpace(dltb.get_Value(dltb.Fields.FindField("tbybh")).ToString())) bhfeatureBuffer.set_Value(bhfeatureBuffer.Fields.FindField("tbybh"), dltb.get_Value(dltb.Fields.FindField("tbybh")));
                                        if (!string.IsNullOrWhiteSpace(dltb.get_Value(dltb.Fields.FindField("tbbh")).ToString())) bhfeatureBuffer.set_Value(bhfeatureBuffer.Fields.FindField("tbbh"), dltb.get_Value(dltb.Fields.FindField("tbbh")));
                                        bhinsertCursor.InsertFeature(bhfeatureBuffer);
                                        bhcount++;
                                        ischange = true;
                                    }
                                    OtherHelper.ReleaseComObject(pbhCursor);
                                    //bhinsertCursor.Flush();
                                    if (ischange)
                                    {
                                        sdfeatureBuffer.set_Value(sdfeatureBuffer.Fields.FindField("GZMS"), "二调全地类（不含203、204）流向 三调建设用地");
                                        sdfeatureBuffer.set_Value(sdfeatureBuffer.Fields.FindField("JXJZLX"), "新增建设用地");
                                    }
                                }
                            }
                            
                        }
                    }
                    //============================================================================================
                    //二调全地类 流向 设施农用地
                    else if (sddl == "1202")
                    {
                        foreach (var item in groupList)
                        {
                            if (item.bl > double.Parse(txt1202.Text) && item.dlbm != "122")
                            {
                                Boolean ischange = false;
                                if (chkS.Checked == true && item.mj > double.Parse(txtS.Text))
                                {
                                    pQueryFilter = new QueryFilterClass();
                                    pQueryFilter.WhereClause = "FID_DLTB = " + dltb.OID.ToString() + " and DLBM_1 = '" + item.dlbm + "'";
                                    IFeatureCursor pbhCursor = pInterFeatureClass.Search(pQueryFilter, true);
                                    IFeature bhtb;
                                    while ((bhtb = pbhCursor.NextFeature()) != null)
                                    {
                                        IArea pA = bhtb.ShapeCopy as IArea;
                                        double s = pA.Area;
                                        IPolyline pL = Polygon2Polyline(bhtb.ShapeCopy as IPolygon);
                                        double l = pL.Length;
                                        if (chkS.Checked == true && s < double.Parse(txtS.Text)) continue;
                                        if (chkE.Checked == true && (2 * l / s) > double.Parse(txtE.Text)) continue;
                                        bhfeatureBuffer.Shape = bhtb.ShapeCopy;
                                        if (!string.IsNullOrWhiteSpace(bhtb.get_Value(bhtb.Fields.FindField("dlbm_1")).ToString())) bhfeatureBuffer.set_Value(bhfeatureBuffer.Fields.FindField("dlbm_2"), bhtb.get_Value(bhtb.Fields.FindField("dlbm_1")));
                                        if (!string.IsNullOrWhiteSpace(dltb.get_Value(dltb.Fields.FindField("dlbm")).ToString())) bhfeatureBuffer.set_Value(bhfeatureBuffer.Fields.FindField("dlbm_3"), dltb.get_Value(dltb.Fields.FindField("dlbm")));
                                        if (!string.IsNullOrWhiteSpace(dltb.get_Value(dltb.Fields.FindField("bsm")).ToString())) bhfeatureBuffer.set_Value(bhfeatureBuffer.Fields.FindField("bsm"), dltb.get_Value(dltb.Fields.FindField("bsm")));
                                        if (!string.IsNullOrWhiteSpace(dltb.get_Value(dltb.Fields.FindField("tbybh")).ToString())) bhfeatureBuffer.set_Value(bhfeatureBuffer.Fields.FindField("tbybh"), dltb.get_Value(dltb.Fields.FindField("tbybh")));
                                        if (!string.IsNullOrWhiteSpace(dltb.get_Value(dltb.Fields.FindField("tbbh")).ToString())) bhfeatureBuffer.set_Value(bhfeatureBuffer.Fields.FindField("tbbh"), dltb.get_Value(dltb.Fields.FindField("tbbh")));
                                        bhinsertCursor.InsertFeature(bhfeatureBuffer);
                                        bhcount++;
                                        ischange = true;
                                    }
                                    OtherHelper.ReleaseComObject(pbhCursor);
                                    //bhinsertCursor.Flush();
                                    if (ischange)
                                    {
                                        sdfeatureBuffer.set_Value(sdfeatureBuffer.Fields.FindField("GZMS"), "二调全地类 流向 设施农用地");
                                        sdfeatureBuffer.set_Value(sdfeatureBuffer.Fields.FindField("JXJZLX"), "新增设施农用地");
                                        sdfeatureBuffer.set_Value(sdfeatureBuffer.Fields.FindField("BZGZ"), "二调全地类 流向 设施农用地");
                                        sdfeatureBuffer.set_Value(sdfeatureBuffer.Fields.FindField("JXBZ"), "新增设施农用地");
                                    }
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            else if (item.bl > double.Parse(txt12023.Text) && item.dlbm == "122")
                            {
                                if (chkS.Checked == true && item.mj > double.Parse(txtS.Text))
                                {
                                    sdfeatureBuffer.set_Value(sdfeatureBuffer.Fields.FindField("BZGZ"), "二调设施农用地 流向 三调设施农用地");
                                    sdfeatureBuffer.set_Value(sdfeatureBuffer.Fields.FindField("JXBZ"), "继承二调设施农用地");
                                }
                            }
                        }
                    }
                        //===================================================================== 
                    //二调耕地 流向 三调不同类型耕地的
                    else if (sddl.Substring(0,2) == "01")
                    {
                        foreach (var item in groupList)
                        {
                            if (item.bl > double.Parse(txtgd.Text) && item.dlbm.Substring(0,2) == "01" && sddl.Substring(sddl.Length - 1) != item.dlbm.Substring(item.dlbm.Length - 1))
                            {
                                Boolean ischange = false;
                                if (chkS.Checked == true && item.mj > double.Parse(txtS.Text))
                                {
                                    pQueryFilter = new QueryFilterClass();
                                    pQueryFilter.WhereClause = "FID_DLTB = " + dltb.OID.ToString() + " and DLBM_1 = '" + item.dlbm + "'";
                                    IFeatureCursor pbhCursor = pInterFeatureClass.Search(pQueryFilter, true);
                                    IFeature bhtb;
                                    while ((bhtb = pbhCursor.NextFeature()) != null)
                                    {
                                        IArea pA = bhtb.ShapeCopy as IArea;
                                        double s = pA.Area;
                                        IPolyline pL = Polygon2Polyline(bhtb.ShapeCopy as IPolygon);
                                        double l = pL.Length;
                                        if (chkS.Checked == true && s < double.Parse(txtS.Text)) continue;
                                        if (chkE.Checked == true && (2 * l / s) > double.Parse(txtE.Text)) continue;
                                        bhfeatureBuffer.Shape = bhtb.ShapeCopy;
                                        if (!string.IsNullOrWhiteSpace(bhtb.get_Value(bhtb.Fields.FindField("dlbm_1")).ToString())) bhfeatureBuffer.set_Value(bhfeatureBuffer.Fields.FindField("dlbm_2"), bhtb.get_Value(bhtb.Fields.FindField("dlbm_1")));
                                        if (!string.IsNullOrWhiteSpace(dltb.get_Value(dltb.Fields.FindField("dlbm")).ToString())) bhfeatureBuffer.set_Value(bhfeatureBuffer.Fields.FindField("dlbm_3"), dltb.get_Value(dltb.Fields.FindField("dlbm")));
                                        if (!string.IsNullOrWhiteSpace(dltb.get_Value(dltb.Fields.FindField("bsm")).ToString())) bhfeatureBuffer.set_Value(bhfeatureBuffer.Fields.FindField("bsm"), dltb.get_Value(dltb.Fields.FindField("bsm")));
                                        if (!string.IsNullOrWhiteSpace(dltb.get_Value(dltb.Fields.FindField("tbybh")).ToString())) bhfeatureBuffer.set_Value(bhfeatureBuffer.Fields.FindField("tbybh"), dltb.get_Value(dltb.Fields.FindField("tbybh")));
                                        if (!string.IsNullOrWhiteSpace(dltb.get_Value(dltb.Fields.FindField("tbbh")).ToString())) bhfeatureBuffer.set_Value(bhfeatureBuffer.Fields.FindField("tbbh"), dltb.get_Value(dltb.Fields.FindField("tbbh")));
                                        bhinsertCursor.InsertFeature(bhfeatureBuffer);
                                        bhcount++;
                                        ischange = true;
                                    }
                                    OtherHelper.ReleaseComObject(pbhCursor);
                                    //bhinsertCursor.Flush();
                                    if (ischange)
                                    {
                                        sdfeatureBuffer.set_Value(sdfeatureBuffer.Fields.FindField("GZMS"), "二调耕地 流向 三调不同类型耕地的");
                                        sdfeatureBuffer.set_Value(sdfeatureBuffer.Fields.FindField("JXJZLX"), "耕地内部变化");
                                    }
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }
                    }
                    //===========================================================================================
                    //二调农用地 流量 三调未利用地的
                    else if (sw.Contains(sddl))
                    {
                        foreach (var item in groupList)
                        {
                            if (item.bl > double.Parse(txtwy.Text) && en.Contains(item.dlbm))
                            {
                                Boolean ischange = false;
                                if (chkS.Checked == true && item.mj > double.Parse(txtS.Text))
                                {
                                    pQueryFilter = new QueryFilterClass();
                                    pQueryFilter.WhereClause = "FID_DLTB = " + dltb.OID.ToString() + " and DLBM_1 = '" + item.dlbm + "'";
                                    IFeatureCursor pbhCursor = pInterFeatureClass.Search(pQueryFilter, true);
                                    IFeature bhtb;
                                    while ((bhtb = pbhCursor.NextFeature()) != null)
                                    {
                                        IArea pA = bhtb.ShapeCopy as IArea;
                                        double s = pA.Area;
                                        IPolyline pL = Polygon2Polyline(bhtb.ShapeCopy as IPolygon);
                                        double l = pL.Length;
                                        if (chkS.Checked == true && s < double.Parse(txtS.Text)) continue;
                                        if (chkE.Checked == true && (2 * l / s) > double.Parse(txtE.Text)) continue;
                                        bhfeatureBuffer.Shape = bhtb.ShapeCopy;
                                        if (!string.IsNullOrWhiteSpace(bhtb.get_Value(bhtb.Fields.FindField("dlbm_1")).ToString())) bhfeatureBuffer.set_Value(bhfeatureBuffer.Fields.FindField("dlbm_2"), bhtb.get_Value(bhtb.Fields.FindField("dlbm_1")));
                                        if (!string.IsNullOrWhiteSpace(dltb.get_Value(dltb.Fields.FindField("dlbm")).ToString())) bhfeatureBuffer.set_Value(bhfeatureBuffer.Fields.FindField("dlbm_3"), dltb.get_Value(dltb.Fields.FindField("dlbm")));
                                        if (!string.IsNullOrWhiteSpace(dltb.get_Value(dltb.Fields.FindField("bsm")).ToString())) bhfeatureBuffer.set_Value(bhfeatureBuffer.Fields.FindField("bsm"), dltb.get_Value(dltb.Fields.FindField("bsm")));
                                        if (!string.IsNullOrWhiteSpace(dltb.get_Value(dltb.Fields.FindField("tbybh")).ToString())) bhfeatureBuffer.set_Value(bhfeatureBuffer.Fields.FindField("tbybh"), dltb.get_Value(dltb.Fields.FindField("tbybh")));
                                        if (!string.IsNullOrWhiteSpace(dltb.get_Value(dltb.Fields.FindField("tbbh")).ToString())) bhfeatureBuffer.set_Value(bhfeatureBuffer.Fields.FindField("tbbh"), dltb.get_Value(dltb.Fields.FindField("tbbh")));
                                        bhinsertCursor.InsertFeature(bhfeatureBuffer);
                                        bhcount++;
                                        ischange = true;
                                    }
                                    OtherHelper.ReleaseComObject(pbhCursor);
                                    //bhinsertCursor.Flush();
                                    if (ischange)
                                    {
                                        sdfeatureBuffer.set_Value(sdfeatureBuffer.Fields.FindField("GZMS"), "二调农用地 流向 三调未利用地的");
                                        sdfeatureBuffer.set_Value(sdfeatureBuffer.Fields.FindField("JXJZLX"), "农变未");
                                    }
                                }
                            }
                        }
                    }
                        //========================================================================================
                    //一级地类变化，非重点
                    else
                    {
                        foreach (var item in groupList)
                        {
                            if (item.bl > double.Parse(txtyj.Text) && item.dlbm.Substring(0,2) != sddl.Substring(0,2)
                                && !((sddl.Substring(0, 2) == "05" || sddl.Substring(0, 2) == "06" || sddl.Substring(0, 2) == "07" || sddl.Substring(0, 2) == "08") &&
                                (item.dlbm == "201" || item.dlbm == "202" || item.dlbm == "203")))
                            {
                                Boolean ischange = false;
                                if (chkS.Checked == true && item.mj > double.Parse(txtS.Text))
                                {
                                    pQueryFilter = new QueryFilterClass();
                                    pQueryFilter.WhereClause = "FID_DLTB = " + dltb.OID.ToString() + " and DLBM_1 = '" + item.dlbm + "'";
                                    IFeatureCursor pbhCursor = pInterFeatureClass.Search(pQueryFilter, true);
                                    IFeature bhtb;
                                    while ((bhtb = pbhCursor.NextFeature()) != null)
                                    {
                                        IArea pA = bhtb.ShapeCopy as IArea;
                                        double s = pA.Area;
                                        IPolyline pL = Polygon2Polyline(bhtb.ShapeCopy as IPolygon);
                                        double l = pL.Length;
                                        if (chkS.Checked == true && s < double.Parse(txtS.Text)) continue;
                                        if (chkE.Checked == true && (2 * l / s) > double.Parse(txtE.Text)) continue;
                                        bhfeatureBuffer.Shape = bhtb.ShapeCopy;
                                        if (!string.IsNullOrWhiteSpace(bhtb.get_Value(bhtb.Fields.FindField("dlbm_1")).ToString())) bhfeatureBuffer.set_Value(bhfeatureBuffer.Fields.FindField("dlbm_2"), bhtb.get_Value(bhtb.Fields.FindField("dlbm_1")));
                                        if (!string.IsNullOrWhiteSpace(dltb.get_Value(dltb.Fields.FindField("dlbm")).ToString())) bhfeatureBuffer.set_Value(bhfeatureBuffer.Fields.FindField("dlbm_3"), dltb.get_Value(dltb.Fields.FindField("dlbm")));
                                        if (!string.IsNullOrWhiteSpace(dltb.get_Value(dltb.Fields.FindField("bsm")).ToString())) bhfeatureBuffer.set_Value(bhfeatureBuffer.Fields.FindField("bsm"), dltb.get_Value(dltb.Fields.FindField("bsm")));
                                        if (!string.IsNullOrWhiteSpace(dltb.get_Value(dltb.Fields.FindField("tbybh")).ToString())) bhfeatureBuffer.set_Value(bhfeatureBuffer.Fields.FindField("tbybh"), dltb.get_Value(dltb.Fields.FindField("tbybh")));
                                        if (!string.IsNullOrWhiteSpace(dltb.get_Value(dltb.Fields.FindField("tbbh")).ToString())) bhfeatureBuffer.set_Value(bhfeatureBuffer.Fields.FindField("tbbh"), dltb.get_Value(dltb.Fields.FindField("tbbh")));
                                        bhinsertCursor.InsertFeature(bhfeatureBuffer);
                                        bhcount++;
                                        ischange = true;
                                    }
                                    OtherHelper.ReleaseComObject(pbhCursor);
                                    //bhinsertCursor.Flush();
                                    if (ischange)
                                    {
                                        sdfeatureBuffer.set_Value(sdfeatureBuffer.Fields.FindField("GZMS"), "一级地类变化，非重点");
                                        sdfeatureBuffer.set_Value(sdfeatureBuffer.Fields.FindField("JXJZLX"), "一级类变化需举证");
                                    }
                                }
                            }
                        }
                    }
                    /////////////////////////////////////////////////////////////////////////////////////////
                    //========================================================================================
                    /////////////////////////////////////////////////////////////////////////////////////////
                    //二调是耕地 三调地类是草地 、林地的
                    if (sddl.Substring(0,2) == "03" || sddl.Substring(0,2) == "04")
                    {
                        foreach (var item in groupList)
                        {
                            if (item.bl > double.Parse(txtcl.Text) && item.dlbm.Substring(0,2) == "01")
                            {
                                sdfeatureBuffer.set_Value(sdfeatureBuffer.Fields.FindField("BZGZ"), "二调是耕地 三调地类是草地 、林地的");
                                sdfeatureBuffer.set_Value(sdfeatureBuffer.Fields.FindField("JXBZ"), "撂荒地");
                            }
                        }
                    }
                    //=================================================================================================
                    //二调全地类流向 裸土地
                    else if (sddl == "1206")
                    {
                        foreach (var item in groupList)
                        {
                            if (item.bl > double.Parse(txt1206.Text) && item.dlbm.Substring(0, 2) != "127")
                            {
                                sdfeatureBuffer.set_Value(sdfeatureBuffer.Fields.FindField("BZGZ"), "二调全地类流向 裸土地");
                                sdfeatureBuffer.set_Value(sdfeatureBuffer.Fields.FindField("JXBZ"), "动土");
                            }
                        }
                    }
                    //二调204流向 三调建设用地（不含0602）
//====================================================================================================
                    else if (sj.Contains(sddl) && sddl != "0602")
                    {
                        foreach (var item in groupList)
                        {
                            if (item.bl > double.Parse(txtjs0602.Text) && item.dlbm.Substring(0, 2) == "204")
                            {
                                sdfeatureBuffer.set_Value(sdfeatureBuffer.Fields.FindField("BZGZ"), "二调204流向 三调建设用地（不含0602）");
                                sdfeatureBuffer.set_Value(sdfeatureBuffer.Fields.FindField("JXBZ"), "建设用地内部变化");
                            }
                        }
                    }
                        //===================================================================/
                    //二调全地类 流向采矿用地
                    else if (sddl == "0602")
                    {
                        foreach (var item in groupList)
                        {
                            if (item.bl > double.Parse(txt0602.Text) && item.dlbm.Substring(0, 2) == "062")
                            {
                                sdfeatureBuffer.set_Value(sdfeatureBuffer.Fields.FindField("BZGZ"), "二调全地类 流向采矿用地");
                                sdfeatureBuffer.set_Value(sdfeatureBuffer.Fields.FindField("JXBZ"), "采矿");
                            }
                        }
                    }
                    
                    //sdinsertCursor.Flush();
                }
                sdinsertCursor.InsertFeature(sdfeatureBuffer);
                sdcount++;
                if (bhcount > 100)
                {
                    bhinsertCursor.Flush();
                    bhcount = 0;
                }
                if (sdcount > 100)
                {
                    sdinsertCursor.Flush();
                    sdcount = 0;
                }
            }
            bhinsertCursor.Flush();
            sdinsertCursor.Flush();
            OtherHelper.ReleaseComObject(bhinsertCursor);
            OtherHelper.ReleaseComObject(sdinsertCursor);
            OtherHelper.ReleaseComObject(pDLTBCursor);

            ////循环国家举证图斑A类，根据规则判断并未生成的三调图斑所添加JZXZ字段未国举
            wait.SetCaption("计算国家举证图斑与三调图斑面积");
            IWorkspace gjWorkspace = WorkspaceHelper2.GetShapefileWorkspace(System.IO.Path.GetDirectoryName(txtGJJZ.Text));
            IFeatureClass gjFeatureClass = (gjWorkspace as IFeatureWorkspace).OpenFeatureClass(System.IO.Path.GetFileName(txtGJJZ.Text));
            GP_TabulateIntersection(txtGJJZ.Text, gjFeatureClass.OIDFieldName, txtSHPResult.Text + sdName + ".shp", sdFeatureClass.OIDFieldName, tempmdb + "\\dltbGJ");
            pTable = (tempWorkspace as IFeatureWorkspace).OpenTable("dltbGJ");
            CreateIndex(pTable, gjFeatureClass.OIDFieldName);
            IFeatureCursor gjFeatureCursor = gjFeatureClass.Search(null, false);
            //IFeatureCursor sdCursor = sdFeatureClass.Update(null, true);
            IFeature gjFeature;
            while ((gjFeature = gjFeatureCursor.NextFeature()) != null)
            {
                wait.SetCaption("正在计算国家举证图斑" + gjFeature.OID.ToString());
                string nyypdl = gjFeature.get_Value(gjFeature.Fields.FindField("NYYPDL")).ToString();
                if (nyypdl.Substring(0, 2) == "10" || nyypdl == "1101" || nyypdl == "1107") continue;
                Boolean ischeckdl = (nyypdl.Substring(0, 1) != "0" && nyypdl.Substring(0, 1) != "1");
                //IGeometry gjGeometry = gjFeature.ShapeCopy;
                //ITopologicalOperator pTopoGj = gjGeometry as ITopologicalOperator;
                //if (!pTopoGj.IsKnownSimple) pTopoGj.Simplify();
                //ISpatialIndex pSpatialIndex = gjGeometry as ISpatialIndex;
                //pSpatialIndex.AllowIndexing = true;
                //pSpatialIndex.Invalidate();
                //pSpatialFilter = new SpatialFilterClass();
                //pSpatialFilter.Geometry = gjGeometry;
                //pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                //IFeatureCursor sdCursor = sdFeatureClass.Update(pSpatialFilter, false);
                //IFeature sdFeature;
                IQueryFilter pQueryFilter = new QueryFilterClass();
                pQueryFilter.WhereClause = gjFeatureClass.OIDFieldName + " = " + gjFeature.OID.ToString();
                ICursor pCurosr = pTable.Search(pQueryFilter, true);
                List<dlmj> gjlist = new List<dlmj>();
                IRow pRow;
                while ((pRow = pCurosr.NextRow()) != null)
                {
                    string objfield = pRow.Fields.FindField(sdFeatureClass.OIDFieldName + "_1") == -1 ? sdFeatureClass.OIDFieldName : sdFeatureClass.OIDFieldName + "_1";
                    int objid = int.Parse(pRow.get_Value(pRow.Fields.FindField(objfield)).ToString());
                    IFeature sdFeature = sdFeatureClass.GetFeature(objid);
                    string sddl = sdFeature.get_Value(sdFeature.Fields.FindField("DLBM_3D")).ToString();
                    if (ischeckdl)
                    {
                        if (sddl.Substring(0, 2) == "10" || sddl == "1101" || sddl == "1107") continue;
                        //IGeometry sdGeometry = sdFeature.ShapeCopy;
                        //ITopologicalOperator pTopoSd = sdGeometry as ITopologicalOperator;
                        //if (!pTopoSd.IsKnownSimple) pTopoSd.Simplify();
                        //if (sdGeometry.SpatialReference != gjGeometry.SpatialReference) sdGeometry.Project(gjGeometry.SpatialReference);
                        //IGeometry pIntersect = pTopoSd.Intersect(gjGeometry, esriGeometryDimension.esriGeometry2Dimension);
                        //if (pIntersect != null && pIntersect.GeometryType == esriGeometryType.esriGeometryPolygon)
                        //{

                        //    pTopoSd = pIntersect as ITopologicalOperator;
                        //    if (!pTopoSd.IsKnownSimple) pTopoSd.Simplify();
                        //    IArea pArea = pTopoSd as IArea;
                        //    if (pArea.Area != 0)
                        //    {
                        if (Math.Round(double.Parse(pRow.get_Value(pRow.Fields.FindField("PERCENTAGE")).ToString()), 2) > double.Parse(txtGJ.Text) && Math.Round(double.Parse(pRow.get_Value(pRow.Fields.FindField("AREA")).ToString()) / (sdFeature.ShapeCopy as IArea).Area * 100, 2) > double.Parse(txtSelf.Text))
                                {
                                    sdFeature.set_Value(sdFeature.Fields.FindField("jzxz"), "国举");
                                    sdFeature.set_Value(sdFeature.Fields.FindField("GJBZ"), nyypdl);
                                    sdFeature.Store();
                                }
                        //    }
                        //}
                    }
                    else
                    {
                        //IGeometry sdGeometry = sdFeature.ShapeCopy;
                        //ITopologicalOperator pTopoSd = sdGeometry as ITopologicalOperator;
                        //if (!pTopoSd.IsKnownSimple) pTopoSd.Simplify();
                        //if (sdGeometry.SpatialReference != gjFeature.ShapeCopy.SpatialReference) sdGeometry.Project(gjFeature.ShapeCopy.SpatialReference);
                        //IGeometry pIntersect = pTopoSd.Intersect(gjFeature.ShapeCopy, esriGeometryDimension.esriGeometry2Dimension);
                        //if (pIntersect != null && pIntersect.GeometryType == esriGeometryType.esriGeometryPolygon)
                        //{
                        //    pTopoSd = pIntersect as ITopologicalOperator;
                        //    if (!pTopoSd.IsKnownSimple) pTopoSd.Simplify();
                        //    IArea pArea = pIntersect as IArea;
                        //    if (pArea.Area != 0)
                        //    {
                                dlmj intermj = new dlmj();
                                intermj.fea = sdFeature;
                                //intermj.geo = pIntersect;
                                intermj.dlbm = sdFeature.get_Value(sdFeature.Fields.FindField("dlbm_3d")).ToString();
                                intermj.bl = Math.Round(double.Parse(pRow.get_Value(pRow.Fields.FindField("PERCENTAGE")).ToString()), 2);
                                intermj.mj = Math.Round(double.Parse(pRow.get_Value(pRow.Fields.FindField("AREA")).ToString()), 2);
                                gjlist.Add(intermj);
                        //    }
                        //}
                    }
                }
                OtherHelper.ReleaseComObject(pCurosr);
                if (!ischeckdl)
                {
                    //二调全地类（不含203、204）流向 三调建设用地
                    if (nyypdl == "1109")
                    {
                        double sumbl = 0;
                        foreach (var item in gjlist)
                        {
                            if (item.dlbm.Substring(0, nyypdl.Length) != nyypdl) sumbl += item.bl;
                        }
                        if (sumbl > double.Parse(txtjs.Text))
                        {
                            var bhlist = gjlist.Where(a => a.dlbm != nyypdl && (a.dlbm.Substring(0, 2) == "01" || a.dlbm.Substring(0, 2) == "02" || a.dlbm.Substring(0, 2) == "03" || a.dlbm.Substring(0, 2) == "04" || a.dlbm == "1105" || a.dlbm == "1106")).
                                        Select(b => new
                                        {
                                            b.dlbm,
                                            //b.geo,
                                            b.fea,
                                            b.mj
                                        });
                            foreach (var item in bhlist)
                            {
                                ITopologicalOperator pselfTopo = item.fea.ShapeCopy as ITopologicalOperator;
                                if (!pselfTopo.IsKnownSimple) pselfTopo.Simplify();
                                IArea pSelfArea = pselfTopo as IArea;
                                if (item.mj / pSelfArea.Area * 100 > double.Parse(txtSelf.Text))
                                {
                                    if (string.IsNullOrWhiteSpace(item.fea.get_Value(item.fea.Fields.FindField("JZXZ")).ToString()))
                                    {
                                        IFeature t = sdFeatureClass.GetFeature(item.fea.OID);
                                        t.set_Value(t.Fields.FindField("JZXZ"), "国举");
                                        t.set_Value(t.Fields.FindField("GJBZ"), "新增建设用地");
                                        t.Store();
                                    }
                                }
                                
                            }
                        }
                    }
                    //===========================================================================================
                    //二调农用地 流量 三调未利用地的
                    else if (nyypdl == "12" || nyypdl == "1105" || nyypdl == "1106" || nyypdl == "1109")
                    {
                        double sumbl = 0;
                        foreach (var item in gjlist)
                        {
                            if (item.dlbm.Substring(0, nyypdl.Length) != nyypdl && (item.dlbm == "01" || item.dlbm == "02" || item.dlbm == "03" || item.dlbm == "04")) sumbl += item.bl;
                        }
                        if (sumbl > double.Parse(txtwy.Text))
                        {
                            var bhlist = gjlist.Where(a => a.dlbm.Substring(0, nyypdl.Length) != nyypdl && (a.dlbm == "01" || a.dlbm == "02" ||a.dlbm == "03" ||a.dlbm == "04")).
                                        Select(b => new
                                        {
                                            b.dlbm,
                                            //b.geo,
                                            b.fea,
                                            b.mj
                                        });
                            foreach (var item in bhlist)
                            {
                                ITopologicalOperator pselfTopo = item.fea.ShapeCopy as ITopologicalOperator;
                                if (!pselfTopo.IsKnownSimple) pselfTopo.Simplify();
                                IArea pSelfArea = pselfTopo as IArea;
                                if (item.mj / pSelfArea.Area * 100 > double.Parse(txtSelf.Text))
                                {
                                    if (string.IsNullOrWhiteSpace(item.fea.get_Value(item.fea.Fields.FindField("JZXZ")).ToString()))
                                    {
                                        IFeature t = sdFeatureClass.GetFeature(item.fea.OID);
                                        t.set_Value(t.Fields.FindField("JZXZ"), "国举");
                                        t.set_Value(t.Fields.FindField("GJBZ"), "农变未");
                                        t.Store();
                                    }
                                }
                                
                            }
                        }
                    }
                    //========================================================================================
                    //一级地类变化，非重点
                    else
                    {
                        char[] splitchar = { '/', '+' };
                        string[] sDLS = nyypdl.Split(splitchar);
                        switch (sDLS.Count())
                        {
                            case 1:
                                double sumbl = 0;
                                foreach (var item in gjlist)
                                {
                                    if (item.dlbm.Substring(0, nyypdl.Length) != nyypdl) sumbl += item.bl;
                                }
                                if (sumbl > double.Parse(txtwy.Text))
                                {
                                    var bhlist = gjlist.Where(a => a.dlbm.Substring(0, nyypdl.Length) != nyypdl && a.dlbm.Substring(0,2) != "10" && a.dlbm != "1101" && a.dlbm != "1107").
                                                Select(b => new
                                                {
                                                    b.dlbm,
                                                    //b.geo,
                                                    b.fea,
                                                    b.mj
                                                });
                                    foreach (var item in bhlist)
                                    {
                                        ITopologicalOperator pselfTopo = item.fea.ShapeCopy as ITopologicalOperator;
                                        if (!pselfTopo.IsKnownSimple) pselfTopo.Simplify();
                                        IArea pSelfArea = pselfTopo as IArea;
                                        if (pSelfArea.Area == 0) continue;
                                        if (item.mj / pSelfArea.Area * 100 > double.Parse(txtSelf.Text))
                                        {
                                            if (string.IsNullOrWhiteSpace(item.fea.get_Value(item.fea.Fields.FindField("JZXZ")).ToString()))
                                            {
                                                IFeature t = sdFeatureClass.GetFeature(item.fea.OID);
                                                t.set_Value(t.Fields.FindField("JZXZ"), "国举");
                                                t.set_Value(t.Fields.FindField("GJBZ"), "一级类变化");
                                                t.Store();
                                            }
                                        }
                                    }
                                }
                                break;
                            case 2:
                                double sumbl1 = 0;
                                string dl1 = sDLS[0];
                                foreach (var item in gjlist)
                                {
                                    if (item.dlbm.Substring(0, dl1.Length) != dl1) sumbl1 += item.bl;
                                }
                                double sumbl2 = 0;
                                string dl2 = sDLS[1];
                                foreach (var item in gjlist)
                                {
                                    if (item.dlbm.Substring(0, dl2.Length) != dl2) sumbl2 += item.bl;
                                }
                                if (!(sumbl1 > 100 - double.Parse(txtwy.Text) || sumbl2 > 100 - double.Parse(txtwy.Text)))
                                {
                                    var bhlist = gjlist.Where(a => a.dlbm.Substring(0, dl1.Length) != dl1 && a.dlbm.Substring(0, dl2.Length) != dl2 && a.dlbm.Substring(0, 2) != "10" && a.dlbm != "1101" && a.dlbm != "1107").
                                                Select(b => new
                                                {
                                                    b.dlbm,
                                                    //b.geo,
                                                    b.fea,
                                                    b.mj
                                                });
                                    foreach (var item in bhlist)
                                    {
                                        ITopologicalOperator pselfTopo = item.fea.ShapeCopy as ITopologicalOperator;
                                        if (!pselfTopo.IsKnownSimple) pselfTopo.Simplify();
                                        IArea pSelfArea = pselfTopo as IArea;
                                        if (pSelfArea.Area == 0) continue;
                                        if (item.mj / pSelfArea.Area * 100 > double.Parse(txtSelf.Text))
                                        {
                                            if (string.IsNullOrWhiteSpace(item.fea.get_Value(item.fea.Fields.FindField("JZXZ")).ToString()))
                                            {
                                                IFeature t = sdFeatureClass.GetFeature(item.fea.OID);
                                                t.set_Value(t.Fields.FindField("JZXZ"), "国举");
                                                t.set_Value(t.Fields.FindField("GJBZ"), "一级类变化");
                                                t.Store();
                                            }
                                        }
                                        
                                    }
                                }
                                break;
                        }
                    }
                }
            }
            OtherHelper.ReleaseComObject(gjFeatureCursor);
            wait.SetCaption("正在删除临时数据。");
            (pInterFeatureClass as IDataset).Delete();
            //OtherHelper.ReleaseComObject(tempWorkspace);
            IEnumDataset pEnumDataset = tempWorkspace.get_Datasets(esriDatasetType.esriDTAny);
            pEnumDataset.Reset();
            IDataset pset;
            while ((pset = pEnumDataset.Next()) != null)
            {
                pset.Delete();
            }
            pset = tempWorkspace as IDataset;
            pset.Delete();
            wait.Close();
            MessageBox.Show("提取完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnClose.Enabled = true;
            btnPick.Enabled = true;
        }
        private void CreateIndex(ITable pTable, string fieldName)
        {
            IIndex pIndex = new IndexClass();
            IIndexEdit pIndexEdit = pIndex as IIndexEdit;
            IFields pFieldsIndex = new FieldsClass();
            IFieldsEdit pFieldsEditIndex = pFieldsIndex as IFieldsEdit;
            int feildindex = pTable.Fields.FindField(fieldName);
            IField pFieldIndex = pTable.Fields.Field[feildindex];
            pFieldsEditIndex.FieldCount_2 = 1;
            pFieldsEditIndex.set_Field(0, pFieldIndex);
            pIndexEdit.Fields_2 = pFieldsIndex;
            pIndexEdit.Name_2 = fieldName;
            pIndexEdit.IsAscending_2 = true;
            pTable.AddIndex(pIndex);
        }

        private void CreateIndex(IFeatureClass pFeatureClass, string fieldName)
        {
            IIndex pIndex = new IndexClass();
            IIndexEdit pIndexEdit = pIndex as IIndexEdit;
            IFields pFieldsIndex = new FieldsClass();
            IFieldsEdit pFieldsEditIndex = pFieldsIndex as IFieldsEdit;
            int feildindex = pFeatureClass.Fields.FindField("FID_DLTB");
            IField pFieldIndex = pFeatureClass.Fields.Field[feildindex];
            pFieldsEditIndex.FieldCount_2 = 1;
            pFieldsEditIndex.set_Field(0, pFieldIndex);
            pIndexEdit.Fields_2 = pFieldsIndex;
            pIndexEdit.Name_2 = fieldName;
            pIndexEdit.IsAscending_2 = true;
            pFeatureClass.AddIndex(pIndex);  
        }

        private IWorkspace CreateMDBWorkspace(string fileName)
        {
            IWorkspaceFactory pWorkspaceFactory = new ESRI.ArcGIS.DataSourcesGDB.AccessWorkspaceFactoryClass();
            string fileDirectory = System.IO.Path.GetDirectoryName(fileName);
            string mdbfileName = System.IO.Path.GetFileNameWithoutExtension(fileName);
            IWorkspaceName workspaceName = pWorkspaceFactory.Create(fileDirectory, mdbfileName, null, 0);
            IName name = workspaceName as IName;
            IWorkspace workspace = (IWorkspace)name.Open();
            return workspace;
        }

        private string NowDateTime()
        {
            return DateTime.Now.Year.ToString("0000") + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00");
        }

        private void GP_Union(IFeatureClass pFeatureClass1, IFeatureClass pFeatureClass2, string outFeatureClass)
        {
            Geoprocessor geoprocessor = new Geoprocessor();
            geoprocessor.OverwriteOutput = true;
            ESRI.ArcGIS.AnalysisTools.Union union = new ESRI.ArcGIS.AnalysisTools.Union();
 
            IGpValueTableObject gpValueTableObject = new GpValueTableObjectClass();//对两个要素类进行相交运算
            gpValueTableObject.SetColumns(2);
            object o1 = pFeatureClass1;//输入IFeatureClass 1 
            object o2 = pFeatureClass2;//输入IFeatureClass 2 
            gpValueTableObject.AddRow(ref o1);
            gpValueTableObject.AddRow(ref o2);
            union.in_features = gpValueTableObject;
            union.out_feature_class = outFeatureClass;
            geoprocessor.Execute(union, null);
        }

        private void GP_TabulateIntersection(string ZoneFeatures, string ZoneField, string ClassFeatures, string ClassField, string outTable)
        {
            Geoprocessor geoprocessor = new Geoprocessor();
            geoprocessor.OverwriteOutput = true;
            ESRI.ArcGIS.AnalysisTools.TabulateIntersection TabInter = new ESRI.ArcGIS.AnalysisTools.TabulateIntersection();

            TabInter.in_zone_features = ZoneFeatures;
            TabInter.zone_fields = ZoneField;
            TabInter.in_class_features = ClassFeatures;
            TabInter.class_fields = ClassField;
            TabInter.out_table = outTable;
            geoprocessor.Execute(TabInter, null);
        }

        private IPolyline Polygon2Polyline(IPolygon pPolygon)
        {
            ISegmentCollection segmentCollction = pPolygon as ISegmentCollection;
            ISegmentCollection polyline = new Polyline() as ISegmentCollection;
            object missing = Type.Missing;
            for (int m = 0; m < segmentCollction.SegmentCount; m++)
            {
            ISegment pSeg = segmentCollction.get_Segment(m);
            polyline.AddSegment(pSeg, ref missing, ref missing);
            }
            IPolyline pGeo = polyline as IPolyline;
            return pGeo;
        }

        private void PickChangeDLTB_Load(object sender, EventArgs e)
        {
            //IFeatureWorkspace pFeatureWorkspace = currWs as IFeatureWorkspace;
            //IFeatureClass pFeatureClass = pFeatureWorkspace.OpenFeatureClass("dltb");
            //IFeatureCursor pFeatureCursor = pFeatureClass.Search(null, false);
            //DataStatistics dataSta = new DataStatisticsClass();
            //dataSta.Cursor = pFeatureCursor as ICursor;
            //dataSta.Field = "qsdwmc";
            //System.Collections.IEnumerator pEnum = dataSta.UniqueValues;
            //pEnum.Reset();
            //while (pEnum.MoveNext())
            //{
            //    lstQSDW.Items.Add(pEnum.Current);
            //}
            //lstQSDW.CheckAll();
        }

        private void txtGJJZ_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.Filter = "SHP格式文件|*.SHP";
            if (openfile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtGJJZ.Text = openfile.FileName;
                IWorkspace sourWorkspace = RCIS.GISCommon.WorkspaceHelper2.GetShapefileWorkspace(System.IO.Path.GetDirectoryName(txtGJJZ.Text));
                IFeatureClass pFeatureClass1 = (sourWorkspace as IFeatureWorkspace).OpenFeatureClass(System.IO.Path.GetFileName(txtGJJZ.Text));
                if (pFeatureClass1.Fields.FindField("NYYPDL") == -1)
                {
                    MessageBox.Show("没有找到地类字段，请确认当前数据是否正确。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtGJJZ.Text = "";
                    return;
                }
                btnPick.Focus();
            }
        }

    }
}
