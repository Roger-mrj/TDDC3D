using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geometry;

using RCIS.GISCommon;
using RCIS.Utility;

namespace RCIS.MapTool
{
    public partial class Absolutely2GeomtryFrm : Form
    {
        public Absolutely2GeomtryFrm()
        {
            InitializeComponent();
        }

        private esriGeometryType mGT;
        private IGeometry m_geometry;
        public IGeometry InputGeometry
        {
            set
            {
                this.m_geometry = value;
            }
        }
        public IGeometry OutputGeometry
        {
            get
            {
                return this.m_geometry;
            }
        }

        public Absolutely2GeomtryFrm(esriGeometryType pGT)
        {
            InitializeComponent();
            this.mGT = pGT;
        }

        private void Absolutely2GeomtryFrm_Load(object sender, EventArgs e)
        {
            if (this.m_geometry != null)
            {
                if (this.m_geometry is IPoint)
                {
                    this.rbPt.Checked = true;
                }
                else if (this.m_geometry is IPolyline)
                {
                    this.rbLine.Checked = true;
                }
                else if (this.m_geometry is IPolygon)
                {
                    this.rbPoly.Checked = true;
                }
                this.cbHasZ.Checked = (this.m_geometry as IZAware).ZAware;
                this.rbPt.Enabled = false;
                this.rbLine.Enabled = false;
                this.rbPoly.Enabled = false;
                this.cbHasZ.Enabled = false;

                this.tbPtList.Text = "";
                this.AppendGeometry(this.m_geometry);
            }
            else
            {
                if (esriGeometryType.esriGeometryPoint == this.mGT)
                {
                    this.rbPt.Enabled = true;
                    this.rbLine.Enabled = false;
                    this.rbPoly.Enabled = false;
                }
                else if (esriGeometryType.esriGeometryPolyline == this.mGT)
                {
                    this.rbPt.Enabled = true;
                    this.rbLine.Enabled = true;
                    this.rbPoly.Enabled = false;
                }
                else if (esriGeometryType.esriGeometryPolygon == this.mGT)
                {
                    this.rbPt.Enabled = true;
                    this.rbLine.Enabled = true;
                    this.rbPoly.Enabled = true;

                    this.rbPoly.Checked = true;
                }
            }
        }

        private void btnReadFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "点数据文件(*.*)|*.*";
            ofd.CheckFileExists = true;
            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                IPointCollection aPC = GeometryHelper.ReadFromFile(ofd.FileName
                    , this.cbHasZ.Checked, this.cbHasPtNO.Checked, ',');
                this.AppendGeometry(aPC as IGeometry);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string[] lines = this.tbPtList.Lines;
            if (lines.Length <= 0)
            {
                MessageBox.Show("没有输入坐标数据", "坐标编辑",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            object missing1 = Type.Missing;
            object missing2 = Type.Missing;

            IGeometry resultGeom = null;
            if (this.rbPt.Checked)
            {
                IPoint aPt = GeometryHelper.ParsePoint(lines[0], false, this.cbHasZ.Checked, ',');
                resultGeom = aPt;
            }
            else if (this.rbPoly.Checked)
            {
                IPointCollection pGonCol1 = new PolygonClass(); //拓扑正确的图形   
                for (int li = 0; li < lines.Length; li++)
                {
                    string aLine = lines[li];
                    IPoint aPt = GeometryHelper.ParsePoint(aLine, false, this.cbHasZ.Checked, ',');
                    if (aPt != null && !aPt.IsEmpty)
                    {
                        pGonCol1.AddPoint(aPt, ref missing1, ref missing2);
                    }
                }
                ITopologicalOperator pToper = pGonCol1 as ITopologicalOperator;
                pToper.Simplify();
                IPolygon pPolygon = pGonCol1 as IPolygon;
                resultGeom = pPolygon as IGeometry;
            }
            else if (this.rbLine.Checked)
            {
                IPointCollection pGonCol1 = new Polyline(); //拓扑正确的图形   
                for (int li = 0; li < lines.Length; li++)
                {
                    string aLine = lines[li];
                    if (aLine.Trim() == "")
                    {
                        continue;
                    }
                    IPoint aPt = GeometryHelper.ParsePoint(aLine, false, this.cbHasZ.Checked, ',');
                    if (aPt != null && !aPt.IsEmpty)
                    {
                        pGonCol1.AddPoint(aPt, ref missing1, ref missing2);
                    }
                }
                resultGeom = pGonCol1 as IPolyline;
            }
            //else if (this.rbPoly.Checked || this.rbLine.Checked)
            //{
            //    if (this.rbLine.Checked)
            //    {
            //        resultGeom = new PolylineClass();
            //    }
            //    else resultGeom = new PolygonClass();
            //    IPointCollection ptCol = resultGeom as IPointCollection;
            //    for (int li = 0; li < lines.Length; li++)
            //    {
            //        string aLine = lines[li];
            //        IPoint aPt = GeometryHelper.ParsePoint(aLine, false, this.cbHasZ.Checked, ',');
            //        if (aPt != null && !aPt.IsEmpty)
            //        {
            //            ptCol.AddPoints(1, ref aPt);
            //        }
            //    }
            //}


            if (resultGeom == null || resultGeom.IsEmpty)
            {
                MessageBox.Show("不能正确构建图形数据!", "坐标编辑"
                , MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //else
            //{
            //    if (resultGeom is IPolygon)
            //    {
            //        (resultGeom as IPolygon).Close();

            //    }
            //    (resultGeom as ITopologicalOperator).Simplify();
            //}
            this.m_geometry = resultGeom;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void AppendGeometry(IGeometry pGeom)
        {
            if (pGeom == null || pGeom.IsEmpty) return;
            string[] lines = this.tbPtList.Lines;
            if (lines.Length > 0)
            {
                string lastline = lines[lines.Length - 1];
                if (!lastline.EndsWith("\n"))
                    this.tbPtList.Text += "\n";
            }
            if (pGeom is IPoint)
            {
                IPoint aPt = (pGeom as IPoint);
                string aPtLine = GeometryHelper.FormatPoint(aPt, this.cbHasZ.Checked, ',');
                if (aPtLine != null)
                    this.tbPtList.Text += aPtLine;
            }
            else if (pGeom is IPointCollection)
            {
                IPointCollection ptCol = pGeom as IPointCollection;
                StringBuilder aBuilder = new StringBuilder();
                for (int pi = 0; pi < ptCol.PointCount; pi++)
                {
                    IPoint aPt = ptCol.get_Point(pi);
                    string aLine = GeometryHelper.FormatPoint(aPt, this.cbHasZ.Checked, ',');
                    aBuilder.Append(aLine).Append("\n");
                }
                this.tbPtList.Text += aBuilder.ToString();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

    }
}