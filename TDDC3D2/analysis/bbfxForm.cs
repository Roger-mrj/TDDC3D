using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;

namespace TDDC3D.analysis
{
    public partial class bbfxForm : Form
    {
        public bbfxForm()
        {
            InitializeComponent();
        }

        //public DrawingTool MyTool = null;
        public AxMapControl mapControl;
        public IGeometry m_SelGeo = null;
        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioGroup1.SelectedIndex == 0)
            {
                txtPath.Enabled = true;
                cmbField.Enabled = true;
                txtPath.Text = "";
                cmbField.Properties.Items.Clear();
                mapControl.ActiveView.GraphicsContainer.DeleteAllElements();
                mapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewBackground, null, null);
                this.mapControl.CurrentTool = null;
            }
            else
            {
                mapControl.ActiveView.GraphicsContainer.DeleteAllElements();
                mapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewBackground, null, null);

                txtPath.Enabled = false;
                cmbField.Enabled = false;
                txtPath.Text = "";
                cmbField.Properties.Items.Clear();
                analysis.DrawingTool MyTool = new DrawingTool();
                MyTool.OnCreate(this.mapControl.Object);
                MyTool.m_UseForm = this;
                this.Visible = false;
                this.mapControl.CurrentTool = MyTool;
            }
        }

        private void bbfxForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            mapControl.ActiveView.GraphicsContainer.DeleteAllElements();
            mapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewBackground, null, null);

        }

        private void txtPath_Click(object sender, EventArgs e)
        {
            List<string> Arr = new List<string>() 
            {
                "ZLDWDM","QSDWDM","QSXZ","GDPDJB","GDLX","ZZSXDM","CZCSXM","TBXHDM","MSSM","FRDBS"
            };
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "shapefile(*.shp)|*.shp|All files(*.*)|*.*";
            if (file.ShowDialog() == DialogResult.OK)
            {
                txtPath.Text = file.FileName;
                cmbField.Properties.Items.Clear();
                IFeatureClass pShpClass = RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(file.FileName);
                for (int i = 0; i < pShpClass.Fields.FieldCount; i++)
                {
                    IField pField = pShpClass.Fields.get_Field(i);
                    if (!Arr.Contains(pField.Name))
                        cmbField.Properties.Items.Add(pField.Name + "|" + pField.AliasName);
                }
            }
        }

        private void bbfxForm_FormClosed(object sender, FormClosedEventArgs e)
        {

            if (e.CloseReason == CloseReason.UserClosing)
                this.mapControl.CurrentTool = null;
        }

        private void beDestDir_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            this.beDestDir.Text = dlg.SelectedPath;
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            //全选
            foreach (Control c in this.groupBoxTabs.Controls)
            {
                if (c is DevExpress.XtraEditors.CheckEdit)
                {
                    DevExpress.XtraEditors.CheckEdit ce = (DevExpress.XtraEditors.CheckEdit)c;
                    ce.Checked = true;
                }
            }
        }

        private void btnUnselect_Click(object sender, EventArgs e)
        {
            foreach (Control c in this.groupBoxTabs.Controls)
            {
                if (c is DevExpress.XtraEditors.CheckEdit)
                {
                    DevExpress.XtraEditors.CheckEdit ce = (DevExpress.XtraEditors.CheckEdit)c;
                    ce.Checked = !ce.Checked;
                }
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(beDestDir.Text))
            {
                MessageBox.Show("未选择输出路径", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            List<string> dm = new List<string> 
            {
                "ZMJ",
                "00","0303","0304","0306","0402","0603","1105","1106","1108",
                "01","0101","0102","0103",
                "02","0201HJ","0201","0201K","0202HJ","0202","0202K","0203HJ","0203","0203K","0204HJ","0204","0204K",
                "03","0301HJ","0301","0301K","0302HJ","0302","0302K","0305","0307HJ","0307","0307K",
                "04","0401","0403HJ","0403","0403K","0404",
                "05","05H1","0508",
                "06","0601","0602",
                "07","0701","0702",
                "08","08H1","08H2HJ","08H2","08H2A","0809","0810HJ","0810","0810A",
                "09",
                "10","1001","1002","1003","1004","1005","1006","1007","1008","1009",
                "11","1101","1102","1103","1104HJ","1104","1104A","1104K","1107HJ","1107","1107A","1109","1110",
                "12","1201","1202","1203","1204","1205","1206","1207"
            };
            string shpField = "";
            if (radioGroup1.SelectedIndex == 0)
            {
                if (cmbField.SelectedItem == null)
                {
                    MessageBox.Show("未选择字段", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                shpField = cmbField.Text.Substring(0, cmbField.Text.IndexOf("|"));
            }
            else
            {
                if (m_SelGeo == null)
                {
                    MessageBox.Show("图形绘制错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                shpField = "tmp";
            }
            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("正在进行统计分析", "请稍等");
            wait.Show();
            #region 统计分析 生成基础表
            List<string> colArr = new List<string>() 
            {
                shpField,"ZLDWDM","QSDWDM","QSXZ","GDPDJB","GDLX","ZZSXDM","CZCSXM","TBXHDM","MSSM","FRDBS","HDMC"
            };
            //创建基础表，新建字段
            DataTable dtJCB = new DataTable();
            for (int i = 0; i < colArr.Count; i++)
            {
                dtJCB.Columns.Add(colArr[i]);
            }
            for (int i = 0; i < dm.Count; i++)
            {
                if ((dm[i].Length > 3 || dm[i] == "09") && !dm[i].Contains("HJ"))
                    dtJCB.Columns.Add(dm[i], typeof(double));
            }
            #region 进行相交分析 得到基础表
            wait.SetCaption("正在进行相交图斑提取");
            IFeatureClass pDLTB = (RCIS.Global.GlobalEditObject.GlobalWorkspace as IFeatureWorkspace).OpenFeatureClass("DLTB");
            //选择shp
            if (radioGroup1.SelectedIndex == 0)
            {
                string shpPath = txtPath.Text;
                IFeatureClass pShpClass = RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(shpPath);
                IFeatureCursor pFeaCursor = pShpClass.Search(null, true);
                IFeature pShpFeature;

                //循环shp里边的各个图形
                while ((pShpFeature = pFeaCursor.NextFeature()) != null)
                {
                    //选择shp字段的值  用于区分每个shp图形
                    string shpBH = pShpFeature.get_Value(pShpFeature.Fields.FindField(shpField)).ToString();

                    ITopologicalOperator pTopolo = pShpFeature.ShapeCopy as ITopologicalOperator;
                    //查询地类图斑中与该shp相交的图斑
                    ISpatialFilter pSF = new SpatialFilterClass();
                    pSF.Geometry = pShpFeature.ShapeCopy;
                    pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                    IFeatureCursor pCursor = pDLTB.Search(pSF, true);
                    IFeature pFeature;
                    while ((pFeature = pCursor.NextFeature()) != null)
                    {
                        //判断相交面积
                        IGeometry pIntersectGeo = pTopolo.Intersect(pFeature.ShapeCopy, esriGeometryDimension.esriGeometry2Dimension);
                        double aa2 = (pIntersectGeo as IArea).Area;
                        if ((pIntersectGeo as IArea).Area > 0)
                        {
                            string dlbm = pFeature.get_Value(pFeature.Fields.FindField("DLBM")).ToString();
                            double tbmj = double.Parse(pFeature.get_Value(pFeature.Fields.FindField("TBMJ")).ToString());
                            //根据相交面积占图斑面积比例，计算相交部分图斑地类面积
                            double mj = Math.Round(tbmj * (Math.Round((pIntersectGeo as IArea).Area, 2) / Math.Round((pFeature.ShapeCopy as IArea).Area, 2)), 2);
                            double kcxs = 0;
                            double.TryParse(pFeature.get_Value(pFeature.Fields.FindField("KCXS")).ToString(), out kcxs);
                            double kcmj = Math.Round(mj * kcxs, 2);
                            DataRow dr = dtJCB.NewRow();
                            //各个列赋值  如zldwdm qsdwdm
                            dr[0] = shpBH;
                            for (int i = 1; i < colArr.Count; i++)
                            {
                                dr[i] = pFeature.get_Value(pFeature.Fields.FindField(colArr[i])).ToString().Trim();
                            }
                            //面积赋值
                            if (dlbm.Substring(0, 2) == "01")
                            {
                                dr["1203"] = kcmj;
                                dr[dlbm] = Math.Round(mj - kcmj, 2);
                            }
                            else
                                dr[dlbm] = Math.Round(mj, 2);
                            dtJCB.Rows.Add(dr);
                        }
                        RCIS.Utility.OtherHelper.ReleaseComObject(pFeature);
                    }
                    RCIS.Utility.OtherHelper.ReleaseComObject(pCursor);
                    RCIS.Utility.OtherHelper.ReleaseComObject(pShpFeature);
                }
                RCIS.Utility.OtherHelper.ReleaseComObject(pFeaCursor);
            }
            else if (radioGroup1.SelectedIndex == 1)
            {
                //绘制地图
                if (m_SelGeo == null)
                {
                    MessageBox.Show("未绘制图形", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                ITopologicalOperator pTopolo = m_SelGeo as ITopologicalOperator;
                //查询地类图斑中与该shp相交的图斑
                ISpatialFilter pSF = new SpatialFilterClass();
                pSF.Geometry = m_SelGeo;
                pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                IFeatureCursor pCursor = pDLTB.Search(pSF, true);
                IFeature pFeature;
                while ((pFeature = pCursor.NextFeature()) != null)
                {
                    //判断相交面积
                    IGeometry pIntersectGeo = pTopolo.Intersect(pFeature.ShapeCopy, esriGeometryDimension.esriGeometry2Dimension);
                    double aa2 = (pIntersectGeo as IArea).Area;
                    if ((pIntersectGeo as IArea).Area > 0)
                    {
                        string dlbm = pFeature.get_Value(pFeature.Fields.FindField("DLBM")).ToString();
                        double tbmj = double.Parse(pFeature.get_Value(pFeature.Fields.FindField("TBMJ")).ToString());
                        //根据相交面积占图斑面积比例，计算相交部分图斑地类面积
                        double mj = Math.Round(tbmj * (Math.Round((pIntersectGeo as IArea).Area, 2) / Math.Round((pFeature.ShapeCopy as IArea).Area, 2)), 2);
                        double kcxs = 0;
                        double.TryParse(pFeature.get_Value(pFeature.Fields.FindField("KCXS")).ToString(), out kcxs);
                        double kcmj = Math.Round(mj * kcxs, 2);
                        DataRow dr = dtJCB.NewRow();
                        //各个列赋值  如zldwdm qsdwdm
                        dr[0] = "";
                        for (int i = 1; i < colArr.Count; i++)
                        {
                            dr[i] = pFeature.get_Value(pFeature.Fields.FindField(colArr[i])).ToString().Trim();
                        }
                        //面积赋值
                        if (dlbm.Substring(0, 2) == "01")
                        {
                            dr["1203"] = kcmj;
                            dr[dlbm] = Math.Round(mj - kcmj, 2);
                        }
                        else
                            dr[dlbm] = Math.Round(mj, 2);
                        dtJCB.Rows.Add(dr);
                    }
                    RCIS.Utility.OtherHelper.ReleaseComObject(pFeature);
                }
                RCIS.Utility.OtherHelper.ReleaseComObject(pCursor);
            }
            #endregion
            //对基础表进行分组求和 
            wait.SetCaption("正在对基础表进行分组求和");
            string fz = "";
            for (int i = 0; i < colArr.Count; i++)
            {
                fz += "[" + colArr[i] + "]+";
            }
            //新增一个分组字段 将zldwdm qsdwdm gdpdjb等字段的值合并到这一个字段内  按这个字段进行分组
            dtJCB.Columns.Add("FZ", typeof(string));

            dtJCB.Columns["FZ"].Expression = fz.Substring(0, fz.Length - 1);

            string[] arr = { "FZ" };
            DataView dataView = dtJCB.DefaultView;
            //获取dtJCB内 fz 字段的唯一值

            DataTable dtTmp = dataView.ToTable(true, "FZ");
            DataTable jcbClone = dtJCB.Clone();
            for (int i = 0; i < dtTmp.Rows.Count; i++)
            {
                string fzVal = dtTmp.Rows[i][0].ToString();
                DataRow drJCB = dtJCB.Select("FZ='" + fzVal + "'")[0];
                DataRow dr = jcbClone.NewRow();
                for (int j = 0; j < dtJCB.Columns.Count - 1; j++)
                {
                    if (j < colArr.Count)
                        dr[j] = drJCB[j];
                    else
                    {
                        double mj = 0;
                        //各个地类根据分组进行求和
                        double.TryParse(dtJCB.Compute("sum([" + dtJCB.Columns[j].ColumnName + "])", "FZ='" + fzVal + "'").ToString(), out mj);
                        dr[j] = mj;
                    }
                }
                jcbClone.Rows.Add(dr);
            }
            dtJCB.Dispose();
            dtJCB = null;
            jcbClone.Columns.Remove("FZ");

            if (radioGroup2.SelectedIndex != 2)
            {
                //基础表调平
                string hjPfm = "";
                for (int i = colArr.Count; i < jcbClone.Columns.Count; i++)
                {
                    hjPfm += "[" + jcbClone.Columns[i].ColumnName + "]+";
                }
                hjPfm = hjPfm.Substring(0, hjPfm.Length - 1);
                jcbClone.Columns.Add("HJ", typeof(double), hjPfm);

                double kzmj = double.Parse(jcbClone.Compute("sum(HJ)", "").ToString());
                kzmj = Math.Round(kzmj / 10000, 2);

                for (int i = 0; i < jcbClone.Rows.Count; i++)
                {
                    for (int j = colArr.Count; j < jcbClone.Columns.Count - 1; j++)
                    {
                        double mj = 0;
                        double.TryParse(jcbClone.Rows[i][j].ToString(), out mj);
                        mj = Math.Round(mj / 10000, 2);
                        jcbClone.Rows[i][j] = mj;
                    }
                }

                double zmj = double.Parse(jcbClone.Compute("sum(HJ)", "").ToString());
                zmj = Math.Round(zmj, 2);

                dataView = jcbClone.DefaultView;
                dataView.Sort = "HJ desc";
                DataTable dtTp = dataView.ToTable();
                if (zmj != kzmj)
                {
                    double tp001 = 0.01;
                    if (kzmj < zmj)
                        tp001 = -0.01;
                    double diff = Math.Abs(kzmj - zmj);
                    double num = diff / 0.01;
                    double item = Math.Floor(num / dtTp.Rows.Count);
                    double ys = num % dtTp.Rows.Count;
                    for (int i = 0; i < dtTp.Rows.Count; i++)
                    {
                        DataRow drTp = dtTp.Rows[i];
                        double tp = 0;
                        if (i < ys)
                            tp = (tp001 * item) + tp001;
                        else
                            tp = (tp001 * item);
                        if (tp == 0)
                            continue;
                        dtTmp = new DataTable();
                        dtTmp = DLTP(drTp, colArr, dtTp.Columns.Count - 2, tp);
                        for (int m = 0; m < dtTmp.Rows.Count; m++)
                        {
                            int index = int.Parse(dtTmp.Rows[m][1].ToString());
                            double mjVal = double.Parse(dtTmp.Rows[m][0].ToString());
                            drTp[index] = mjVal;
                        }
                    }
                }


                dtTp.Columns.Remove("HJ");
                jcbClone = new DataTable();
                jcbClone = dtTp;
            }
            

            //计算小计合计
            //计算00 01等的各个小计
            wait.SetCaption("正在计算小计合计");
            string str = "";
            for (int i = dm.Count - 1; i > 0; i--)
            {
                if (dm[i].Contains("HJ") || dm[i] == "09") continue;
                if (dm[i].Length == 2)
                {
                    str = str.Substring(0, str.Length - 1);
                    jcbClone.Columns.Add(dm[i], typeof(double), str);
                    str = "";
                }
                else
                {
                    str += "[" + dm[i] + "]+";
                }
            }
            //计算合计
            jcbClone.Columns.Add("0201HJ", typeof(double), "[0201]+[0201K]");
            jcbClone.Columns.Add("0202HJ", typeof(double), "[0202]+[0202K]");
            jcbClone.Columns.Add("0203HJ", typeof(double), "[0203]+[0203K]");
            jcbClone.Columns.Add("0204HJ", typeof(double), "[0204]+[0204K]");
            jcbClone.Columns.Add("0301HJ", typeof(double), "[0301]+[0301K]");
            jcbClone.Columns.Add("0302HJ", typeof(double), "[0302]+[0302K]");
            jcbClone.Columns.Add("0307HJ", typeof(double), "[0307]+[0307K]");
            jcbClone.Columns.Add("0403HJ", typeof(double), "[0403]+[0403K]");
            jcbClone.Columns.Add("08H2HJ", typeof(double), "[08H2]+[08H2A]");
            jcbClone.Columns.Add("0810HJ", typeof(double), "[0810]+[0810A]");
            jcbClone.Columns.Add("1104HJ", typeof(double), "[1104]+[1104A]+[1104K]");
            jcbClone.Columns.Add("1107HJ", typeof(double), "[1107]+[1107A]");
            jcbClone.Columns.Add("ZMJ", typeof(double), "[00]+[01]+[02]+[03]+[04]+[05]+[06]+[07]+[08]+[09]+[10]+[11]+[12]");

            for (int i = 0; i < dm.Count; i++)
            {
                jcbClone.Columns[jcbClone.Columns.IndexOf(dm[i])].SetOrdinal(i + colArr.Count);
            }

            Dictionary<string, string> dicQsdwdm = GetDMMCDicByQueryDef(RCIS.Global.GlobalEditObject.GlobalWorkspace as IFeatureWorkspace, "XZQ", "XZQDM", "XZQMC");

            Dictionary<string, string> qsdwb = GetDMMCDicByQueryDef(RCIS.Global.GlobalEditObject.GlobalWorkspace as IFeatureWorkspace, "DLTB", "QSDWDM", "QSDWMC");

            Dictionary<string, string> zldwb = GetDMMCDicByQueryDef(RCIS.Global.GlobalEditObject.GlobalWorkspace as IFeatureWorkspace, "DLTB", "ZLDWDM", "ZLDWMC");

            Dictionary<string, string> dicA = getDic(dicQsdwdm, qsdwb);

            Dictionary<string, string> dicC = getDic(dicA, zldwb);
            #endregion
            //DataRow dr = RCIS.Database.LS_SetupMDBHelper.GetDataRow("Select MC From SYS_XZQ Where DM = '" + item.Key.Substring(0, 6) + "'", "tmp");

            //DataTable dt1 = OutTableDLMJ(jcbClone, shpField, "ZLDWDM", new string[] { "08H2A", "0810A", "1104A", "1107A" });
            //DataTable dt2 = OutTableSXMJ(jcbClone, shpField, "ZLDWDM", "CZCSXM", new string[] { "201A", "202A", "203A" }, new string[] { "zmj" },new string[] {"20 = 201A + 202A + 203A"});
            //dt1.Merge(dt2, false, MissingSchemaAction.AddWithKey);

            string statisticsDW = "";
            if (radioGroup3.SelectedIndex == 0)
                statisticsDW = "ZLDWDM";
            else if (radioGroup3.SelectedIndex == 1)
                statisticsDW = "QSDWDM";

            if (chkTab1.Checked)
            {
                wait.SetCaption("正在输出土地利用现状一级分类面积汇总表...");
                //土地利用现状一级分类面积汇总表
                string[] dms = { "ZMJ", "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };
                DataTable dt = OutTableDLMJ(jcbClone, shpField, statisticsDW, dms);
                ExportTable(dt, RCIS.Global.AppParameters.TemplatePath + "\\土地利用现状一级分类面积汇总表.xlsx", beDestDir.Text, 4, dicC);
            }
            if (chkTab2.Checked)
            {
                wait.SetCaption("正在输出土地利用现状二级分类面积汇总表...");
                string[] dms = 
                {
                    "ZMJ",
                    "00","0303","0304","0306","0402","0603","1105","1106","1108",
                    "01","0101","0102","0103",
                    "02","0201HJ","0202HJ","0203HJ","0204HJ",
                    "03","0301HJ","0302HJ","0305","0307HJ",
                    "04","0401","0403HJ","0404",
                    "05","05H1","0508",
                    "06","0601","0602",
                    "07","0701","0702",
                    "08","08H1","08H2HJ","0809","0810HJ",
                    "09",
                    "10","1001","1002","1003","1004","1005","1006","1007","1008","1009",
                    "11","1101","1102","1103","1104HJ","1107HJ","1109","1110",
                    "12","1201","1202","1203","1204","1205","1206","1207"
                };
                DataTable dt = OutTableTDEJDL(jcbClone, shpField);
                dt.Columns.Remove("FZ");
                //DataTable dt = OutTableDLMJ(jcbClone, shpField, statisticsDW, dms);
                ExportTable(dt, RCIS.Global.AppParameters.TemplatePath + "\\土地利用现状二级分类面积汇总表.xlsx", beDestDir.Text, 4, dicC);
            }
            if (chkTab3.Checked)
            {
                wait.SetCaption("正在输出土地利用现状一级分类面积按权属性质汇总表...");
                //土地利用现状一级分类面积按权属性质汇总表
                DataTable dt = OutTableYJFLQSXZ(jcbClone, shpField, statisticsDW);
                ExportTable(dt, RCIS.Global.AppParameters.TemplatePath + "\\土地利用现状一级分类面积按权属性质汇总表.xlsx", beDestDir.Text, 5, dicC);

            }
            if (chkTab4.Checked)
            {
                wait.SetCaption("正在输出城镇村及工矿用地面积汇总表...");
                //城镇村及工矿用地面积汇总表
                string[] dms = 
                {
                    "ZMJ",
                    "00","0303","0304","0306","0402","0603","1105","1106","1108",
                    "01","0101","0102","0103",
                    "02","0201HJ","0202HJ","0203HJ","0204HJ",
                    "03","0301HJ","0302HJ","0305","0307HJ",
                    "04","0401","0403HJ","0404",
                    "05","05H1","0508",
                    "06","0601","0602",
                    "07","0701","0702",
                    "08","08H1","08H2HJ","0809","0810HJ",
                    "09",
                    "10","1001","1002","1003","1004","1005","1006","1007","1008","1009",
                    "11","1101","1102","1103","1104HJ","1107HJ","1109","1110",
                    "12","1201","1202","1203","1204","1205","1206","1207"
                };
                DataTable dt = OutTableSXMJ(jcbClone, shpField, statisticsDW, "CZCSXM", new string[] { "20", "201", "202", "203", "204", "205" }, dms);
                ExportTable(dt, RCIS.Global.AppParameters.TemplatePath + "\\城镇村及工矿用地面积汇总表.xlsx", beDestDir.Text, 5, dicC);

            }
            if (chkTab5.Checked)
            {
                wait.SetCaption("正在输出耕地坡度分级面积汇总表...");
                //耕地坡度分级面积汇总表
                DataTable dt = OutTableGDPD(jcbClone, shpField, statisticsDW);
                ExportTable(dt, RCIS.Global.AppParameters.TemplatePath + "\\耕地坡度分级面积汇总表.xlsx", beDestDir.Text, 5, dicC);
            }
            if (chkTab6.Checked)
            {
                wait.SetCaption("正在输出耕地种植类型面积统计表...");
                //耕地种植类型面积统计表
                DataTable dt = OutTableSXMJ(jcbClone, shpField, statisticsDW, "ZZSXDM", new string[] { "LS", "FLS", "LYFL", "XG", "LLJZ", "WG" }, new string[] { "01", "0101", "0102", "0103" }, new string[] { "HJ = LS + FLS + LYFL + XG + LLJZ + WG" }, true);
                ExportTable(dt, RCIS.Global.AppParameters.TemplatePath + "\\耕地种植类型面积统计表.xlsx", beDestDir.Text, 4, dicC);
            }
            if (chkTab7.Checked)
            {
                wait.SetCaption("正在输出林区范围内园地汇总统计表...");
                //林区范围内园地汇总统计表
                DataTable dt = OutTableSXMJ(jcbClone, shpField, statisticsDW, "TBXHDM", new string[] { "LQYD" }, new string[] { "02", "0201HJ", "0202HJ", "0203HJ", "0204HJ" });
                ExportTable(dt, RCIS.Global.AppParameters.TemplatePath + "\\林区范围内园地汇总统计表.xlsx", beDestDir.Text, 4, dicC);
            }
            if (chkTab8.Checked)
            {
                wait.SetCaption("正在输出灌丛草地汇总情况统计表...");
                //灌丛草地汇总情况统计表
                DataTable dt = OutTableSXMJ2(jcbClone, shpField, statisticsDW, "TBXHDM", new string[] { "GCCD" }, new string[] { "0401", "0402", "0403HJ", "0404" }, new string[] { "HJ = 0401 + 0402 + 0403HJ + 0404" });
                ExportTable(dt, RCIS.Global.AppParameters.TemplatePath + "\\灌丛草地汇总情况统计表.xlsx", beDestDir.Text, 4, dicC);

            }
            if (chkTab9.Checked)
            {
                wait.SetCaption("正在输出工业用地按类型汇总统计表...");
                //工业用地按类型汇总统计表
                DataTable dt = OutTableSXMJ(jcbClone, shpField, statisticsDW, "TBXHDM", new string[] { "HDGY", "GTGY", "MTGY", "SNGY", "BLGY", "DLGY" }, new string[] { "0601" }, new string[] { "HJ = HDGY + GTGY + MTGY + SNGY + BLGY + DLGY" }, true);
                ExportTable(dt, RCIS.Global.AppParameters.TemplatePath + "\\工业用地按类型汇总统计表.xlsx", beDestDir.Text, 4, dicC);

            }
            if (chkTab10.Checked)
            {
                wait.SetCaption("正在输出可调整地类面积汇总表...");
                //可调整地类面积汇总表
                DataTable dt = OutTableDLMJ(jcbClone, shpField, statisticsDW, new string[] { "0201K", "0202K", "0203K", "0204K", "0301K", "0302K", "0307K", "0403K", "1104K" },
                                "HJ = 0201K + 0202K + 0203K + 0204K + 0301K + 0302K + 0307K + 0403K + 1104K");
                ExportTable(dt, RCIS.Global.AppParameters.TemplatePath + "\\可调整地类面积汇总表.xlsx", beDestDir.Text, 4, dicC);
            }
            if (chkTab11.Checked)
            {
                wait.SetCaption("正在输出部分细化地类面积汇总表...");
                //部分细化地类面积汇总表
                DataTable dt = OutTableBFXHDL(jcbClone, shpField, statisticsDW);
                ExportTable(dt, RCIS.Global.AppParameters.TemplatePath + "\\部分细化地类面积汇总表.xlsx", beDestDir.Text, 4, dicC);

            }
            if (chkTab12.Checked)
            {
                wait.SetCaption("正在输出耕地细化调查情况统计表...");
                //耕地细化调查情况统计表
                DataTable dt1 = OutTableSXMJ(jcbClone, shpField, statisticsDW, "TBXHDM", new string[] { "HDGD", "HQGD", "LQGD", "MQGD", "SHGD", "SMGD", "YJGD" },
                                new string[] { "01", "0101", "0102", "0103" }, new string[] { "HJ = HDGD + HQGD + LQGD + MQGD + SHGD + SMGD + YJGD" }, false, sumType.SXAndFirstDL);
                ExportTable(dt1, RCIS.Global.AppParameters.TemplatePath + "\\耕地细化调查情况统计表.xlsx", beDestDir.Text, 4, dicC);
            }
            if (chkFRD1.Checked)
            {
                wait.SetCaption("正在输出飞入地土地利用现状一级分类面积汇总表...");
                //飞入地土地利用现状一级分类面积汇总表
                string[] dms = { "ZMJ", "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };
                DataTable dt = OutTableFRDMJ(jcbClone, shpField, statisticsDW, dms);
                ExportTableFRD(dt, RCIS.Global.AppParameters.TemplatePath + "\\飞入地土地利用现状一级分类面积汇总表.xlsx", beDestDir.Text, 4, dicC);
            }
            if (chkFrd2.Checked)
            {
                wait.SetCaption("正在输出飞入地土地利用现状二级分类面积汇总表...");
                //飞入地土地利用现状二级分类面积汇总表
                string[] dms = 
                {
                    "ZMJ",
                    "00","0303","0304","0306","0402","0603","1105","1106","1108",
                    "01","0101","0102","0103",
                    "02","0201HJ","0202HJ","0203HJ","0204HJ",
                    "03","0301HJ","0302HJ","0305","0307HJ",
                    "04","0401","0403HJ","0404",
                    "05","05H1","0508",
                    "06","0601","0602",
                    "07","0701","0702",
                    "08","08H1","08H2HJ","0809","0810HJ",
                    "09",
                    "10","1001","1002","1003","1004","1005","1006","1007","1008","1009",
                    "11","1101","1102","1103","1104HJ","1107HJ","1109","1110",
                    "12","1201","1202","1203","1204","1205","1206","1207"
                };
                DataTable dt = OutTableFRDMJ(jcbClone, shpField, statisticsDW, dms);
                ExportTableFRD(dt, RCIS.Global.AppParameters.TemplatePath + "\\飞入地土地利用现状二级分类面积汇总表.xlsx", beDestDir.Text, 4, dicC);

            }
            if (chkFrd3.Checked)
            {
                wait.SetCaption("正在输出飞入地土地利用现状一级分类面积按权属性质汇总表...");
                //飞入地土地利用现状一级分类面积按权属性质汇总表
                DataTable dt = OutTableFRDYJFLQSXZ(jcbClone, shpField);
                ExportTableFRD(dt, RCIS.Global.AppParameters.TemplatePath + "\\飞入地土地利用现状一级分类面积按权属性质汇总表.xlsx", beDestDir.Text, 4, dicC);

            }
            if (chkFrd4.Checked)
            {
                wait.SetCaption("正在输出飞入地城镇村及工矿用地面积汇总表...");
                //城镇村及工矿用地面积汇总表
                string[] dms = 
                {
                    "ZMJ",
                    "00","0303","0304","0306","0402","0603","1105","1106","1108",
                    "01","0101","0102","0103",
                    "02","0201HJ","0202HJ","0203HJ","0204HJ",
                    "03","0301HJ","0302HJ","0305","0307HJ",
                    "04","0401","0403HJ","0404",
                    "05","05H1","0508",
                    "06","0601","0602",
                    "07","0701","0702",
                    "08","08H1","08H2HJ","0809","0810HJ",
                    "09",
                    "10","1001","1002","1003","1004","1005","1006","1007","1008","1009",
                    "11","1101","1102","1103","1104HJ","1107HJ","1109","1110",
                    "12","1201","1202","1203","1204","1205","1206","1207"
                };
                DataTable dt = OutTableSXMJCZC(jcbClone, shpField, statisticsDW, "CZCSXM", new string[] { "20", "201", "202", "203", "204", "205" }, dms);
                ExportTableFRD(dt, RCIS.Global.AppParameters.TemplatePath + "\\飞入地城镇村及工矿用地面积汇总表.xlsx", beDestDir.Text, 5, dicC);

            }
            if (chkHd1.Checked)
            {
                wait.SetCaption("正在输出海岛土地利用现状一级分类面积汇总表...");
                //海岛土地利用现状一级分类面积汇总表
                string[] dms = { "ZMJ", "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };
                DataTable dt = OutTableHDMJ(jcbClone, shpField, statisticsDW, dms);
                ExportTableHD(dt, RCIS.Global.AppParameters.TemplatePath + "\\海岛土地利用现状一级分类面积汇总表.xlsx", beDestDir.Text, 4, dicC);
            }
            if (chkHd2.Checked)
            {
                wait.SetCaption("正在输出海岛土地利用现状二级分类面积汇总表...");
                //海岛土地利用现状二级分类面积汇总表
                string[] dms = 
                {
                    "ZMJ",
                    "00","0303","0304","0306","0402","0603","1105","1106","1108",
                    "01","0101","0102","0103",
                    "02","0201HJ","0202HJ","0203HJ","0204HJ",
                    "03","0301HJ","0302HJ","0305","0307HJ",
                    "04","0401","0403HJ","0404",
                    "05","05H1","0508",
                    "06","0601","0602",
                    "07","0701","0702",
                    "08","08H1","08H2HJ","0809","0810HJ",
                    "09",
                    "10","1001","1002","1003","1004","1005","1006","1007","1008","1009",
                    "11","1101","1102","1103","1104HJ","1107HJ","1109","1110",
                    "12","1201","1202","1203","1204","1205","1206","1207"
                };
                DataTable dt = OutTableHDMJ(jcbClone, shpField, statisticsDW, dms);
                ExportTableHD(dt, RCIS.Global.AppParameters.TemplatePath + "\\海岛土地利用现状二级分类面积汇总表.xlsx", beDestDir.Text, 4, dicC);

            }
            if (chkGDHF.Checked)
            {
                wait.SetCaption("正在输出即可恢复与工程恢复种植属性汇总统计表...");
                //即可恢复与工程恢复种植属性汇总统计表
                DataTable dt = OutTableSXMJ(jcbClone, shpField, statisticsDW, "TBXHDM", new string[] { "JKHF", "GCHF" },
                                new string[] { "0201HJ", "0201K", "0202HJ", "0202K", "0203HJ", "0203K", "0204HJ", "0204K", "0301HJ", "0301K", "0302HJ", "0302K", "0305", "0307HJ", "0307K", "0403K", "0404", "1104HJ", "1104K" },
                                new string[] { "HJ = 0201HJ + 0201K + 0202HJ + 0202K + 0203HJ + 0203K + 0204HJ + 0204K + 0301HJ + 0301K + 0302HJ + 0302K + 0305 + 0307HJ + 0307K + 0403K + 0404 + 1104HJ + 1104K" }, false, sumType.DL);
                dt.Columns.Add("HJ", typeof(double), "JKHFHJ + GCHFHJ");
                dt.Columns["HJ"].SetOrdinal(2);
                ExportTable(dt, RCIS.Global.AppParameters.TemplatePath + "\\即可恢复与工程恢复种植属性汇总统计表.xlsx", beDestDir.Text, 4, dicC);

            }
            if (chkFQLJTM.Checked)
            {
                wait.SetCaption("正在输出废弃与垃圾填埋细化标注汇总统计表...");
                //废弃与垃圾填埋细化标注汇总统计表
                DataTable dt = OutTableFQLJTM(jcbClone, shpField, statisticsDW);
                ExportTable(dt, RCIS.Global.AppParameters.TemplatePath + "\\废弃与垃圾填埋细化标注汇总统计表.xlsx", beDestDir.Text, 4, dicC);

            }
            wait.Close();
            MessageBox.Show("分析完毕", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        enum sumType
        {
            SX, DL, SXAndFirstDL
        }
        /// <summary>
        /// 根据基础表中一个属性字段的多个值和地类组成生成统计表
        /// </summary>
        /// <param name="dtJCB">基础表</param>
        /// <param name="shpField">外部文件的字段</param>
        /// <param name="groupField">分组字段，目前是行政区代码ZLDWDM或者权属代码QSDWDM</param>
        /// <param name="sxField">统计属性字段，如czcsxm</param>
        /// <param name="sxValues">统计属性对应的内容，如{"201A","202A","203A"}</param>
        /// <param name="staFields">统计面积的字段数组，如{"0101","0102","0103"}</param>
        /// <param name="sumFields">需要计算合计的字段，如{"20 = 201A + 202A + 203A"}，前面是原来计算中没有的字段，后面是计算中有的字段</param>
        /// <returns></returns>
        private DataTable OutTableSXMJ(DataTable dtJCB, string shpField, string groupField, string sxField, string[] sxValues, string[] staFields, string[] sumFields = null, Boolean changeOrder = false, sumType sumtype = sumType.SX)
        {
            DataTable dtResult = new DataTable();
            dtResult.Columns.Add(shpField, typeof(string));
            dtResult.Columns.Add(groupField, typeof(string));
            if (changeOrder == false)
            {
                foreach (string val in sxValues)
                {
                    foreach (string item in staFields)
                    {
                        dtResult.Columns.Add(val + item, typeof(double));
                    }
                }
            }
            else
            {
                foreach (string item in staFields)
                {
                    foreach (string val in sxValues)
                    {
                        dtResult.Columns.Add(item + val, typeof(double));
                    }
                }
            }
            var lstshp = dtJCB.AsEnumerable().GroupBy(a => a.Field<string>(shpField)).ToList();
            var lstGroup = dtJCB.AsEnumerable().GroupBy(a => a.Field<string>(groupField)).ToList().
                Union(dtJCB.AsEnumerable().GroupBy(a => a.Field<string>(groupField).Substring(0, 9)).ToList()).
                Union(dtJCB.AsEnumerable().GroupBy(a => a.Field<string>(groupField).Substring(0, 6)).ToList());
            string dwdm = "";
            foreach (var ishp in lstshp)
            {
                foreach (var igroup in lstGroup)
                {
                    if (igroup.Key.Length == 6)
                        dwdm = igroup.Key;
                    DataRow dr = dtResult.NewRow();
                    if (changeOrder == false)
                    {
                        foreach (string val in sxValues)
                        {
                            foreach (string item in staFields)
                            {
                                dr[shpField] = ishp.Key;
                                dr[groupField] = igroup.Key;
                                object o = dtJCB.Compute("Sum([" + item + "])", shpField + " = '" + ishp.Key + "' And " + groupField + " Like '" + igroup.Key + "%' And " + sxField + " Like '" + val + "%'");
                                double mj = 0;
                                double.TryParse(o.ToString(), out mj);
                                dr[val + item] = mj;
                            }
                        }
                    }
                    else
                    {
                        foreach (string item in staFields)
                        {
                            foreach (string val in sxValues)
                            {
                                dr[shpField] = ishp.Key;
                                dr[groupField] = igroup.Key;
                                object o = dtJCB.Compute("Sum([" + item + "])", shpField + " = '" + ishp.Key + "' And " + groupField + " Like '" + igroup.Key + "%' And " + sxField + " Like '" + val + "%'");
                                double mj = 0;
                                double.TryParse(o.ToString(), out mj);
                                dr[item + val] = mj;
                            }
                        }
                    }
                    dtResult.Rows.Add(dr);
                }
            }
            if (sumFields != null)
            {
                switch (sumtype)
                {
                    case sumType.SX:
                        foreach (string item in sumFields)
                        {
                            string sumField = item.Split('=')[0].Trim();
                            string subField = item.Split('=')[1].Trim();
                            string[] subFields = subField.Split('+');

                            foreach (string val in staFields)
                            {
                                List<string> flds = new List<string>();
                                foreach (string fld in subFields)
                                {
                                    if (changeOrder == false)
                                        flds.Add("[" + fld.Trim() + val + "]");
                                    else
                                        flds.Add("[" + val + fld.Trim() + "]");
                                }

                                if (changeOrder == false)
                                {
                                    dtResult.Columns.Add(sumField + val, typeof(double), string.Join(" + ", flds));
                                    dtResult.Columns[sumField + val].SetOrdinal(dtResult.Columns.IndexOf(subFields[0].Trim() + val));
                                }
                                else
                                {
                                    dtResult.Columns.Add(val + sumField, typeof(double), string.Join(" + ", flds));
                                    dtResult.Columns[val + sumField].SetOrdinal(dtResult.Columns.IndexOf(val + subFields[0].Trim()));
                                }
                            }
                        }
                        break;
                    case sumType.DL:
                        foreach (string item in sumFields)
                        {
                            string sumField = item.Split('=')[0].Trim();
                            string subField = item.Split('=')[1].Trim();
                            string[] subFields = subField.Split('+');

                            foreach (string val in sxValues)
                            {
                                List<string> flds = new List<string>();
                                foreach (string fld in subFields)
                                {
                                    if (changeOrder == false)
                                        flds.Add("[" + val + fld.Trim() + "]");
                                    else
                                        flds.Add("[" + fld.Trim() + val + "]");
                                }

                                if (changeOrder == false)
                                {
                                    dtResult.Columns.Add(val + sumField, typeof(double), string.Join(" + ", flds));
                                    dtResult.Columns[val + sumField].SetOrdinal(dtResult.Columns.IndexOf(val + subFields[0].Trim()));
                                }
                                else
                                {
                                    dtResult.Columns.Add(sumField + val, typeof(double), string.Join(" + ", flds));
                                    dtResult.Columns[sumField + val].SetOrdinal(dtResult.Columns.IndexOf(subFields[0].Trim() + val));
                                }
                            }
                        }
                        break;
                    case sumType.SXAndFirstDL:
                        foreach (string item in sumFields)
                        {
                            string sumField = item.Split('=')[0].Trim();
                            string subField = item.Split('=')[1].Trim();
                            string[] subFields = subField.Split('+');
                            string val = staFields[0];

                            List<string> flds = new List<string>();
                            foreach (string fld in subFields)
                            {
                                if (changeOrder == false)
                                    flds.Add("[" + fld.Trim() + val + "]");
                                else
                                    flds.Add("[" + val + fld.Trim() + "]");
                            }

                            if (changeOrder == false)
                            {
                                dtResult.Columns.Add(sumField + val, typeof(double), string.Join(" + ", flds));
                                dtResult.Columns[sumField + val].SetOrdinal(dtResult.Columns.IndexOf(subFields[0].Trim() + val));
                            }
                            else
                            {
                                dtResult.Columns.Add(val + sumField, typeof(double), string.Join(" + ", flds));
                                dtResult.Columns[val + sumField].SetOrdinal(dtResult.Columns.IndexOf(val + subFields[0].Trim()));
                            }
                        }
                        break;
                    default:
                        break;
                }

            }
            DataView dv = new DataView(dtResult);
            dv.Sort = groupField;
            dtResult = dv.ToTable();
            //dtResult.PrimaryKey = new DataColumn[] { dtResult.Columns[groupField] };

            if (lstshp.Count > 1 && !string.IsNullOrWhiteSpace(dwdm))
            {
                DataRow dr = dtResult.NewRow();
                foreach (var item in staFields)
                {
                    dr[shpField] = "合计";
                    dr[groupField] = dwdm;
                    object o = dtResult.Compute("Sum([" + item + "])", "" + groupField + "='" + dwdm + "'");
                    double mj = 0;
                    double.TryParse(o.ToString(), out mj);
                    dr[item] = mj;
                }
                dtResult.Rows.InsertAt(dr, 0);
            }

            return dtResult;
        }
        /// <summary>
        /// 直接根据基础表中的地类字段进行汇总的统计表
        /// </summary>
        /// <param name="dtJCB">统计基础表</param>
        /// <param name="shpField">外部文件的字段</param>
        /// <param name="groupField">分组字段，目前是行政区代码ZLDWDM或者权属代码QSDWDM</param>
        /// <param name="staFields">统计面积的字段数组如{"0101","0102","0103"}</param>
        /// <returns></returns>
        private DataTable OutTableDLMJ(DataTable dtJCB, string shpField, string groupField, string[] staFields, string sumExpress = "")
        {
            DataTable dtResult = new DataTable();
            dtResult.Columns.Add(shpField, typeof(string));
            dtResult.Columns.Add(groupField, typeof(string));
            foreach (string item in staFields)
            {
                dtResult.Columns.Add(item, typeof(double));
            }
            var lstshp = dtJCB.AsEnumerable().GroupBy(a => a.Field<string>(shpField)).ToList();
            var lstGroup = dtJCB.AsEnumerable().GroupBy(a => a.Field<string>(groupField)).ToList().
                Union(dtJCB.AsEnumerable().GroupBy(a => a.Field<string>(groupField).Substring(0, 9)).ToList()).
                Union(dtJCB.AsEnumerable().GroupBy(a => a.Field<string>(groupField).Substring(0, 6)).ToList());
            string dwdm = "";
            foreach (var ishp in lstshp)
            {
                foreach (var igroup in lstGroup)
                {
                    if (igroup.Key.Length == 6)
                        dwdm = igroup.Key;
                    DataRow dr = dtResult.NewRow();
                    foreach (var item in staFields)
                    {
                        dr[shpField] = ishp.Key;
                        dr[groupField] = igroup.Key;
                        object o = dtJCB.Compute("Sum([" + item + "])", shpField + " = '" + ishp.Key + "' And " + groupField + " Like '" + igroup.Key + "%'");
                        double mj = 0;
                        double.TryParse(o.ToString(), out mj);
                        dr[item] = mj;
                    }
                    dtResult.Rows.Add(dr);
                }
            }
            if (!string.IsNullOrWhiteSpace(sumExpress))
            {
                string sumField = sumExpress.Split('=')[0].Trim();
                string subField = sumExpress.Split('=')[1].Trim();
                string[] subFields = subField.Split('+');
                List<string> flds = new List<string>();
                foreach (string fld in subFields)
                {
                    flds.Add("[" + fld.Trim() + "]");
                }
                dtResult.Columns.Add(sumField, typeof(double), string.Join(" + ", flds));
                dtResult.Columns[sumField].SetOrdinal(dtResult.Columns.IndexOf(subFields[0].Trim()));
            }

            DataView dv = new DataView(dtResult);
            dv.Sort = groupField;
            dtResult = dv.ToTable();
            //dtResult.PrimaryKey = new DataColumn[] { dtResult.Columns[groupField] };

            if (lstshp.Count > 1 && !string.IsNullOrWhiteSpace(dwdm))
            {
                DataRow dr = dtResult.NewRow();
                foreach (var item in staFields)
                {
                    dr[shpField] = "合计";
                    dr[groupField] = dwdm;
                    object o = dtJCB.Compute("Sum([" + item + "])", "");
                    double mj = 0;
                    double.TryParse(o.ToString(), out mj);
                    dr[item] = mj;
                }
                dtResult.Rows.InsertAt(dr, 0);
            }
            return dtResult;
        }

        /// <summary>
        /// 根据基础表中一个属性字段的多个值和地类组成生成统计表
        /// </summary>
        /// <param name="dtJCB">基础表</param>
        /// <param name="shpField">外部文件的字段</param>
        /// <param name="groupField">分组字段，目前是行政区代码ZLDWDM或者权属代码QSDWDM</param>
        /// <param name="sxField">统计属性字段，如czcsxm</param>
        /// <param name="sxValues">统计属性对应的内容，如{"201A","202A","203A"}</param>
        /// <param name="staFields">统计面积的字段数组，如{"0101","0102","0103"}</param>
        /// <param name="sumFields">需要计算合计的字段，如{"HJ = 0101 + 0102 + 0103"}，前面是原来计算中没有的字段，后面是计算中有的字段</param>
        /// <returns></returns>
        private DataTable OutTableSXMJ2(DataTable dtJCB, string shpField, string groupField, string sxField, string[] sxValues, string[] staFields, string[] sumFields = null, Boolean changeOrder = false)
        {
            DataTable dtResult = new DataTable();
            dtResult.Columns.Add(shpField, typeof(string));
            dtResult.Columns.Add(groupField, typeof(string));
            if (changeOrder == false)
            {
                foreach (string val in sxValues)
                {
                    foreach (string item in staFields)
                    {
                        dtResult.Columns.Add(val + item, typeof(double));
                    }
                }
            }
            else
            {
                foreach (string item in staFields)
                {
                    foreach (string val in sxValues)
                    {
                        dtResult.Columns.Add(item + val, typeof(double));
                    }
                }
            }
            var lstshp = dtJCB.AsEnumerable().GroupBy(a => a.Field<string>(shpField)).ToList();
            var lstGroup = dtJCB.AsEnumerable().GroupBy(a => a.Field<string>(groupField)).ToList().
                Union(dtJCB.AsEnumerable().GroupBy(a => a.Field<string>(groupField).Substring(0, 9)).ToList()).
                Union(dtJCB.AsEnumerable().GroupBy(a => a.Field<string>(groupField).Substring(0, 6)).ToList());
            string dwdm = "";
            foreach (var ishp in lstshp)
            {
                foreach (var igroup in lstGroup)
                {
                    if (igroup.Key.Length == 6)
                        dwdm = igroup.Key;
                    DataRow dr = dtResult.NewRow();
                    if (changeOrder == false)
                    {
                        foreach (string val in sxValues)
                        {
                            foreach (string item in staFields)
                            {
                                dr[shpField] = ishp.Key;
                                dr[groupField] = igroup.Key;
                                object o = dtJCB.Compute("Sum([" + item + "])", shpField + " = '" + ishp.Key + "' And " + groupField + " Like '" + igroup.Key + "%' And " + sxField + " Like '" + val + "%'");
                                double mj = 0;
                                double.TryParse(o.ToString(), out mj);
                                dr[val + item] = mj;
                            }
                        }
                    }
                    else
                    {
                        foreach (string item in staFields)
                        {
                            foreach (string val in sxValues)
                            {
                                dr[shpField] = ishp.Key;
                                dr[groupField] = igroup.Key;
                                object o = dtJCB.Compute("Sum([" + item + "])", shpField + " = '" + ishp.Key + "' And " + groupField + " Like '" + igroup.Key + "%' And " + sxField + " Like '" + val + "%'");
                                double mj = 0;
                                double.TryParse(o.ToString(), out mj);
                                dr[item + val] = mj;
                            }
                        }
                    }
                    dtResult.Rows.Add(dr);
                }
            }
            if (sumFields != null)
            {
                foreach (string item in sumFields)
                {
                    string sumField = item.Split('=')[0].Trim();
                    string subField = item.Split('=')[1].Trim();
                    string[] subFields = subField.Split('+');

                    foreach (string val in sxValues)
                    {
                        List<string> flds = new List<string>();
                        foreach (string fld in subFields)
                        {
                            if (changeOrder == false)
                                flds.Add("[" + val + fld.Trim() + "]");
                            else
                                flds.Add("[" + fld.Trim() + val + "]");
                        }

                        if (changeOrder == false)
                        {
                            dtResult.Columns.Add(val + sumField, typeof(double), string.Join(" + ", flds));
                            dtResult.Columns[val + sumField].SetOrdinal(dtResult.Columns.IndexOf(val + subFields[0].Trim()));
                        }
                        else
                        {
                            dtResult.Columns.Add(sumField + val, typeof(double), string.Join(" + ", flds));
                            dtResult.Columns[sumField + val].SetOrdinal(dtResult.Columns.IndexOf(subFields[0].Trim() + val));
                        }
                    }

                }
            }
            DataView dv = new DataView(dtResult);
            dv.Sort = groupField;
            dtResult = dv.ToTable();
            //dtResult.PrimaryKey = new DataColumn[] { dtResult.Columns[groupField] };

            if (lstshp.Count > 1 && !string.IsNullOrWhiteSpace(dwdm))
            {
                DataRow dr = dtResult.NewRow();
                foreach (var item in staFields)
                {
                    dr[shpField] = "合计";
                    dr[groupField] = dwdm;
                    object o = dtResult.Compute("Sum([" + item + "])", "" + groupField + "='" + dwdm + "'");
                    double mj = 0;
                    double.TryParse(o.ToString(), out mj);
                    dr[item] = mj;
                }
                dtResult.Rows.InsertAt(dr, 0);
            }

            return dtResult;
        }

        private DataTable OutTableHDMJ(DataTable dtJCB, string shpField, string groupField, string[] staFields)
        {
            DataTable dtResult = new DataTable();
            dtResult.Columns.Add(shpField, typeof(string));
            dtResult.Columns.Add("XH", typeof(string));
            dtResult.Columns.Add(groupField, typeof(string));
            dtResult.Columns.Add("HDMC", typeof(string));
            foreach (string item in staFields)
            {
                dtResult.Columns.Add(item, typeof(double));
            }
            var lstshp = dtJCB.AsEnumerable().GroupBy(a => a.Field<string>(shpField)).ToList();
            var lstGroup = dtJCB.AsEnumerable().GroupBy(a => a.Field<string>(groupField)).ToList().
                Union(dtJCB.AsEnumerable().GroupBy(a => a.Field<string>(groupField).Substring(0, 9)).ToList()).
                Union(dtJCB.AsEnumerable().GroupBy(a => a.Field<string>(groupField).Substring(0, 6)).ToList());
            var lsthd = dtJCB.AsEnumerable().GroupBy(a => a.Field<string>("HDMC")).ToList();
            string dwdm = "";
            int n = 1;
            foreach (var ishp in lstshp)
            {
                foreach (var igroup in lstGroup)
                {
                    foreach (var hd in lsthd)
                    {
                        if (igroup.Key.Length == 6)
                            dwdm = igroup.Key;
                        DataRow dr = dtResult.NewRow();
                        foreach (var item in staFields)
                        {
                            dr["HDMC"] = hd.Key;
                            dr[shpField] = ishp.Key;
                            dr[groupField] = igroup.Key;
                            object o = dtJCB.Compute("Sum([" + item + "])", shpField + " = '" + ishp.Key + "' And " + groupField + " Like '" + igroup.Key + "%' And MSSM = '01' And HDMC = '" + hd.Key + "'");
                            double mj = 0;
                            double.TryParse(o.ToString(), out mj);
                            dr[item] = mj;
                        }
                        dtResult.Rows.Add(dr);
                    }
                }
            }

            DataView dv = new DataView(dtResult);
            dv.Sort = groupField;
            dtResult = dv.ToTable();
            for (int i = 0; i < dtResult.Rows.Count; i++)
            {
                dtResult.Rows[i]["XH"] = i + 1;
            }
            //dtResult.PrimaryKey = new DataColumn[] { dtResult.Columns[groupField] };

            if (lstshp.Count > 1 && !string.IsNullOrWhiteSpace(dwdm))
            {
                DataRow dr = dtResult.NewRow();
                foreach (var item in staFields)
                {
                    dr[shpField] = "合计";
                    dr[groupField] = dwdm;
                    object o = dtJCB.Compute("Sum([" + item + "])", "");
                    double mj = 0;
                    double.TryParse(o.ToString(), out mj);
                    dr[item] = mj;
                }
                dtResult.Rows.InsertAt(dr, 0);
            }
            return dtResult;
        }


        private DataTable OutTableTDEJDL(DataTable dtJcb, string shpField)
        {
            string tjdw = "";
            if (radioGroup3.SelectedIndex == 0)
                tjdw = "ZLDWDM";
            else if (radioGroup3.SelectedIndex == 1)
                tjdw = "QSDWDM";

            string[] StatisticsCol = { shpField, tjdw };
            string[] dm = 
            {
                "ZMJ",
                "00","0303","0304","0306","0402","0603","1105","1106","1108",
                "01","0101","0102","0103",
                "02","0201HJ","0202HJ","0203HJ","0204HJ",
                "03","0301HJ","0302HJ","0305","0307HJ",
                "04","0401","0403HJ","0404",
                "05","05H1","0508",
                "06","0601","0602",
                "07","0701","0702",
                "08","08H1","08H2HJ","0809","0810HJ",
                "09",
                "10","1001","1002","1003","1004","1005","1006","1007","1008","1009",
                "11","1101","1102","1103","1104HJ","1107HJ","1109","1110",
                "12","1201","1202","1203","1204","1205","1206","1207"
            };
            string[] arr = new string[StatisticsCol.Length + dm.Length];
            StatisticsCol.CopyTo(arr, 0);
            dm.CopyTo(arr, StatisticsCol.Length);
            DataTable dt = dtJcb.DefaultView.ToTable(false, arr);
            string filterStr = "";
            for (int i = 0; i < StatisticsCol.Length; i++)
            {
                filterStr += "" + StatisticsCol[i] + "+";
            }
            dt.Columns.Add("FZ", typeof(string), filterStr.Substring(0, filterStr.Length - 1));
            DataTable dtTmp = dt.DefaultView.ToTable(true, "FZ");
            DataTable dtResult = dt.Clone();
            List<string> xiandm = new List<string>();
            DataRow drHj = dtResult.NewRow();
            Dictionary<string, int> hjCoutn = new Dictionary<string, int>();
            for (int i = 0; i < dtTmp.Rows.Count; i++)
            {
                string fzVal = dtTmp.Rows[i][0].ToString();
                if (string.IsNullOrWhiteSpace(fzVal)) continue;

                DataRow drJCB = dt.Select("FZ='" + fzVal + "'")[0];
                DataRow dr = dtResult.NewRow();
                for (int j = 0; j < dt.Columns.Count - 1; j++)
                {
                    if (j < StatisticsCol.Length)
                        dr[j] = drJCB[j];
                    else
                    {
                        double mj = 0;
                        //各个地类根据分组进行求和
                        double.TryParse(dt.Compute("sum([" + dt.Columns[j].ColumnName + "])", "FZ='" + fzVal + "'").ToString(), out mj);
                        dr[j] = mj;
                    }
                }
                dtResult.Rows.Add(dr);

                //添加乡镇行
                //fzVal = fzVal.Substring(0, fzVal.Length - 10);
                //if (!xiandm.Contains(fzVal))
                //{
                //    dr = dtResult.NewRow();
                //    for (int j = 0; j < dt.Columns.Count - 1; j++)
                //    {
                //        if (j < StatisticsCol.Length)
                //        {
                //            if (StatisticsCol[j] == tjdw)
                //                dr[j] = drJCB[j].ToString().Substring(0, drJCB[j].ToString().Length - 10);
                //            else
                //                dr[j] = drJCB[j];
                //        }
                //        else
                //        {
                //            double mj = 0;
                //            //各个地类根据分组进行求和
                //            double.TryParse(dt.Compute("sum([" + dt.Columns[j].ColumnName + "])", "FZ like '" + fzVal + "%'").ToString(), out mj);
                //            dr[j] = mj;
                //        }
                //    }
                //    dtResult.Rows.Add(dr);
                //    xiandm.Add(fzVal);
                //}

                //添加区县行
                fzVal = fzVal.Substring(0, fzVal.Length - 19);

                if (hjCoutn.Keys.Contains(fzVal))
                    hjCoutn[fzVal] = hjCoutn[fzVal] + 1;
                else
                    hjCoutn.Add(fzVal, 1);

                //if (!xiandm.Contains(fzVal))
                //{
                //    dr = dtResult.NewRow();
                //    for (int j = 0; j < dt.Columns.Count - 1; j++)
                //    {
                //        if (j < StatisticsCol.Length)
                //        {
                //            if (StatisticsCol[j] == tjdw)
                //                dr[j] = drJCB[j].ToString().Substring(0, drJCB[j].ToString().Length - 13);
                //            else
                //                dr[j] = drJCB[j];
                //        }
                //        else
                //        {
                //            double mj = 0;
                //            //各个地类根据分组进行求和
                //            double.TryParse(dt.Compute("sum([" + dt.Columns[j].ColumnName + "])", "FZ like '" + fzVal + "%'").ToString(), out mj);
                //            dr[j] = mj;
                //        }
                //    }
                //    dtResult.Rows.Add(dr);
                //    xiandm.Add(fzVal);
                //}
                //添加总的合计
                if (i == 0)
                {
                    drHj = dtResult.NewRow();
                    for (int j = 0; j < dt.Columns.Count - 1; j++)
                    {
                        if (j < StatisticsCol.Length)
                        {
                            if (StatisticsCol[j] == tjdw)
                                drHj[j] = drJCB[j].ToString().Substring(0, drJCB[j].ToString().Length - 13);
                            else if (StatisticsCol[j] == shpField)
                                drHj[j] = "合计";
                            else
                                drHj[j] = drJCB[j];
                        }
                        else
                        {
                            double mj = 0;
                            //各个地类根据分组进行求和
                            double.TryParse(dt.Compute("sum([" + dt.Columns[j].ColumnName + "])", "").ToString(), out mj);
                            drHj[j] = mj;
                        }
                    }
                    //dtResult.Rows.Add(dr);
                }
            }

            foreach (string item in hjCoutn.Keys)
            {
                if (hjCoutn[item] == 1) continue;
                
                DataRow dr = dtResult.NewRow();
                for (int j = 0; j < dt.Columns.Count - 1; j++)
                {
                    if (j < StatisticsCol.Length)
                    {
                        if (StatisticsCol[j] == tjdw)
                            dr[j] = "合计";
                        else
                            dr[j] = item;
                    }
                    else
                    {
                        double mj = 0;
                        //各个地类根据分组进行求和
                        double.TryParse(dt.Compute("sum([" + dt.Columns[j].ColumnName + "])", ""+shpField+" = '" + item + "'").ToString(), out mj);
                        dr[j] = mj;
                    }
                }
                dtResult.Rows.Add(dr);
            }

            DataView dv = dtResult.DefaultView;
            dv.Sort = "" + StatisticsCol[0] + "," + StatisticsCol[1] + " ASC";
            dt.Dispose();
            dt = null;
            dtJcb.Dispose();
            dtJcb = null;
            dtTmp = dv.ToTable();
            if (StatisticsCol[0] != "tmp")
            {
                DataRow dr = dtTmp.NewRow();
                for (int i = 0; i < dtTmp.Columns.Count; i++)
                {
                    dr[i] = drHj[i];
                }
                dtTmp.Rows.InsertAt(dr, 0);
            }
            return dtTmp;
        }

        private void ExportTable(DataTable dt, string tmplateFile, string outPath, int rowIndex, Dictionary<string, string> dicQsdwdm)
        {
            if (dt.Rows.Count > 0)
            {
                string nowTime = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                string xzqdm = dt.Rows[0][1].ToString();
                //string tmplateFile = RCIS.Global.AppParameters.TemplatePath + "\\土地利用现状二级分类面积汇总表.xlsx";
                string tableName = tmplateFile.Substring(tmplateFile.LastIndexOf("\\") + 1);
                tableName = tableName.Substring(0, tableName.IndexOf("."));
                string excelReportFilename = outPath + "\\(" + xzqdm + ")" + tableName + "" + nowTime + ".xlsx";
                System.IO.File.Copy(tmplateFile, excelReportFilename);

                Aspose.Cells.Workbook wk = new Aspose.Cells.Workbook(excelReportFilename);
                Aspose.Cells.Worksheet sheet = wk.Worksheets[0];
                Aspose.Cells.Cells cells = sheet.Cells;
                //边框和 数值 格式
                Aspose.Cells.Style styleNum = wk.Styles[wk.Styles.Add()];
                styleNum.Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleNum.Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleNum.Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleNum.Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleNum.Number = 2;
                styleNum.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Right;

                Aspose.Cells.Style styleTxt = wk.Styles[wk.Styles.Add()];
                styleTxt.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Right;
                styleTxt.Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleTxt.Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleTxt.Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleTxt.Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleTxt.Number = 49; //@

                cells.InsertColumn(1);
                cells.InsertColumn(1);
                cells.InsertColumn(0);
                cells[3, 2].PutValue("乡镇名称");
                cells[3, 3].PutValue("区县名");


                cells.Merge(2, 0, rowIndex - 2, 1);
                //int rowIndex = 4;
                cells[2, 0].SetStyle(styleTxt);
                if (dt.Columns[0].ColumnName.ToString().Trim() != "tmp")
                    cells[2, 0].PutValue(dt.Columns[0].ColumnName);

                int dw = radioGroup2.SelectedIndex;
                string dwName = "";
                if (dw == 0)
                {
                    dw = 1;
                    dwName = "单位：公顷";
                }
                else if (dw == 1)
                {
                    dw = 15;
                    dwName = "单位：亩";
                }
                else if (dw == 2)
                {
                    dw = 1;
                    dwName = "单位：平方米";
                }

                string tjdw = "";
                if (radioGroup3.SelectedIndex == 0)
                    tjdw = "ZLDWDM";
                else if (radioGroup3.SelectedIndex == 1)
                    tjdw = "QSDWDM";

                string oldVal = "";
                int oldRow = rowIndex;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    //if (dr[tjdw].ToString().Length > 12)
                    //    dr[tjdw] = dr[tjdw].ToString().Substring(0, 12);

                    cells[rowIndex, 0].SetStyle(styleTxt);
                    cells[rowIndex, 1].SetStyle(styleTxt);
                    cells[rowIndex, 2].SetStyle(styleTxt);
                    cells[rowIndex, 3].SetStyle(styleTxt);
                    cells[rowIndex, 4].SetStyle(styleTxt);

                    int iColNum = 0;
                    string dwdm = dr[tjdw].ToString();
                    cells[rowIndex, iColNum++].PutValue(dr[0].ToString());
                    cells[rowIndex, iColNum++].PutValue(dicQsdwdm.ContainsKey(dwdm) ? dicQsdwdm[dwdm] : dwdm);
                    if (dwdm.Length > 9)
                        dwdm = dwdm.Substring(0, 9);
                    cells[rowIndex, iColNum++].PutValue(dicQsdwdm.ContainsKey(dwdm) ? dicQsdwdm[dwdm] : "");
                    if (dwdm.Length > 6)
                        dwdm = dwdm.Substring(0, 6);
                    cells[rowIndex, iColNum++].PutValue(dicQsdwdm.ContainsKey(dwdm) ? dicQsdwdm[dwdm] : "");
                    cells[rowIndex, iColNum++].PutValue(dr[tjdw].ToString());

                    //int a = dt.Columns.IndexOf("ZMJ");

                    for (int k = 2; k < dt.Columns.Count; k++)
                    {
                        double mj = 0;
                        double.TryParse(dr[dt.Columns[k]].ToString(), out mj);
                        cells[rowIndex, iColNum].SetStyle(styleNum);
                        cells[rowIndex, iColNum].PutValue(mj * dw);
                        iColNum++;
                    }
                    rowIndex++;

                    if (oldVal != dt.Rows[i][0].ToString())
                    {
                        if (rowIndex - oldRow != 1)
                        {
                            cells.Merge(oldRow-1, 0, rowIndex - oldRow, 1);
                            oldVal = dt.Rows[i][0].ToString();
                            oldRow = rowIndex;
                        }
                        
                        else
                        {
                            oldVal = dt.Rows[i][0].ToString();
                            oldRow = rowIndex;
                        }
                    }

                    if (i == dt.Rows.Count - 1)
                    {
                        cells.Merge(oldRow - 1, 0, rowIndex - oldRow+1, 1);
                    }

                }

                if (dt.Columns[0].ColumnName == "tmp")
                {
                    cells.DeleteColumn(0);
                    cells[1, 0].PutValue(dwName);
                }
                else
                {
                    cells[1, 1].PutValue(dwName);
                }

                wk.Save(excelReportFilename);

            }

        }

        private void ExportTableHD(DataTable dt, string tmplateFile, string outPath, int rowIndex, Dictionary<string, string> dicQsdwdm)
        {
            if (dt.Rows.Count > 0)
            {
                string nowTime = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                string xzqdm = dt.Rows[0][2].ToString();
                if (xzqdm.Length > 6)
                    xzqdm = xzqdm.Substring(0, 6);
                //string tmplateFile = RCIS.Global.AppParameters.TemplatePath + "\\土地利用现状二级分类面积汇总表.xlsx";
                string tableName = tmplateFile.Substring(tmplateFile.LastIndexOf("\\") + 1);
                tableName = tableName.Substring(0, tableName.IndexOf("."));
                string excelReportFilename = outPath + "\\(" + xzqdm + ")" + tableName + "" + nowTime + ".xlsx";
                System.IO.File.Copy(tmplateFile, excelReportFilename);

                Aspose.Cells.Workbook wk = new Aspose.Cells.Workbook(excelReportFilename);
                Aspose.Cells.Worksheet sheet = wk.Worksheets[0];
                Aspose.Cells.Cells cells = sheet.Cells;
                //边框和 数值 格式
                Aspose.Cells.Style styleNum = wk.Styles[wk.Styles.Add()];
                styleNum.Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleNum.Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleNum.Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleNum.Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleNum.Number = 2;
                styleNum.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Right;

                Aspose.Cells.Style styleTxt = wk.Styles[wk.Styles.Add()];
                styleTxt.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Right;
                styleTxt.Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleTxt.Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleTxt.Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleTxt.Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleTxt.Number = 49; //@

                cells.InsertColumn(0);
                cells.Merge(2, 0, rowIndex - 2, 1);
                //int rowIndex = 4;
                cells[2, 0].SetStyle(styleTxt);
                if (dt.Columns[0].ColumnName.ToString().Trim() != "tmp")
                    cells[2, 0].PutValue(dt.Columns[0].ColumnName);

                int dw = radioGroup2.SelectedIndex;
                string dwName = "";
                if (dw == 0)
                {
                    dw = 1;
                    dwName = "单位：公顷";
                }
                else if (dw == 1)
                {
                    dw = 15;
                    dwName = "单位：亩";
                }
                else if (dw == 2)
                {
                    dw = 10000;
                    dwName = "单位：平方米";
                }

                string tjdw = "";
                if (radioGroup3.SelectedIndex == 0)
                    tjdw = "ZLDWDM";
                else if (radioGroup3.SelectedIndex == 1)
                    tjdw = "QSDWDM";

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    //if (dr[tjdw].ToString().Length > 12)
                    //    dr[tjdw] = dr[tjdw].ToString().Substring(0, 12);

                    cells[rowIndex, 0].SetStyle(styleTxt);
                    cells[rowIndex, 1].SetStyle(styleTxt);
                    cells[rowIndex, 2].SetStyle(styleTxt);
                    cells[rowIndex, 3].SetStyle(styleTxt);
                    cells[rowIndex, 4].SetStyle(styleTxt);

                    int iColNum = 0;

                    cells[rowIndex, iColNum++].PutValue(dr[0].ToString());
                    cells[rowIndex, iColNum++].PutValue(dr[1].ToString());
                    cells[rowIndex, iColNum++].PutValue(dicQsdwdm.ContainsKey(dr[tjdw].ToString()) ? dicQsdwdm[dr[tjdw].ToString()] : "");
                    cells[rowIndex, iColNum++].PutValue(dr[tjdw].ToString());
                    cells[rowIndex, iColNum++].PutValue(dr["HDMC"].ToString());

                    //int a = dt.Columns.IndexOf("ZMJ");

                    for (int k = 4; k < dt.Columns.Count; k++)
                    {
                        double mj = 0;
                        double.TryParse(dr[dt.Columns[k]].ToString(), out mj);
                        cells[rowIndex, iColNum].SetStyle(styleNum);
                        cells[rowIndex, iColNum].PutValue(mj * dw);
                        iColNum++;
                    }
                    rowIndex++;
                }

                if (dt.Columns[0].ColumnName == "tmp")
                {
                    cells.DeleteColumn(0);
                    cells[1, 0].PutValue(dwName);
                }
                else
                {
                    cells[1, 1].PutValue(dwName);
                }

                wk.Save(excelReportFilename);

            }

        }


        private void ExportTableFRD(DataTable dt, string tmplateFile, string outPath, int rowIndex, Dictionary<string, string> dicQsdwdm)
        {
            if (dt.Rows.Count > 0)
            {
                string nowTime = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                string xzqdm = dt.Rows[0][2].ToString();
                if (xzqdm.Length > 6)
                    xzqdm = xzqdm.Substring(0, 6);
                //string tmplateFile = RCIS.Global.AppParameters.TemplatePath + "\\土地利用现状二级分类面积汇总表.xlsx";
                string tableName = tmplateFile.Substring(tmplateFile.LastIndexOf("\\") + 1);
                tableName = tableName.Substring(0, tableName.IndexOf("."));
                string excelReportFilename = outPath + "\\(" + xzqdm + ")" + tableName + "" + nowTime + ".xlsx";
                System.IO.File.Copy(tmplateFile, excelReportFilename);

                Aspose.Cells.Workbook wk = new Aspose.Cells.Workbook(excelReportFilename);
                Aspose.Cells.Worksheet sheet = wk.Worksheets[0];
                Aspose.Cells.Cells cells = sheet.Cells;
                //边框和 数值 格式
                Aspose.Cells.Style styleNum = wk.Styles[wk.Styles.Add()];
                styleNum.Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleNum.Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleNum.Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleNum.Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleNum.Number = 2;
                styleNum.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Right;

                Aspose.Cells.Style styleTxt = wk.Styles[wk.Styles.Add()];
                styleTxt.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Right;
                styleTxt.Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleTxt.Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleTxt.Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleTxt.Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleTxt.Number = 49; //@

                cells.InsertColumn(0);
                cells.Merge(2, 0, rowIndex - 2, 1);
                //int rowIndex = 4;
                cells[2, 0].SetStyle(styleTxt);
                if (dt.Columns[0].ColumnName.ToString().Trim() != "tmp")
                    cells[2, 0].PutValue(dt.Columns[0].ColumnName);

                int dw = radioGroup2.SelectedIndex;
                string dwName = "";
                if (dw == 0)
                {
                    dw = 1;
                    dwName = "单位：公顷";
                }
                else if (dw == 1)
                {
                    dw = 15;
                    dwName = "单位：亩";
                }
                else if (dw == 2)
                {
                    dw = 10000;
                    dwName = "单位：平方米";
                }

                //string tjdw = "";
                //if (radioGroup3.SelectedIndex == 0)
                //    tjdw = "ZLDWDM";
                //else if (radioGroup3.SelectedIndex == 1)
                //    tjdw = "QSDWDM";

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    //if (dr[tjdw].ToString().Length > 12)
                    //    dr[tjdw] = dr[tjdw].ToString().Substring(0, 12);

                    cells[rowIndex, 0].SetStyle(styleTxt);
                    cells[rowIndex, 1].SetStyle(styleTxt);
                    cells[rowIndex, 2].SetStyle(styleTxt);
                    cells[rowIndex, 3].SetStyle(styleTxt);
                    cells[rowIndex, 4].SetStyle(styleTxt);
                    cells[rowIndex, 5].SetStyle(styleTxt);

                    int iColNum = 0;

                    cells[rowIndex, iColNum++].PutValue(dr[0].ToString());
                    cells[rowIndex, iColNum++].PutValue(dr[1].ToString());
                    cells[rowIndex, iColNum++].PutValue(dicQsdwdm.ContainsKey(dr["ZLDWDM"].ToString()) ? dicQsdwdm[dr["ZLDWDM"].ToString()] : "");
                    cells[rowIndex, iColNum++].PutValue(dr["ZLDWDM"].ToString());
                    cells[rowIndex, iColNum++].PutValue(dicQsdwdm.ContainsKey(dr["QSDWDM"].ToString()) ? dicQsdwdm[dr["QSDWDM"].ToString()] : "");
                    cells[rowIndex, iColNum++].PutValue(dr["QSDWDM"].ToString());

                    //int a = dt.Columns.IndexOf("ZMJ");

                    for (int k = 4; k < dt.Columns.Count; k++)
                    {
                        double mj = 0;
                        double.TryParse(dr[dt.Columns[k]].ToString(), out mj);
                        cells[rowIndex, iColNum].SetStyle(styleNum);
                        cells[rowIndex, iColNum].PutValue(mj * dw);
                        iColNum++;
                    }
                    rowIndex++;
                }

                if (dt.Columns[0].ColumnName == "tmp")
                {
                    cells.DeleteColumn(0);
                    cells[1, 0].PutValue(dwName);
                }
                else
                {
                    cells[1, 1].PutValue(dwName);
                }

                wk.Save(excelReportFilename);

            }

        }

        private Dictionary<string, string> GetDMMCDicByQueryDef(IFeatureWorkspace pFeatureWorkspace, string tableName, string keyField, string valueField)
        {
            Dictionary<string, string> dmmc = new Dictionary<string, string>();
            using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
            {
                IQueryDef pQDef = pFeatureWorkspace.CreateQueryDef();
                comRel.ManageLifetime(pQDef);
                pQDef.Tables = tableName + " Group By " + keyField + "," + valueField;
                pQDef.SubFields = keyField + "," + valueField;
                ICursor pCur = pQDef.Evaluate();
                comRel.ManageLifetime(pCur);
                IRow pRow;
                while ((pRow = pCur.NextRow()) != null)
                {
                    string dm = pRow.get_Value(0).ToString();
                    //if (dm.Length > 12)
                    //dm = dm.Substring(0,12);
                    string mc = pRow.get_Value(1).ToString();
                    if (!dmmc.Keys.Contains(dm))
                        dmmc.Add(dm, mc);
                }
            }
            return dmmc;
        }

        private Dictionary<string, string> getDic(Dictionary<string, string> A, Dictionary<string, string> B)
        {
            Dictionary<string, string> C = A;
            foreach (string item in B.Keys)
            {
                if (!C.Keys.Contains(item))
                {
                    C.Add(item, B[item]);
                }

                if (!C.Keys.Contains(item.Substring(0, 6)))
                {
                    DataRow dr = RCIS.Database.LS_SetupMDBHelper.GetDataRow("Select MC From SYS_XZQ Where DM = '" + item.Substring(0, 6) + "'", "tmp");
                    if (dr == null)
                    {
                        C.Add(item.Substring(0, 6), "全县");
                    }
                    else
                    {
                        C.Add(item.Substring(0, 6), dr[0].ToString());
                    }
                }
            }




            return C;
        }

        private DataTable DLTP(DataRow dr, List<string> colArr, int num, double tpVal)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("mj", typeof(double));
            dt.Columns.Add("index", typeof(string));
            for (int i = colArr.Count; i < num; i++)
            {
                double mj = 0;
                double.TryParse(dr[i].ToString(), out mj);
                if (mj <= 0)
                    continue;
                DataRow dr1 = dt.NewRow();
                dr1[0] = dr[i];
                dr1[1] = i;
                dt.Rows.Add(dr1);
            }

            DataView dv = dt.DefaultView;
            dv.Sort = "mj desc";
            DataTable dtDesc = dv.ToTable();

            double tp001 = 0.01;
            if (tpVal < 0)
                tp001 = -0.01;
            double count = Math.Abs(tpVal / 0.01);
            double item = Math.Floor(count / dtDesc.Rows.Count);
            double ys = count % dtDesc.Rows.Count;
            for (int i = 0; i < dtDesc.Rows.Count; i++)
            {
                if (i < ys)
                    dtDesc.Rows[i][0] = double.Parse(dtDesc.Rows[i][0].ToString()) + tp001 * item + tp001;
                else
                    dtDesc.Rows[i][0] = double.Parse(dtDesc.Rows[i][0].ToString()) + tp001 * item;
            }
            return dtDesc;
        }

        private void bbfxForm_Load(object sender, EventArgs e)
        {
            //foreach (Control c in this.groupBoxTabs.Controls)
            //{
            //    if (c is DevExpress.XtraEditors.CheckEdit)
            //    {
            //        DevExpress.XtraEditors.CheckEdit ce = (DevExpress.XtraEditors.CheckEdit)c;
            //        ce.Enabled = false;
            //    }
            //}
        }

        private DataTable OutTableFQLJTM(DataTable dtJCB, string shpField, string groupField)
        {
            DataTable dtResult = new DataTable();
            dtResult.Columns.Add(shpField, typeof(string));
            dtResult.Columns.Add(groupField, typeof(string));
            dtResult.Columns.Add("FQ0602", typeof(double));
            dtResult.Columns.Add("FQ1001", typeof(double));
            dtResult.Columns.Add("FQ1003", typeof(double));
            dtResult.Columns.Add("FQHJ", typeof(double), "FQ0602 + FQ1001 + FQ1003");
            dtResult.Columns["FQHJ"].SetOrdinal(2);
            dtResult.Columns.Add("LJTM0301", typeof(double));
            dtResult.Columns.Add("LJTM0302", typeof(double));
            dtResult.Columns.Add("LJTM0305", typeof(double));
            dtResult.Columns.Add("LJTM0307", typeof(double));
            dtResult.Columns.Add("LJTM0404", typeof(double));
            dtResult.Columns.Add("LJTMHJ", typeof(double), "LJTM0301 + LJTM0302 + LJTM0305 + LJTM0307 + LJTM0404");
            dtResult.Columns["LJTMHJ"].SetOrdinal(6);
            //dtResult.Columns.Add("HJ", typeof(double), "FQHJ + LJTMHJ");
            //dtResult.Columns["HJ"].SetOrdinal(2);

            var lstshp = dtJCB.AsEnumerable().GroupBy(a => a.Field<string>(shpField)).ToList();
            var lstGroup = dtJCB.AsEnumerable().GroupBy(a => a.Field<string>(groupField)).ToList().
                Union(dtJCB.AsEnumerable().GroupBy(a => a.Field<string>(groupField).Substring(0, 9)).ToList()).
                Union(dtJCB.AsEnumerable().GroupBy(a => a.Field<string>(groupField).Substring(0, 6)).ToList());
            string dwdm = "";
            foreach (var ishp in lstshp)
            {
                foreach (var igroup in lstGroup)
                {
                    if (igroup.Key.Length == 6)
                        dwdm = igroup.Key;
                    DataRow dr = dtResult.NewRow();
                    dr[shpField] = ishp.Key;
                    dr[groupField] = igroup.Key;
                    object o = dtJCB.Compute("Sum([0602])", shpField + " = '" + ishp.Key + "' And " + groupField + " Like '" + igroup.Key + "%' And TBXHDM = 'FQ'");
                    double mj = 0;
                    double.TryParse(o.ToString(), out mj);
                    dr["FQ0602"] = mj;
                    o = dtJCB.Compute("Sum([1001])", shpField + " = '" + ishp.Key + "' And " + groupField + " Like '" + igroup.Key + "%' And TBXHDM = 'FQ'");
                    double.TryParse(o.ToString(), out mj);
                    dr["FQ1001"] = mj;
                    o = dtJCB.Compute("Sum([1003])", shpField + " = '" + ishp.Key + "' And " + groupField + " Like '" + igroup.Key + "%' And TBXHDM = 'FQ'");
                    double.TryParse(o.ToString(), out mj);
                    dr["FQ1003"] = mj;
                    o = dtJCB.Compute("Sum([0301])", shpField + " = '" + ishp.Key + "' And " + groupField + " Like '" + igroup.Key + "%' And TBXHDM = 'LJTM'");
                    double.TryParse(o.ToString(), out mj);
                    dr["LJTM0301"] = mj;
                    o = dtJCB.Compute("Sum([0302])", shpField + " = '" + ishp.Key + "' And " + groupField + " Like '" + igroup.Key + "%' And TBXHDM = 'LJTM'");
                    double.TryParse(o.ToString(), out mj);
                    dr["LJTM0302"] = mj;
                    o = dtJCB.Compute("Sum([0305])", shpField + " = '" + ishp.Key + "' And " + groupField + " Like '" + igroup.Key + "%' And TBXHDM = 'LJTM'");
                    double.TryParse(o.ToString(), out mj);
                    dr["LJTM0305"] = mj;
                    o = dtJCB.Compute("Sum([0307])", shpField + " = '" + ishp.Key + "' And " + groupField + " Like '" + igroup.Key + "%' And TBXHDM = 'LJTM'");
                    double.TryParse(o.ToString(), out mj);
                    dr["LJTM0307"] = mj;
                    o = dtJCB.Compute("Sum([0404])", shpField + " = '" + ishp.Key + "' And " + groupField + " Like '" + igroup.Key + "%' And TBXHDM = 'LJTM'");
                    double.TryParse(o.ToString(), out mj);
                    dr["LJTM0404"] = mj;
                    dtResult.Rows.Add(dr);
                }
            }

            DataView dv = new DataView(dtResult);
            dv.Sort = groupField;
            dtResult = dv.ToTable();
            //dtResult.PrimaryKey = new DataColumn[] { dtResult.Columns[groupField] };

            if (lstshp.Count > 1 && !string.IsNullOrWhiteSpace(dwdm))
            {
                DataRow dr = dtResult.NewRow();
                for (int i = 2; i < dtResult.Columns.Count; i++)
                {
                    string item = dtResult.Columns[i].ColumnName;
                    dr[shpField] = "合计";
                    dr[groupField] = dwdm;
                    object o = dtResult.Compute("Sum([" + item + "])", "" + groupField + "='" + dwdm + "'");
                    double mj = 0;
                    double.TryParse(o.ToString(), out mj);
                    dr[item] = mj;
                }
                //foreach (var item in staFields)
                //{
                //    dr[shpField] = "合计";
                //    dr[groupField] = dwdm;
                //    object o = dtResult.Compute("Sum([" + item + "])", "" + groupField + "='" + dwdm + "'");
                //    double mj = 0;
                //    double.TryParse(o.ToString(), out mj);
                //    dr[item] = mj;
                //}
                dtResult.Rows.InsertAt(dr, 0);
            }

            return dtResult;
        }

        private DataTable GetWJMHDTab(string shpField)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(shpField, typeof(string));
            dt.Columns.Add("ZLDWDM", typeof(string));
            string[] allDls = new string[] { 
                "i101","i102","i103","i104","i105","i106","i107","i108","i109","i110","i201","i202","i203"
            };
            for (int i = 0; i < allDls.Length; i++)
            {
                dt.Columns.Add(allDls[i], typeof(double));
            }
            dt.Columns.Add("i1", typeof(double), "i101+i102+i103+i104+i105+i106+i107+i108+i109+i110");
            dt.Columns["i1"].SetOrdinal(2);
            dt.Columns.Add("i2", typeof(double), "i201+i202+i203");
            dt.Columns["i2"].SetOrdinal(13);
            dt.Columns.Add("hj", typeof(double), "i1+i2");
            dt.Columns["hj"].SetOrdinal(2);
            IFeatureClass pWJMHD = (RCIS.Global.GlobalEditObject.GlobalWorkspace as IFeatureWorkspace).OpenFeatureClass("WJMHD");
            if (pWJMHD.FeatureCount(null) == 0)
                return null;
            //选择shp
            //if (radioGroup1.SelectedIndex == 0)
            //{
            //    string shpPath = txtPath.Text;
            //    IFeatureClass pShpClass = RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(shpPath);
            //    IFeatureCursor pFeaCursor = pShpClass.Search(null, true);
            //    IFeature pShpFeature;

            //    //循环shp里边的各个图形
            //    while ((pShpFeature = pFeaCursor.NextFeature()) != null)
            //    {
            //        //选择shp字段的值  用于区分每个shp图形
            //        string shpBH = pShpFeature.get_Value(pShpFeature.Fields.FindField(shpField)).ToString();

            //        ITopologicalOperator pTopolo = pShpFeature.ShapeCopy as ITopologicalOperator;
            //        //查询地类图斑中与该shp相交的图斑
            //        ISpatialFilter pSF = new SpatialFilterClass();
            //        pSF.Geometry = pShpFeature.ShapeCopy;
            //        pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            //        IFeatureCursor pCursor = pWJMHD.Search(pSF, true);
            //        IFeature pFeature;
            //        while ((pFeature = pCursor.NextFeature()) != null)
            //        {
            //            //判断相交面积
            //            IGeometry pIntersectGeo = pTopolo.Intersect(pFeature.ShapeCopy, esriGeometryDimension.esriGeometry2Dimension);
            //            double aa2 = (pIntersectGeo as IArea).Area;
            //            if ((pIntersectGeo as IArea).Area > 0)
            //            {
            //                string dlbm = pFeature.get_Value(pFeature.Fields.FindField("DLBM")).ToString();
            //                double tbmj = double.Parse(pFeature.get_Value(pFeature.Fields.FindField("TBMJ")).ToString());
            //                //根据相交面积占图斑面积比例，计算相交部分图斑地类面积
            //                double mj = Math.Round(tbmj * (Math.Round((pIntersectGeo as IArea).Area, 2) / Math.Round((pFeature.ShapeCopy as IArea).Area, 2)), 2);
            //                double kcxs = 0;
            //                double.TryParse(pFeature.get_Value(pFeature.Fields.FindField("KCXS")).ToString(), out kcxs);
            //                double kcmj = Math.Round(mj * kcxs, 2);
            //                DataRow dr = dtJCB.NewRow();
            //                //各个列赋值  如zldwdm qsdwdm
            //                dr[0] = shpBH;
            //                for (int i = 1; i < colArr.Count; i++)
            //                {
            //                    dr[i] = pFeature.get_Value(pFeature.Fields.FindField(colArr[i])).ToString().Trim();
            //                }
            //                //面积赋值
            //                if (dlbm.Substring(0, 2) == "01")
            //                {
            //                    dr["1203"] = kcmj;
            //                    dr[dlbm] = Math.Round(mj - kcmj, 2);
            //                }
            //                else
            //                    dr[dlbm] = Math.Round(mj, 2);
            //                dtJCB.Rows.Add(dr);
            //            }
            //            RCIS.Utility.OtherHelper.ReleaseComObject(pFeature);
            //        }
            //        RCIS.Utility.OtherHelper.ReleaseComObject(pCursor);
            //        RCIS.Utility.OtherHelper.ReleaseComObject(pShpFeature);
            //    }
            //    RCIS.Utility.OtherHelper.ReleaseComObject(pFeaCursor);
            //}
            //else if (radioGroup1.SelectedIndex == 1)
            //{
            //    //绘制地图
            //    if (m_SelGeo == null)
            //    {
            //        MessageBox.Show("未绘制图形", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        return;
            //    }
            //    ITopologicalOperator pTopolo = m_SelGeo as ITopologicalOperator;
            //    //查询地类图斑中与该shp相交的图斑
            //    ISpatialFilter pSF = new SpatialFilterClass();
            //    pSF.Geometry = m_SelGeo;
            //    pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            //    IFeatureCursor pCursor = pDLTB.Search(pSF, true);
            //    IFeature pFeature;
            //    while ((pFeature = pCursor.NextFeature()) != null)
            //    {
            //        //判断相交面积
            //        IGeometry pIntersectGeo = pTopolo.Intersect(pFeature.ShapeCopy, esriGeometryDimension.esriGeometry2Dimension);
            //        double aa2 = (pIntersectGeo as IArea).Area;
            //        if ((pIntersectGeo as IArea).Area > 0)
            //        {
            //            string dlbm = pFeature.get_Value(pFeature.Fields.FindField("DLBM")).ToString();
            //            double tbmj = double.Parse(pFeature.get_Value(pFeature.Fields.FindField("TBMJ")).ToString());
            //            //根据相交面积占图斑面积比例，计算相交部分图斑地类面积
            //            double mj = Math.Round(tbmj * (Math.Round((pIntersectGeo as IArea).Area, 2) / Math.Round((pFeature.ShapeCopy as IArea).Area, 2)), 2);
            //            double kcxs = 0;
            //            double.TryParse(pFeature.get_Value(pFeature.Fields.FindField("KCXS")).ToString(), out kcxs);
            //            double kcmj = Math.Round(mj * kcxs, 2);
            //            DataRow dr = dtJCB.NewRow();
            //            //各个列赋值  如zldwdm qsdwdm
            //            dr[0] = "";
            //            for (int i = 1; i < colArr.Count; i++)
            //            {
            //                dr[i] = pFeature.get_Value(pFeature.Fields.FindField(colArr[i])).ToString().Trim();
            //            }
            //            //面积赋值
            //            if (dlbm.Substring(0, 2) == "01")
            //            {
            //                dr["1203"] = kcmj;
            //                dr[dlbm] = Math.Round(mj - kcmj, 2);
            //            }
            //            else
            //                dr[dlbm] = Math.Round(mj, 2);
            //            dtJCB.Rows.Add(dr);
            //        }
            //        RCIS.Utility.OtherHelper.ReleaseComObject(pFeature);
            //    }
            //    RCIS.Utility.OtherHelper.ReleaseComObject(pCursor);
            //}

            return dt;
        }

        private DataTable OutTableGDPD(DataTable dtJCB, string shpField, string groupField)
        {
            DataTable dtResult = new DataTable();
            dtResult.Columns.Add(shpField, typeof(string));
            dtResult.Columns.Add(groupField, typeof(string));
            dtResult.Columns.Add("PD", typeof(double));
            dtResult.Columns.Add("TT2", typeof(double));
            dtResult.Columns.Add("PD2", typeof(double));
            dtResult.Columns.Add("HJ2", typeof(double), "TT2 + PD2");
            dtResult.Columns["HJ2"].SetOrdinal(dtResult.Columns.IndexOf("TT2"));
            dtResult.Columns.Add("TT3", typeof(double));
            dtResult.Columns.Add("PD3", typeof(double));
            dtResult.Columns.Add("HJ3", typeof(double), "TT3 + PD3");
            dtResult.Columns["HJ3"].SetOrdinal(dtResult.Columns.IndexOf("TT3"));
            dtResult.Columns.Add("TT4", typeof(double));
            dtResult.Columns.Add("PD4", typeof(double));
            dtResult.Columns.Add("HJ4", typeof(double), "TT4 + PD4");
            dtResult.Columns["HJ4"].SetOrdinal(dtResult.Columns.IndexOf("TT4"));
            dtResult.Columns.Add("TT5", typeof(double));
            dtResult.Columns.Add("PD5", typeof(double));
            dtResult.Columns.Add("HJ5", typeof(double), "TT5 + PD5");
            dtResult.Columns["HJ5"].SetOrdinal(dtResult.Columns.IndexOf("TT5"));
            dtResult.Columns.Add("HJ", typeof(double), "PD + HJ2 + HJ3 + HJ4 + HJ5");
            dtResult.Columns["HJ"].SetOrdinal(2);

            var lstshp = dtJCB.AsEnumerable().GroupBy(a => a.Field<string>(shpField)).ToList();
            var lstGroup = dtJCB.AsEnumerable().GroupBy(a => a.Field<string>(groupField)).ToList().
                Union(dtJCB.AsEnumerable().GroupBy(a => a.Field<string>(groupField).Substring(0, 9)).ToList()).
                Union(dtJCB.AsEnumerable().GroupBy(a => a.Field<string>(groupField).Substring(0, 6)).ToList());
            string dwdm = "";
            foreach (var ishp in lstshp)
            {
                foreach (var igroup in lstGroup)
                {
                    if (igroup.Key.Length == 6)
                        dwdm = igroup.Key;
                    DataRow dr = dtResult.NewRow();
                    dr[shpField] = ishp.Key;
                    dr[groupField] = igroup.Key;
                    object o = dtJCB.Compute("Sum([01])", shpField + " = '" + ishp.Key + "' And " + groupField + " Like '" + igroup.Key + "%' And GDPDJB = '1'");
                    double mj = 0;
                    double.TryParse(o.ToString(), out mj);
                    dr["PD"] = mj;
                    o = dtJCB.Compute("Sum([01])", shpField + " = '" + ishp.Key + "' And " + groupField + " Like '" + igroup.Key + "%' And GDPDJB = '2' And GDLX = 'TT'");
                    double.TryParse(o.ToString(), out mj);
                    dr["TT2"] = mj;
                    o = dtJCB.Compute("Sum([01])", shpField + " = '" + ishp.Key + "' And " + groupField + " Like '" + igroup.Key + "%' And GDPDJB = '2' And GDLX = 'PD'");
                    double.TryParse(o.ToString(), out mj);
                    dr["PD2"] = mj;
                    o = dtJCB.Compute("Sum([01])", shpField + " = '" + ishp.Key + "' And " + groupField + " Like '" + igroup.Key + "%' And GDPDJB = '3' And GDLX = 'TT'");
                    double.TryParse(o.ToString(), out mj);
                    dr["TT3"] = mj;
                    o = dtJCB.Compute("Sum([01])", shpField + " = '" + ishp.Key + "' And " + groupField + " Like '" + igroup.Key + "%' And GDPDJB = '3' And GDLX = 'PD'");
                    double.TryParse(o.ToString(), out mj);
                    dr["PD3"] = mj;
                    o = dtJCB.Compute("Sum([01])", shpField + " = '" + ishp.Key + "' And " + groupField + " Like '" + igroup.Key + "%' And GDPDJB = '4' And GDLX = 'TT'");
                    double.TryParse(o.ToString(), out mj);
                    dr["TT4"] = mj;
                    o = dtJCB.Compute("Sum([01])", shpField + " = '" + ishp.Key + "' And " + groupField + " Like '" + igroup.Key + "%' And GDPDJB = '4' And GDLX = 'PD'");
                    double.TryParse(o.ToString(), out mj);
                    dr["PD4"] = mj;
                    o = dtJCB.Compute("Sum([01])", shpField + " = '" + ishp.Key + "' And " + groupField + " Like '" + igroup.Key + "%' And GDPDJB = '5' And GDLX = 'TT'");
                    double.TryParse(o.ToString(), out mj);
                    dr["TT5"] = mj;
                    o = dtJCB.Compute("Sum([01])", shpField + " = '" + ishp.Key + "' And " + groupField + " Like '" + igroup.Key + "%' And GDPDJB = '5' And GDLX = 'PD'");
                    double.TryParse(o.ToString(), out mj);
                    dr["PD5"] = mj;
                    dtResult.Rows.Add(dr);
                }
            }

            DataView dv = new DataView(dtResult);
            dv.Sort = groupField;
            dtResult = dv.ToTable();

            if (lstshp.Count > 1 && !string.IsNullOrWhiteSpace(dwdm))
            {
                DataRow dr = dtResult.NewRow();
                for (int i = 2; i < dtResult.Columns.Count; i++)
                {
                    string item = dtResult.Columns[i].ColumnName;
                    dr[shpField] = "合计";
                    dr[groupField] = dwdm;
                    object o = dtResult.Compute("Sum([" + item + "])", "" + groupField + "='" + dwdm + "'");
                    double mj = 0;
                    double.TryParse(o.ToString(), out mj);
                    dr[item] = mj;
                }
                dtResult.Rows.InsertAt(dr, 0);
            }

            return dtResult;
        }

        private DataTable OutTableYJFLQSXZ(DataTable dtJCB, string shpField, string groupField)
        {
            string[] staFields = { "ZMJ", "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };
            DataTable dtResult = new DataTable();
            dtResult.Columns.Add(shpField, typeof(string));
            dtResult.Columns.Add(groupField, typeof(string));
            foreach (string item in staFields)
            {
                dtResult.Columns.Add(item, typeof(double));
                dtResult.Columns.Add("G" + item, typeof(double));
                dtResult.Columns.Add("J" + item, typeof(double));
            }
            var lstshp = dtJCB.AsEnumerable().GroupBy(a => a.Field<string>(shpField)).ToList();
            var lstGroup = dtJCB.AsEnumerable().GroupBy(a => a.Field<string>(groupField)).ToList().
                Union(dtJCB.AsEnumerable().GroupBy(a => a.Field<string>(groupField).Substring(0, 9)).ToList()).
                Union(dtJCB.AsEnumerable().GroupBy(a => a.Field<string>(groupField).Substring(0, 6)).ToList());
            string dwdm = "";
            foreach (var ishp in lstshp)
            {
                foreach (var igroup in lstGroup)
                {
                    if (igroup.Key.Length == 6)
                        dwdm = igroup.Key;
                    DataRow dr = dtResult.NewRow();
                    foreach (var item in staFields)
                    {
                        dr[shpField] = ishp.Key;
                        dr[groupField] = igroup.Key;
                        object o = dtJCB.Compute("Sum([" + item + "])", shpField + " = '" + ishp.Key + "' And " + groupField + " Like '" + igroup.Key + "%'");
                        double mj = 0;
                        double.TryParse(o.ToString(), out mj);
                        dr[item] = mj;
                        o = dtJCB.Compute("Sum([" + item + "])", shpField + " = '" + ishp.Key + "' And " + groupField + " Like '" + igroup.Key + "%' And (QSXZ Like '1%' Or QSXZ Like '2%')");
                        double.TryParse(o.ToString(), out mj);
                        dr["G" + item] = mj;
                        o = dtJCB.Compute("Sum([" + item + "])", shpField + " = '" + ishp.Key + "' And " + groupField + " Like '" + igroup.Key + "%' And (QSXZ Like '3%' Or QSXZ Like '4%')");
                        double.TryParse(o.ToString(), out mj);
                        dr["J" + item] = mj;
                    }
                    dtResult.Rows.Add(dr);
                }
            }

            DataView dv = new DataView(dtResult);
            dv.Sort = groupField;
            dtResult = dv.ToTable();
            //dtResult.PrimaryKey = new DataColumn[] { dtResult.Columns[groupField] };

            if (lstshp.Count > 1 && !string.IsNullOrWhiteSpace(dwdm))
            {
                DataRow dr = dtResult.NewRow();
                foreach (var item in staFields)
                {
                    dr[shpField] = "合计";
                    dr[groupField] = dwdm;
                    object o = dtJCB.Compute("Sum([" + item + "])", "");
                    double mj = 0;
                    double.TryParse(o.ToString(), out mj);
                    dr[item] = mj;
                }
                dtResult.Rows.InsertAt(dr, 0);
            }
            return dtResult;
        }

        private DataTable OutTableBFXHDL(DataTable dtJCB, string shpField, string groupField)
        {
            DataTable dtResult = new DataTable();
            dtResult.Columns.Add(shpField, typeof(string));
            dtResult.Columns.Add(groupField, typeof(string));
            dtResult.Columns.Add("08H2A", typeof(double));
            dtResult.Columns.Add("0810A", typeof(double));
            dtResult.Columns.Add("1104A", typeof(double));
            dtResult.Columns.Add("1107A", typeof(double));
            dtResult.Columns.Add("201A", typeof(double));
            dtResult.Columns.Add("202A", typeof(double));
            dtResult.Columns.Add("203A", typeof(double));
            dtResult.Columns.Add("20", typeof(double), "[201A] + [202A] + [203A]");
            dtResult.Columns["20"].SetOrdinal(6);

            var lstshp = dtJCB.AsEnumerable().GroupBy(a => a.Field<string>(shpField)).ToList();
            var lstGroup = dtJCB.AsEnumerable().GroupBy(a => a.Field<string>(groupField)).ToList().
                Union(dtJCB.AsEnumerable().GroupBy(a => a.Field<string>(groupField).Substring(0, 9)).ToList()).
                Union(dtJCB.AsEnumerable().GroupBy(a => a.Field<string>(groupField).Substring(0, 6)).ToList());
            string dwdm = "";
            foreach (var ishp in lstshp)
            {
                foreach (var igroup in lstGroup)
                {
                    if (igroup.Key.Length == 6)
                        dwdm = igroup.Key;
                    DataRow dr = dtResult.NewRow();
                    dr[shpField] = ishp.Key;
                    dr[groupField] = igroup.Key;
                    object o = dtJCB.Compute("Sum([08H2A])", shpField + " = '" + ishp.Key + "' And " + groupField + " Like '" + igroup.Key + "%'");
                    double mj = 0;
                    double.TryParse(o.ToString(), out mj);
                    dr["08H2A"] = mj;
                    o = dtJCB.Compute("Sum([0810A])", shpField + " = '" + ishp.Key + "' And " + groupField + " Like '" + igroup.Key + "%'");
                    double.TryParse(o.ToString(), out mj);
                    dr["0810A"] = mj;
                    o = dtJCB.Compute("Sum([1104A])", shpField + " = '" + ishp.Key + "' And " + groupField + " Like '" + igroup.Key + "%'");
                    double.TryParse(o.ToString(), out mj);
                    dr["1104A"] = mj;
                    o = dtJCB.Compute("Sum([1107A])", shpField + " = '" + ishp.Key + "' And " + groupField + " Like '" + igroup.Key + "%'");
                    double.TryParse(o.ToString(), out mj);
                    dr["1107A"] = mj;
                    o = dtJCB.Compute("Sum([ZMJ])", shpField + " = '" + ishp.Key + "' And " + groupField + " Like '" + igroup.Key + "%' And CZCSXM = '201A'");
                    double.TryParse(o.ToString(), out mj);
                    dr["201A"] = mj;
                    o = dtJCB.Compute("Sum([ZMJ])", shpField + " = '" + ishp.Key + "' And " + groupField + " Like '" + igroup.Key + "%' And CZCSXM = '202A'");
                    double.TryParse(o.ToString(), out mj);
                    dr["202A"] = mj;
                    o = dtJCB.Compute("Sum([ZMJ])", shpField + " = '" + ishp.Key + "' And " + groupField + " Like '" + igroup.Key + "%' And CZCSXM = '203A'");
                    double.TryParse(o.ToString(), out mj);
                    dr["203A"] = mj;
                    dtResult.Rows.Add(dr);
                }
            }

            DataView dv = new DataView(dtResult);
            dv.Sort = groupField;
            dtResult = dv.ToTable();

            if (lstshp.Count > 1 && !string.IsNullOrWhiteSpace(dwdm))
            {
                DataRow dr = dtResult.NewRow();
                for (int i = 2; i < dtResult.Columns.Count; i++)
                {
                    string item = dtResult.Columns[i].ColumnName;
                    dr[shpField] = "合计";
                    dr[groupField] = dwdm;
                    object o = dtResult.Compute("Sum([" + item + "])", "" + groupField + "='" + dwdm + "'");
                    double mj = 0;
                    double.TryParse(o.ToString(), out mj);
                    dr[item] = mj;
                }
                dtResult.Rows.InsertAt(dr, 0);
            }

            return dtResult;
        }

        private DataTable OutTableFRDMJ(DataTable dtJCB, string shpField, string groupField, string[] staFields)
        {
            DataTable dtResult = new DataTable();
            dtResult.Columns.Add(shpField, typeof(string));
            dtResult.Columns.Add("XH", typeof(string));
            dtResult.Columns.Add("ZLDWDM", typeof(string));
            dtResult.Columns.Add("QSDWDM", typeof(string));
            foreach (string item in staFields)
            {
                dtResult.Columns.Add(item, typeof(double));
            }

            var lstshp = dtJCB.AsEnumerable().GroupBy(a => a.Field<string>(shpField)).ToList();
            var lstGroupZL = dtJCB.AsEnumerable().GroupBy(a => a.Field<string>("ZLDWDM")).ToList();
            //Union(dtJCB.AsEnumerable().GroupBy(a => a.Field<string>("ZLDWDM").Substring(0, 9)).ToList()).
            //Union(dtJCB.AsEnumerable().GroupBy(a => a.Field<string>("ZLDWDM").Substring(0, 6)).ToList());
            var lstGroupQS = dtJCB.AsEnumerable().GroupBy(a => a.Field<string>("QSDWDM")).ToList();
            //Union(dtJCB.AsEnumerable().GroupBy(a => a.Field<string>("QSDWDM").Substring(0, 9)).ToList()).
            //Union(dtJCB.AsEnumerable().GroupBy(a => a.Field<string>("QSDWDM").Substring(0, 6)).ToList());
            string dwdm = "";
            int n = 1;
            foreach (var ishp in lstshp)
            {
                foreach (var igroupZL in lstGroupZL)
                {
                    foreach (var igroupQS in lstGroupQS)
                    {
                        if (igroupZL.Key.Length == 6)
                            dwdm = igroupZL.Key;
                        DataRow dr = dtResult.NewRow();
                        foreach (var item in staFields)
                        {
                            dr[shpField] = ishp.Key;
                            dr["ZLDWDM"] = igroupZL.Key;
                            dr["QSDWDM"] = igroupQS.Key;
                            object o = dtJCB.Compute("Sum([" + item + "])", shpField + " = '" + ishp.Key + "' And ZLDWDM Like '" + igroupZL.Key + "%' And FRDBS = '1' And QSDWDM LIKE '" + igroupQS.Key + "%'");
                            double mj = 0;
                            double.TryParse(o.ToString(), out mj);
                            dr[item] = mj;
                        }
                        dtResult.Rows.Add(dr);
                    }

                }
            }

            DataView dv = new DataView(dtResult);
            dv.Sort = groupField;
            dtResult = dv.ToTable();
            for (int i = 0; i < dtResult.Rows.Count; i++)
            {
                dtResult.Rows[i]["XH"] = i + 1;
            }
            //dtResult.PrimaryKey = new DataColumn[] { dtResult.Columns[groupField] };

            if (lstshp.Count > 1)
            {
                DataRow dr = dtResult.NewRow();
                foreach (var item in staFields)
                {
                    dr[shpField] = "合计";
                    //dr[groupField] = dwdm;
                    object o = dtJCB.Compute("Sum([" + item + "])", "FRDBS='1'");
                    double mj = 0;
                    double.TryParse(o.ToString(), out mj);
                    dr[item] = mj;
                }
                dtResult.Rows.InsertAt(dr, 0);
            }
            return dtResult;
        }


        private DataTable OutTableSXMJCZC(DataTable dtJCB, string shpField, string groupField, string sxField, string[] sxValues, string[] staFields, string[] sumFields = null, Boolean changeOrder = false, sumType sumtype = sumType.SX)
        {
            DataTable dtResult = new DataTable();
            dtResult.Columns.Add(shpField, typeof(string));
            dtResult.Columns.Add("XH", typeof(string));
            dtResult.Columns.Add("ZLDWDM", typeof(string));
            dtResult.Columns.Add("QSDWDM", typeof(string));

            //dtResult.Columns.Add(groupField, typeof(string));
            if (changeOrder == false)
            {
                foreach (string val in sxValues)
                {
                    foreach (string item in staFields)
                    {
                        dtResult.Columns.Add(val + item, typeof(double));
                    }
                }
            }


            var lstshp = dtJCB.AsEnumerable().GroupBy(a => a.Field<string>(shpField)).ToList();
            var lstGroupZL = dtJCB.AsEnumerable().GroupBy(a => a.Field<string>("ZLDWDM")).ToList();
            //Union(dtJCB.AsEnumerable().GroupBy(a => a.Field<string>("ZLDWDM").Substring(0, 9)).ToList()).
            //Union(dtJCB.AsEnumerable().GroupBy(a => a.Field<string>("ZLDWDM").Substring(0, 6)).ToList());
            var lstGroupQS = dtJCB.AsEnumerable().GroupBy(a => a.Field<string>("QSDWDM")).ToList();
            //Union(dtJCB.AsEnumerable().GroupBy(a => a.Field<string>("QSDWDM").Substring(0, 9)).ToList()).
            //Union(dtJCB.AsEnumerable().GroupBy(a => a.Field<string>("QSDWDM").Substring(0, 6)).ToList());

            //var lstshp = dtJCB.AsEnumerable().GroupBy(a => a.Field<string>(shpField)).ToList();
            //var lstGroup = dtJCB.AsEnumerable().GroupBy(a => a.Field<string>(groupField)).ToList().
            //    Union(dtJCB.AsEnumerable().GroupBy(a => a.Field<string>(groupField).Substring(0, 9)).ToList()).
            //    Union(dtJCB.AsEnumerable().GroupBy(a => a.Field<string>(groupField).Substring(0, 6)).ToList());
            string dwdm = "";
            //int index = 1;
            foreach (var ishp in lstshp)
            {
                foreach (var igroupZL in lstGroupZL)
                {
                    foreach (var igroupQS in lstGroupQS)
                    {
                        if (igroupZL.Key.Length == 6)
                            dwdm = igroupZL.Key;
                        DataRow dr = dtResult.NewRow();
                        if (changeOrder == false)
                        {
                            foreach (string val in sxValues)
                            {
                                foreach (string item in staFields)
                                {
                                    dr[shpField] = ishp.Key;
                                    dr["ZLDWDM"] = igroupZL.Key;
                                    dr["QSDWDM"] = igroupQS.Key;
                                    object o = dtJCB.Compute("Sum([" + item + "])", shpField + " = '" + ishp.Key + "' And ZLDWDM Like '" + igroupZL.Key + "%' And QSDWDM Like '" + igroupQS.Key + "%' AND FRDBS='1'");
                                    double mj = 0;
                                    double.TryParse(o.ToString(), out mj);
                                    dr[val + item] = mj;
                                }
                            }
                        }
                        dtResult.Rows.Add(dr);
                    }
                }
            }
            if (sumFields != null)
            {
                switch (sumtype)
                {
                    case sumType.SX:
                        foreach (string item in sumFields)
                        {
                            string sumField = item.Split('=')[0].Trim();
                            string subField = item.Split('=')[1].Trim();
                            string[] subFields = subField.Split('+');

                            foreach (string val in staFields)
                            {
                                List<string> flds = new List<string>();
                                foreach (string fld in subFields)
                                {
                                    if (changeOrder == false)
                                        flds.Add("[" + fld.Trim() + val + "]");
                                    else
                                        flds.Add("[" + val + fld.Trim() + "]");
                                }

                                if (changeOrder == false)
                                {
                                    dtResult.Columns.Add(sumField + val, typeof(double), string.Join(" + ", flds));
                                    dtResult.Columns[sumField + val].SetOrdinal(dtResult.Columns.IndexOf(subFields[0].Trim() + val));
                                }
                                else
                                {
                                    dtResult.Columns.Add(val + sumField, typeof(double), string.Join(" + ", flds));
                                    dtResult.Columns[val + sumField].SetOrdinal(dtResult.Columns.IndexOf(val + subFields[0].Trim()));
                                }
                            }
                        }
                        break;
                    case sumType.DL:
                        foreach (string item in sumFields)
                        {
                            string sumField = item.Split('=')[0].Trim();
                            string subField = item.Split('=')[1].Trim();
                            string[] subFields = subField.Split('+');

                            foreach (string val in sxValues)
                            {
                                List<string> flds = new List<string>();
                                foreach (string fld in subFields)
                                {
                                    if (changeOrder == false)
                                        flds.Add("[" + val + fld.Trim() + "]");
                                    else
                                        flds.Add("[" + fld.Trim() + val + "]");
                                }

                                if (changeOrder == false)
                                {
                                    dtResult.Columns.Add(val + sumField, typeof(double), string.Join(" + ", flds));
                                    dtResult.Columns[val + sumField].SetOrdinal(dtResult.Columns.IndexOf(val + subFields[0].Trim()));
                                }
                                else
                                {
                                    dtResult.Columns.Add(sumField + val, typeof(double), string.Join(" + ", flds));
                                    dtResult.Columns[sumField + val].SetOrdinal(dtResult.Columns.IndexOf(subFields[0].Trim() + val));
                                }
                            }
                        }
                        break;
                    case sumType.SXAndFirstDL:
                        foreach (string item in sumFields)
                        {
                            string sumField = item.Split('=')[0].Trim();
                            string subField = item.Split('=')[1].Trim();
                            string[] subFields = subField.Split('+');
                            string val = staFields[0];

                            List<string> flds = new List<string>();
                            foreach (string fld in subFields)
                            {
                                if (changeOrder == false)
                                    flds.Add("[" + fld.Trim() + val + "]");
                                else
                                    flds.Add("[" + val + fld.Trim() + "]");
                            }

                            if (changeOrder == false)
                            {
                                dtResult.Columns.Add(sumField + val, typeof(double), string.Join(" + ", flds));
                                dtResult.Columns[sumField + val].SetOrdinal(dtResult.Columns.IndexOf(subFields[0].Trim() + val));
                            }
                            else
                            {
                                dtResult.Columns.Add(val + sumField, typeof(double), string.Join(" + ", flds));
                                dtResult.Columns[val + sumField].SetOrdinal(dtResult.Columns.IndexOf(val + subFields[0].Trim()));
                            }
                        }
                        break;
                    default:
                        break;
                }

            }
            DataView dv = new DataView(dtResult);
            dv.Sort = groupField;
            dtResult = dv.ToTable();
            //dtResult.PrimaryKey = new DataColumn[] { dtResult.Columns[groupField] };
            for (int i = 0; i < dtResult.Rows.Count; i++)
            {
                dtResult.Rows[i]["XH"] = i + 1;
            }
            if (lstshp.Count > 1 && !string.IsNullOrWhiteSpace(dwdm))
            {
                DataRow dr = dtResult.NewRow();
                foreach (var item in staFields)
                {
                    dr[shpField] = "合计";
                    dr[groupField] = dwdm;
                    object o = dtResult.Compute("Sum([" + item + "])", "" + groupField + "='" + dwdm + "'");
                    double mj = 0;
                    double.TryParse(o.ToString(), out mj);
                    dr[item] = mj;
                }
                dtResult.Rows.InsertAt(dr, 0);
            }

            return dtResult;
        }

        private DataTable OutTableFRDYJFLQSXZ(DataTable dtJCB, string shpField)
        {
            string[] staFields = { "ZMJ", "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };
            DataTable dtResult = new DataTable();
            dtResult.Columns.Add(shpField, typeof(string));
            dtResult.Columns.Add("XH", typeof(int));
            dtResult.Columns.Add("ZLDWDM", typeof(string));
            dtResult.Columns.Add("QSDWDM", typeof(string));
            foreach (string item in staFields)
            {
                dtResult.Columns.Add(item, typeof(double));
                dtResult.Columns.Add("G" + item, typeof(double));
                dtResult.Columns.Add("J" + item, typeof(double));
            }
            var lstshp = dtJCB.AsEnumerable().GroupBy(a => a.Field<string>(shpField)).ToList();
            var xdm = from t in dtJCB.AsEnumerable()
                      group t by new { t1 = t.Field<string>("ZLDWDM"), t2 = t.Field<string>("QSDWDM") } into m
                      select new
                      {
                          zl = m.Key.t1,
                          qs = m.Key.t2
                      };
            var zdm = from t in dtJCB.AsEnumerable()
                      group t by new { t1 = t.Field<string>("ZLDWDM").Substring(0, 9), t2 = t.Field<string>("QSDWDM").Substring(0, 9) } into m
                      select new
                      {
                          zl = m.Key.t1,
                          qs = m.Key.t2
                      };
            var cdm = from t in dtJCB.AsEnumerable()
                      group t by new { t1 = t.Field<string>("ZLDWDM").Substring(0, 6), t2 = t.Field<string>("QSDWDM").Substring(0, 6) } into m
                      select new
                      {
                          zl = m.Key.t1,
                          qs = m.Key.t2
                      };
            var lstGroup = xdm.Union(zdm).Union(cdm);
            string dwdm = "";
            foreach (var ishp in lstshp)
            {
                foreach (var igroup in lstGroup)
                {
                    if (igroup.zl != igroup.qs)
                    {
                        if (igroup.zl.Length == 6)
                            dwdm = igroup.zl;
                        DataRow dr = dtResult.NewRow();
                        foreach (var item in staFields)
                        {
                            dr[shpField] = ishp.Key;
                            dr["ZLDWDM"] = igroup.zl;
                            dr["QSDWDM"] = igroup.qs;
                            object o = dtJCB.Compute("Sum([" + item + "])", shpField + " = '" + ishp.Key + "' And ZLDWDM Like '" + igroup.zl + "%' And QSDWDM Like '" + igroup.qs + "%' And FRDBS = '1'");
                            double mj = 0;
                            double.TryParse(o.ToString(), out mj);
                            dr[item] = mj;
                            o = dtJCB.Compute("Sum([" + item + "])", shpField + " = '" + ishp.Key + "' And ZLDWDM Like '" + igroup.zl + "%' And QSDWDM Like '" + igroup.qs + "%' And FRDBS = '1' And (QSXZ Like '1%' Or QSXZ Like '2%')");
                            double.TryParse(o.ToString(), out mj);
                            dr["G" + item] = mj;
                            o = dtJCB.Compute("Sum([" + item + "])", shpField + " = '" + ishp.Key + "' And ZLDWDM Like '" + igroup.zl + "%' And QSDWDM Like '" + igroup.qs + "%' And FRDBS = '1' And (QSXZ Like '3%' Or QSXZ Like '4%')");
                            double.TryParse(o.ToString(), out mj);
                            dr["J" + item] = mj;
                        }
                        dtResult.Rows.Add(dr);
                    }
                }
            }

            DataView dv = new DataView(dtResult);
            dv.Sort = "ZLDWDM,QSDWDM";
            dtResult = dv.ToTable();
            //dtResult.PrimaryKey = new DataColumn[] { dtResult.Columns[groupField] };

            if (lstshp.Count > 1 && !string.IsNullOrWhiteSpace(dwdm))
            {
                DataRow dr = dtResult.NewRow();
                foreach (var item in staFields)
                {
                    dr[shpField] = "合计";
                    dr["ZLDWDM"] = dwdm;
                    object o = dtJCB.Compute("Sum([" + item + "])", "");
                    double mj = 0;
                    double.TryParse(o.ToString(), out mj);
                    dr[item] = mj;
                }
                dtResult.Rows.InsertAt(dr, 0);
            }

            for (int i = 0; i < dtResult.Rows.Count; i++)
            {
                dtResult.Rows[i]["XH"] = i + 1;
            }
            return dtResult;
        }

    }
}
