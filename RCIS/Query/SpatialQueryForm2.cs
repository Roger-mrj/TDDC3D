using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Controls;


using sycCommonLib;
namespace RCIS.Query
{
    public partial class SpatialQueryForm2 : Form
    {
        public SpatialQueryForm2()
        {
            InitializeComponent();
        }
        private sycCommonFuns m_CommonClass;
        public IGeometry m_SelGeo;
        public IPolygon m_bufferPolygon;
        public IElement m_CreateEle;		    //对应的元素

        public AxMapControl m_MapControl;
        private void SpatialQueryForm2_Load(object sender, EventArgs e)
        {
            m_CommonClass = new sycCommonFuns();
        }

        public class sycFieldInfo		//该类被FillDatagrid使用
        {
            public sycFieldInfo() { }

            public string sDispName;
            public string sFieldName;
        }

        public class otherInfoClass
        {
            public otherInfoClass() { }

            public string sType;      //交，内
            public double dSumArea, dInsideArea, dOutsideArea;
        }

        private double m_dBuffer;
        private ArrayList m_Result;		//存放每次产生的查询结果,IFeature
        private ArrayList m_OtherInfo;   //自己增加的字段内容: 关系，内面积，外面积，总面积
        private ArrayList m_Fields;		    //存放被查询类的字段,dataGrid1用
        private void FillDatagrid(IFeatureClass myFeatCls, ArrayList myResult)
        {
            //用myResult中的内容填充dataGrid1:

            //如果分析区域是面同时分析图层也是面、在dataGrid1中增加列: 关系(内,交)+内面积+外面积
            IPolygon needPolygon = null;
            if (m_dBuffer < 0.001)
            {
                if (m_SelGeo is IPolygon)
                    needPolygon = m_SelGeo as IPolygon;
            }
            else
            {
                needPolygon = m_bufferPolygon;
            }
            m_OtherInfo = new ArrayList();
            if (needPolygon != null && m_selFeatCls.ShapeType == esriGeometryType.esriGeometryPolygon)
            {
                IGeometry pSource = needPolygon as IGeometry;
                for (int i = 0; i < m_Result.Count; i++)
                {
                    otherInfoClass info = new otherInfoClass();

                    IFeature feat = m_Result[i] as IFeature;
                    IGeometry pTargetGeo = feat.ShapeCopy;
                    pSource.SpatialReference = feat.Shape.SpatialReference;
                    IPolygon pTargetPolygon = pTargetGeo as IPolygon;
                    pTargetPolygon.Close();
                    IGeometry pResultGeo = ((ITopologicalOperator)pSource).Intersect(pTargetGeo, esriGeometryDimension.esriGeometry2Dimension);
                    IPolygon newPolygon = pResultGeo as IPolygon;
                    if (newPolygon == null)
                    {
                        info.sType = "U/N";
                        info.dInsideArea = Math.Abs(((IArea)pTargetPolygon).Area);
                        info.dOutsideArea = -1.0;
                        info.dInsideArea = -1.0;
                    }
                    else
                    {
                        double dSumArea = Math.Abs(((IArea)pTargetPolygon).Area);
                        double dJArea = Math.Abs(((IArea)newPolygon).Area);
                        double dOtherArea = dSumArea - dJArea;

                        string sType = "U/A";
                        double dd = Math.Abs(dJArea - dSumArea);
                        if (dd < 0.01)
                        {
                            info.sType = "内";
                            info.dSumArea = dSumArea;
                            info.dInsideArea = dSumArea;
                            info.dOutsideArea = 0.0;
                        }
                        else
                        {
                            info.sType = "交";
                            info.dSumArea = dSumArea;
                            info.dInsideArea = dJArea;
                            info.dOutsideArea = dSumArea - dJArea;
                        }
                    }
                    m_OtherInfo.Add(info);
                }
            }


            dataGrid1.TableStyles.Clear();
            //[01] 得到字段的列表:
            ArrayList myField = new ArrayList();
            IFields flds = myFeatCls.Fields;
            for (int i = 0; i < flds.FieldCount; i++)
            {
                IField curFld = flds.get_Field(i);
                string sName = curFld.Name.Trim().ToUpper();
                string sAliasName = curFld.AliasName.Trim().ToUpper();
                if ((sName.Equals("SHAPE") == true) ||
                    (sName.Equals("SHAPE_LENGTH") == true) ||
                    (sName.Equals("SHAPE_AREA") == true))
                    continue;

                sycFieldInfo info = new sycFieldInfo();
                info.sDispName = sAliasName;
                info.sFieldName = sName;
                myField.Add(info);
            }
            m_Fields = myField;


            //[02] 填充表:
            DataTable myTable = new DataTable("查询信息表");
            myTable.Columns.Add(new DataColumn("序号"));

            //在表内添加自己的字段:
            if (m_OtherInfo.Count != 0)
            {
                myTable.Columns.Add(new DataColumn("关系"));
                myTable.Columns.Add(new DataColumn("内面积"));
                myTable.Columns.Add(new DataColumn("外面积"));
                myTable.Columns.Add(new DataColumn("总面积"));
            }
            for (int i = 0; i < myField.Count; i++)
            {
                sycFieldInfo info = myField[i] as sycFieldInfo;
                string sFldDispName = info.sDispName;
                string sFldName = info.sDispName.ToUpper();
                myTable.Columns.Add(new DataColumn(sFldDispName));
            }
            DataSet myDataSet = new DataSet("MyDataSet");
            myDataSet.Tables.Add(myTable);
            dataGrid1.SetDataBinding(myDataSet, "查询信息表");

            DataGridTableStyle dgt = new DataGridTableStyle();
            dgt.MappingName = "查询信息表";
            dgt.ReadOnly = true;
            dgt.RowHeadersVisible = false;
            dgt.AlternatingBackColor = System.Drawing.Color.AliceBlue;
            dataGrid1.TableStyles.Add(dgt);
            dataGrid1.CaptionVisible = false;
            dataGrid1.ReadOnly = true;

            for (int i = 0; i < myResult.Count; i++)
            {
                DataRow row = myTable.NewRow();

                //加自己的信息:
                if (m_OtherInfo.Count != 0)
                {
                    otherInfoClass info = m_OtherInfo[i] as otherInfoClass;
                    row["关系"] = info.sType;
                    row["内面积"] = info.dInsideArea.ToString("F2");
                    row["外面积"] = info.dOutsideArea.ToString("F2");
                    row["总面积"] = info.dSumArea.ToString("F2");
                }

                //加IFeature信息:
                int nXH = i + 1;
                row["序号"] = nXH.ToString();
                IFeature curFeat = myResult[i] as IFeature;
                for (int j = 0; j < myField.Count; j++)
                {
                    sycFieldInfo info = myField[j] as sycFieldInfo;
                    string sFldName = info.sFieldName;
                    string sAliasName = info.sDispName;

                    int nPos = curFeat.Fields.FindField(sFldName);
                    object oo = curFeat.get_Value(nPos);
                    string sValue = "";

                    if (!(oo is System.DBNull))
                        sValue = oo.ToString();

                    row[sAliasName] = sValue;
                }
                myTable.Rows.Add(row);
            }
            myTable.AcceptChanges();


            //用m_Fields中的内容填充comboBox3[包括自己的字段、如果有]:
            this.cmbStatsfield.Properties.Items.Clear();
            this.cmbCalField.Properties.Items.Clear();


            this.cmbStatsfield.Properties.Items.Add("序号");   //统计字段
            this.cmbCalField.Properties.Items.Add("序号");   //计算字段
            if (m_OtherInfo.Count != 0)
            {
                this.cmbStatsfield.Properties.Items.Add("关系");
                this.cmbStatsfield.Properties.Items.Add("内面积");
                this.cmbStatsfield.Properties.Items.Add("外面积");
                this.cmbStatsfield.Properties.Items.Add("总面积");

                this.cmbCalField.Properties.Items.Add("关系");
                this.cmbCalField.Properties.Items.Add("内面积");
                this.cmbCalField.Properties.Items.Add("外面积");
                this.cmbCalField.Properties.Items.Add("总面积");
            }
            for (int i = 0; i < m_Fields.Count; i++)
            {
                sycFieldInfo info = m_Fields[i] as sycFieldInfo;
                string sFldDispName = info.sDispName;
                string sFldName = info.sDispName.ToUpper();

                this.cmbStatsfield.Properties.Items.Add(sFldDispName);   //统计字段
                this.cmbCalField.Properties.Items.Add(sFldDispName);   //计算字段
            }
            if (m_Fields.Count != 0)
            {
                this.cmbStatsfield.SelectedIndex = 0;
                this.cmbCalField.SelectedIndex = 0;
            }
        }

        private IFeatureClass m_selFeatCls;		//选择的待分析的特性类
        private IFeatureLayer m_selFeatLyr;

        private void SpatialQueryForm2_FormClosed(object sender, FormClosedEventArgs e)
        {

        }
        private string m_sSelLayerType;			//Point,Line,Area
        private Hashtable m_has;
        private void btnExecute_Click(object sender, EventArgs e)
        {
            //执行分析:
            IMap myMap = m_MapControl.ActiveView.FocusMap as IMap;
            IActiveView act = myMap as IActiveView;

            //检查基本参数
            if (m_SelGeo == null)
            {
                MessageBox.Show("先确定分析参数[RedLine?]  !");
                return;
            }

            m_dBuffer = 0.0;
            try
            {
                m_dBuffer = Convert.ToDouble(this.txtBuffer.Text);
            }
            catch (Exception E)
            {
                MessageBox.Show("Buffer参数:" + E.Message + " !");
                return;
            }

            bool bSingle = false;
            if (this.chkGoDirection.Checked == true)
                bSingle = true;

            if (m_has.Count == 0)
            {
                MessageBox.Show("选择待分析的层!");
                return;
            }
            string sSelLayerName = this.cmbLayers.Text.Trim();
            sSelLayerName = RCIS.Utility.OtherHelper.GetLeftName(sSelLayerName);
            if (sSelLayerName.Length == 0)
            {
                MessageBox.Show("选择待分析的图层!");
                return;
            }


            //查询类型:
            string sQryType = this.cmbQueryLx.Text.Trim();
            if (sQryType.Length == 0)
            {
                MessageBox.Show("选择查询类型!");
                return;
            }
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            m_Result = new ArrayList();

            object oo = m_has[sSelLayerName];
            IFeatureLayer selFeatLyr = oo as IFeatureLayer;
            IFeatureClass selFeatCls = selFeatLyr.FeatureClass;
            if (m_dBuffer < 0.001)
            {
                if (sQryType.Equals("交") == true)
                {
                    if (m_SelGeo is IPoint || m_SelGeo is IPolyline)
                    {
                        ISpatialFilter pFilter = new SpatialFilterClass();
                        string sFldName = selFeatCls.ShapeFieldName;
                        pFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                        pFilter.Geometry = m_SelGeo;
                        pFilter.GeometryField = sFldName;
                        pFilter.set_OutputSpatialReference(sFldName, myMap.SpatialReference);

                        IFeatureCursor pFeatCursor = selFeatLyr.Search(pFilter, false);
                        IFeature pFeat = pFeatCursor.NextFeature();
                        while (pFeat != null)
                        {
                            m_Result.Add(pFeat);
                            pFeat = pFeatCursor.NextFeature();
                        }
                    }
                    else if (m_SelGeo is IPolygon)
                    {
                        IPolyline pPolyline = new PolylineClass();
                        ISegmentCollection pCol = pPolyline as ISegmentCollection;
                        ISegmentCollection pCol2 = m_SelGeo as ISegmentCollection;
                        for (int i = 0; i < pCol2.SegmentCount; i++)
                        {
                            object o1 = Type.Missing;
                            ISegment pS = pCol2.get_Segment(i);
                            pCol.AddSegment(pS, ref o1, ref o1);
                        }

                        ISpatialFilter pFilter = new SpatialFilterClass();
                        string sFldName = selFeatCls.ShapeFieldName;
                        pFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                        pFilter.Geometry = (IGeometry)pPolyline;
                        pFilter.GeometryField = sFldName;
                        pFilter.set_OutputSpatialReference(sFldName, myMap.SpatialReference);

                        IFeatureCursor pFeatCursor = selFeatLyr.Search(pFilter, false);
                        IFeature pFeat = pFeatCursor.NextFeature();
                        while (pFeat != null)
                        {
                            m_Result.Add(pFeat);
                            pFeat = pFeatCursor.NextFeature();
                        }
                    }
                }
                if (sQryType.Equals("内") == true)
                {
                    if (m_SelGeo is IPolygon)
                    {
                        ISpatialFilter pFilter = new SpatialFilterClass();
                        string sFldName = selFeatCls.ShapeFieldName;
                        pFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                        pFilter.Geometry = (IGeometry)m_SelGeo;
                        pFilter.GeometryField = sFldName;
                        pFilter.set_OutputSpatialReference(sFldName, myMap.SpatialReference);

                        ArrayList result1 = new ArrayList();
                        IFeatureCursor pFeatCursor = selFeatLyr.Search(pFilter, false);
                        IFeature pFeat = pFeatCursor.NextFeature();
                        while (pFeat != null)
                        {
                            result1.Add(pFeat);
                            pFeat = pFeatCursor.NextFeature();
                        }

                        IPolyline pPolyline = new PolylineClass();
                        ISegmentCollection pCol = pPolyline as ISegmentCollection;
                        ISegmentCollection pCol2 = m_SelGeo as ISegmentCollection;
                        for (int i = 0; i < pCol2.SegmentCount; i++)
                        {
                            object o1 = Type.Missing;
                            ISegment pS = pCol2.get_Segment(i);
                            pCol.AddSegment(pS, ref o1, ref o1);
                        }
                        pFilter = new SpatialFilterClass();
                        sFldName = selFeatCls.ShapeFieldName;
                        pFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                        pFilter.Geometry = (IGeometry)pPolyline;
                        pFilter.GeometryField = sFldName;
                        pFilter.set_OutputSpatialReference(sFldName, myMap.SpatialReference);

                        ArrayList result2 = new ArrayList();
                        pFeatCursor = selFeatLyr.Search(pFilter, false);
                        pFeat = pFeatCursor.NextFeature();
                        while (pFeat != null)
                        {
                            result2.Add(pFeat);
                            pFeat = pFeatCursor.NextFeature();
                        }

                        for (int i = 0; i < result1.Count; i++)
                        {
                            IFeature feat1 = result1[i] as IFeature;
                            IRelationalOperator pRO = feat1.ShapeCopy as IRelationalOperator;
                            bool bExist = false;
                            for (int j = 0; j < result2.Count; j++)
                            {
                                IFeature feat2 = result2[j] as IFeature;
                                if (pRO.Equals(feat2.ShapeCopy) == true)
                                {
                                    bExist = true;
                                    break;
                                }
                            }
                            if (bExist == false)
                                m_Result.Add(feat1);
                        }
                    }
                    else
                    {
                        this.Cursor = System.Windows.Forms.Cursors.Default;
                        MessageBox.Show(null, "当BufferDist=0.0, 只有基础实体为面时、才可使用 ‘内’ 参数查询!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                if (sQryType.Equals("内和交") == true)
                {
                    ISpatialFilter pFilter = new SpatialFilterClass();
                    string sFldName = selFeatCls.ShapeFieldName;
                    pFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                    pFilter.Geometry = m_SelGeo;
                    pFilter.GeometryField = sFldName;
                    pFilter.set_OutputSpatialReference(sFldName, myMap.SpatialReference);

                    IFeatureCursor pFeatCursor = selFeatLyr.Search(pFilter, false);
                    IFeature pFeat = pFeatCursor.NextFeature();
                    while (pFeat != null)
                    {
                        m_Result.Add(pFeat);
                        pFeat = pFeatCursor.NextFeature();
                    }
                } //内和交
            }
            else
            {
                IPolygon bufferPolygon = null;
                if ((m_SelGeo.GeometryType == esriGeometryType.esriGeometryPolyline) &&
                    (bSingle == true))
                {
                    sycCommonFuns CommonClass = new sycCommonLib.sycCommonFuns();
                    IPolyline pLine = m_SelGeo as IPolyline;
                    bool bRet = CommonClass.syc_CreateSingleSideBuffer(pLine, m_dBuffer, ref bufferPolygon);
                    CommonClass.Dispose();
                    if (bRet == false)
                    {
                        this.Cursor = System.Windows.Forms.Cursors.Default;
                        MessageBox.Show("根据实体建立单边Buffer错误[Polyline,Buffer=" + m_dBuffer.ToString("F1") + "] !");
                        return;
                    }
                }
                else
                {
                    sycCommonFuns CommonClass = new sycCommonLib.sycCommonFuns();
                    bool bRet = CommonClass.syc_CreateBuffer(m_SelGeo, m_dBuffer, ref bufferPolygon);
                    CommonClass.Dispose();
                    if (bRet == false)
                    {
                        this.Cursor = System.Windows.Forms.Cursors.Default;
                        MessageBox.Show("根据实体建立Buffer错误[实体类型:" + m_SelGeo.GeometryType.ToString() + ",Buffer=" + m_dBuffer.ToString("F1") + "] !");
                        return;
                    }
                }
                m_bufferPolygon = bufferPolygon;

                if (sQryType.Equals("交") == true)
                {
                    IPolyline pPolyline = new PolylineClass();
                    ISegmentCollection pCol = pPolyline as ISegmentCollection;
                    ISegmentCollection pCol2 = bufferPolygon as ISegmentCollection;
                    for (int i = 0; i < pCol2.SegmentCount; i++)
                    {
                        object o1 = Type.Missing;
                        ISegment pS = pCol2.get_Segment(i);
                        pCol.AddSegment(pS, ref o1, ref o1);
                    }

                    ISpatialFilter pFilter = new SpatialFilterClass();
                    string sFldName = selFeatCls.ShapeFieldName;
                    pFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                    pFilter.Geometry = (IGeometry)pPolyline;
                    pFilter.GeometryField = sFldName;
                    pFilter.set_OutputSpatialReference(sFldName, myMap.SpatialReference);

                    IFeatureCursor pFeatCursor = selFeatLyr.Search(pFilter, false);
                    IFeature pFeat = pFeatCursor.NextFeature();
                    while (pFeat != null)
                    {
                        m_Result.Add(pFeat);
                        pFeat = pFeatCursor.NextFeature();
                    }
                } //"交"

                if (sQryType.Equals("内") == true)
                {
                    ISpatialFilter pFilter = new SpatialFilterClass();
                    string sFldName = selFeatCls.ShapeFieldName;
                    pFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                    pFilter.Geometry = (IGeometry)bufferPolygon;
                    pFilter.GeometryField = sFldName;
                    pFilter.set_OutputSpatialReference(sFldName, myMap.SpatialReference);

                    ArrayList result1 = new ArrayList();
                    IFeatureCursor pFeatCursor = selFeatLyr.Search(pFilter, false);
                    IFeature pFeat = pFeatCursor.NextFeature();
                    while (pFeat != null)
                    {
                        result1.Add(pFeat);
                        pFeat = pFeatCursor.NextFeature();
                    }
                    IPolyline pPolyline = new PolylineClass();
                    ISegmentCollection pSG = pPolyline as ISegmentCollection;
                    ISegmentCollection pSG2 = bufferPolygon as ISegmentCollection;
                    for (int i = 0; i < pSG2.SegmentCount; i++)
                    {
                        object o1 = Type.Missing;
                        ISegment pS = pSG2.get_Segment(i);
                        pSG.AddSegment(pS, ref o1, ref o1);
                    }
                    pFilter = new SpatialFilterClass();
                    sFldName = selFeatCls.ShapeFieldName;
                    pFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                    pFilter.Geometry = (IGeometry)pPolyline;
                    pFilter.GeometryField = sFldName;
                    pFilter.set_OutputSpatialReference(sFldName, myMap.SpatialReference);

                    ArrayList result2 = new ArrayList();
                    pFeatCursor = selFeatLyr.Search(pFilter, false);
                    pFeat = pFeatCursor.NextFeature();
                    while (pFeat != null)
                    {
                        result2.Add(pFeat);
                        pFeat = pFeatCursor.NextFeature();
                    }

                    for (int i = 0; i < result1.Count; i++)
                    {
                        IFeature feat1 = result1[i] as IFeature;
                        IRelationalOperator pRO = feat1.ShapeCopy as IRelationalOperator;
                        bool bExist = false;
                        for (int j = 0; j < result2.Count; j++)
                        {
                            IFeature feat2 = result2[j] as IFeature;
                            if (pRO.Equals(feat2.ShapeCopy) == true)
                            {
                                bExist = true;
                                break;
                            }
                        }
                        if (bExist == false)
                            m_Result.Add(feat1);
                    } //for(int i
                } //内

                if (sQryType.Equals("内和交") == true)
                {
                    ISpatialFilter pFilter = new SpatialFilterClass();
                    string sFldName = selFeatCls.ShapeFieldName;
                    pFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                    pFilter.Geometry = bufferPolygon;
                    pFilter.GeometryField = sFldName;
                    pFilter.set_OutputSpatialReference(sFldName, myMap.SpatialReference);

                    IFeatureCursor pFeatCursor = selFeatLyr.Search(pFilter, false);
                    IFeature pFeat = pFeatCursor.NextFeature();
                    while (pFeat != null)
                    {
                        m_Result.Add(pFeat);
                        pFeat = pFeatCursor.NextFeature();
                    }
                } //内和交
                //... ...
            }

            //[03] 用m_Result中的结果填充dataGrid1:
            m_selFeatCls = selFeatCls;
            m_selFeatLyr = selFeatLyr;
            FillDatagrid(selFeatCls, m_Result);
            this.xtraTabControl1.SelectedTabPageIndex = 1;

            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //交互绘制实体:

            //[01]: Draw类型: point,line,polygon
            string sDrawType = "";
            if (this.rgJHLX.SelectedIndex == 0)
            {
                sDrawType = "Point";
            }
            else if (this.rgJHLX.SelectedIndex == 1)
            {
                sDrawType = "Line";
            }
            else if (this.rgJHLX.SelectedIndex == 2)
            {
                sDrawType = "Polygon";
            }           
           
                
            if (sDrawType.Length == 0)
            {
                MessageBox.Show("确定类型: 点、线、面 ? ");
                return;
            }


            //[02]: ...
            m_SelGeo = null;
            this.Visible = false;
            //m_mapToolbar.RemoveAll();
            RedLineTool MyTool = new RedLineTool();
            MyTool.OnCreate(this.m_MapControl.Object);

            MyTool.m_UseForm = this;
            MyTool.m_sOper = "Draw";
            MyTool.m_sDrawType = sDrawType;
            //m_mapToolbar.AddItem(MyTool, -1, -1, true, 0, esriCommandStyles.esriCommandStyleIconAndText);
            m_MapControl.CurrentTool = MyTool;
        }

        private void radioGroup2_SelectedIndexChanged(object sender, EventArgs e)
        {
            IMap myMap = m_MapControl.ActiveView.FocusMap as IMap;
            IActiveView act = myMap as IActiveView;
            esriGeometryType type=esriGeometryType.esriGeometryPoint;    ;
            if (this.radioGroup2.SelectedIndex == 0)
            {
                type = esriGeometryType.esriGeometryPoint;               
            }
            else if (this.radioGroup2.SelectedIndex == 1)
            {
                type = esriGeometryType.esriGeometryPolyline;
            }
            else
            {
                type = esriGeometryType.esriGeometryPolygon;
            }
            //点:-->comboBox1:
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            
            Hashtable curHas = new Hashtable();
            for (int i = 0; i < myMap.LayerCount; i++)
            {
                ILayer lyr = myMap.get_Layer(i);
                IFeatureLayer featLyr = lyr as IFeatureLayer;
                if (featLyr != null)
                {
                    IFeatureClass featCls = featLyr.FeatureClass;
                    if (featCls.ShapeType == type)
                    {
                        string sLyrName = featLyr.Name;
                        curHas.Add(sLyrName, featLyr);
                    }
                }
            } //for(int i=0;...

            this.cmbLayers.Properties.Items.Clear();
            ICollection sKeys = curHas.Keys;
            foreach (string curKey in sKeys)
                this.cmbLayers.Properties.Items.Add(curKey);
            m_has = curHas;
            if (m_has.Count != 0)
                this.cmbLayers.SelectedIndex = 0;
            this.Cursor = System.Windows.Forms.Cursors.Default;

        }

        //统计输出
        private void simpleButton7_Click(object sender, EventArgs e)
        {
            //统计:按照comboBox3中的地段统计到Excel

            if (m_Result == null || m_Result.Count == 0)
            {
                MessageBox.Show("没待统计内容!");
                return;
            }
           
            string sTJFldName = cmbStatsfield.Text.Trim();
            if (sTJFldName.Length == 0)
            {
                MessageBox.Show("选择待统计字段! ");
                return;
            }

            string sJSFldName = this.cmbCalField.Text.Trim();
            if (sJSFldName.Length == 0)
            {
                MessageBox.Show("选择计算字段!");
                return;
            }

            //模版文件放置到执行文件上级目录的"\Template"目录下。
            string sXLSTemplate = Application.StartupPath + @"\空间分析模板文件.xls";
            string sExePath = Application.StartupPath;
            int nPos = sExePath.LastIndexOf('\\');
            if (nPos != -1)
            {
                string sNewPath = sExePath.Substring(0, nPos);
                sNewPath = sNewPath + @"\Template";
                string sNewFile = sNewPath + @"\空间分析模板文件.xls";
                sXLSTemplate = sNewFile;
            }
            if (System.IO.File.Exists(sXLSTemplate) == false)
            {
                MessageBox.Show("没发现需要的模板文件: " + sXLSTemplate + "!");
                return;
            }

            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            DataSet myDS = (DataSet)dataGrid1.DataSource;
            DataTable myTable = myDS.Tables["查询信息表"];
            sycExcelClass myExcel = new sycExcelClass();
            string sErrorInfo = "";
            bool bRet = myExcel.OutToExcel(sTJFldName, sJSFldName, myTable, sXLSTemplate, out sErrorInfo);
            if (bRet == false)
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
                MessageBox.Show("把分析结果输出到Excel模板时错误: " + sErrorInfo);
                return;
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void SpatialQueryForm2_FormClosing(object sender, FormClosingEventArgs e)
        {
            IGraphicsContainer myCon = m_MapControl.ActiveView.GraphicsContainer;
            myCon.DeleteAllElements();
            IActiveView act = m_MapControl.ActiveView.FocusMap as IActiveView;
            act.PartialRefresh(esriViewDrawPhase.esriViewBackground, null, act.Extent);
            this.m_MapControl.CurrentTool = null;
        }


        private int m_CurLine;

        private void dataGrid1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //确定列、更新成comboBox3的选择列:
                System.Windows.Forms.DataGrid.HitTestInfo hi;
                DataGrid grid = (DataGrid)sender;
                hi = grid.HitTest(e.X, e.Y);

                //Test if the clicked area was a cell.
                if (hi.Type == DataGrid.HitTestType.Cell)
                {
                    int nCol = hi.Column;
                    if (nCol != -1)
                    {
                        DataSet myDS = (DataSet)dataGrid1.DataSource;
                        DataTable myTable = myDS.Tables["查询信息表"];
                        DataColumnCollection myCols = myTable.Columns;
                        DataColumn curCol = myCols[nCol];
                        string sColName = curCol.ColumnName;

                        int nPos = this.cmbStatsfield.Properties.Items.IndexOf(sColName);
                        if (nPos != -1)
                            this.cmbStatsfield.SelectedIndex = nPos;
                    }
                    m_CurLine = hi.Row;
                }
                else m_CurLine = -1;
            }
            else m_CurLine = -1;
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            //Flash用户绘制的定位实体:
            if (m_SelGeo != null)
            {
                object oSymbol = Type.Missing;
                m_MapControl.FlashShape(m_SelGeo, 5, 300, oSymbol);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            //选择存在的实体:

            //[01]: Draw类型: point,line,polygon
            string sDrawType = "";
            if (this.rgJHLX.SelectedIndex == 0)
            {
                sDrawType = "Point";
            }
            else if (this.rgJHLX.SelectedIndex == 1)
            {
                sDrawType = "Line";
            }
            else if (this.rgJHLX.SelectedIndex == 2)
            {
                sDrawType = "Polygon";
            }        

            if (sDrawType.Length == 0)
            {
                MessageBox.Show("确定类型: 点、线、面 ? ");
                return;
            }


            //[02]: ...
            m_SelGeo = null;
            this.Visible = false;
            //m_mapToolbar.RemoveAll();
            RedLineTool MyTool = new RedLineTool();
            MyTool.m_UseForm = this;
            MyTool.m_sOper = "Select";
            MyTool.m_sDrawType = sDrawType;
            //m_mapToolbar.AddItem(MyTool, -1, -1, true, 0, esriCommandStyles.esriCommandStyleIconAndText);
            m_MapControl.CurrentTool = MyTool;
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            //打开坐标文件、构建需要的实体:
            sycCommonFuns CommonClass = new sycCommonLib.sycCommonFuns();
            IMap myMap = (IMap)m_MapControl.ActiveView.FocusMap;
            IActiveView act = m_MapControl.ActiveView.FocusMap as IActiveView;

            //[01]: Draw类型: point,line,polygon
            string sDrawType = "";
            if (this.rgJHLX.SelectedIndex == 0)
            {
                sDrawType = "Point";
            }
            else if (this.rgJHLX.SelectedIndex == 1)
            {
                sDrawType = "Line";
            }
            else if (this.rgJHLX.SelectedIndex == 2)
            {
                sDrawType = "Polygon";
            }      
            if (sDrawType.Length == 0)
            {
                MessageBox.Show("确定类型: 点、线、面 ? ");
                return;
            }

            //读文件、构造几何、放于m_SelGeo:
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            string strFilter = "坐标文件(*.txt)|*.txt";
            openFileDialog1.Filter = strFilter;
            openFileDialog1.InitialDirectory = Application.StartupPath;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string sCorFileName = openFileDialog1.FileName;
                MultipointClass needCors = new MultipointClass();
                System.IO.StreamReader sr = new System.IO.StreamReader(sCorFileName);
                string sXY = sr.ReadLine();
                while (sXY != null)
                {
                    int nGS = 0;
                    int[] POS = new int[100];
                    for (int i = 0; i < sXY.Length; i++)
                    {
                        if (sXY[i] == ',')
                        {
                            POS[nGS] = i;
                            nGS++;
                        }
                    }
                    if (nGS < 1)
                    {
                        MessageBox.Show("坐标行: '" + sXY + "' 错误!");
                        sr.Close();
                        return;
                    }
                    string sX = "", sY = "";
                    if (nGS == 1)
                    {
                        sX = sXY.Substring(0, POS[0]);
                        sY = sXY.Substring(POS[0] + 1);
                    }
                    else if (nGS >= 2)
                    {
                        sX = sXY.Substring(0, POS[0]);
                        sY = sXY.Substring(POS[0] + 1, POS[1] - POS[0] - 1);
                    }

                    double dX = 0.0, dY = 0.0;
                    try
                    {
                        dX = Convert.ToDouble(sX);
                        dY = Convert.ToDouble(sY);
                    }
                    catch (Exception E)
                    {
                        MessageBox.Show("处理坐标文件时、行 '" + sXY + "' 发生错误:" + E.Message + " !");
                        sr.Close();
                        return;
                    }
                    IPoint newP = new PointClass();
                    newP.PutCoords(dX, dY);
                    object oo = Type.Missing;
                    needCors.AddPoint(newP, ref oo, ref oo);

                    sXY = sr.ReadLine();
                }
                sr.Close();
                if (needCors.PointCount == 0)
                {
                    MessageBox.Show("坐标文件中没有任何坐标!");
                    return;
                }

                if (sDrawType.Equals("Point") == true)
                {
                    //Point
                    IPoint p1 = new PointClass();
                    IPoint p2 = needCors.get_Point(0);
                    p1.PutCoords(p2.X, p2.Y);
                    m_SelGeo = (IGeometry)p1;
                }
                else if (sDrawType.Equals("Line") == true)
                {
                    IPolyline newLine = new PolylineClass();
                    IPointCollection pCol = newLine as IPointCollection;
                    for (int i = 0; i < needCors.PointCount; i++)
                    {
                        IPoint p1 = needCors.get_Point(i);
                        IPoint p2 = new PointClass();
                        p2.PutCoords(p1.X, p1.Y);

                        object oo = Type.Missing;
                        pCol.AddPoint(p2, ref oo, ref oo);
                    }
                    m_SelGeo = (IGeometry)newLine;
                }
                else if (sDrawType.Equals("Polygon") == true)
                {
                    IPolygon newPolygon = new PolygonClass();
                    IPointCollection pCol = newPolygon as IPointCollection;
                    for (int i = 0; i < needCors.PointCount; i++)
                    {
                        IPoint p1 = needCors.get_Point(i);
                        IPoint p2 = new PointClass();
                        p2.PutCoords(p1.X, p1.Y);

                        object oo = Type.Missing;
                        pCol.AddPoint(p2, ref oo, ref oo);
                    }
                    m_SelGeo = (IGeometry)newPolygon;
                }

                //建立对应m_SelGeo的Ele:
                IElement createEle = null;
                if (true)
                {
                    IGeometry myGeo = m_SelGeo;

                    IGraphicsContainer mapCon = act.GraphicsContainer;
                    mapCon.DeleteAllElements();

                    ISymbol needSym = CommonClass.syc_CreateDefaultSymbol(myGeo.GeometryType);
                    if (myGeo.GeometryType == esriGeometryType.esriGeometryPoint)
                    {
                        IElement element = new MarkerElementClass();
                        element.Geometry = myGeo;
                        IMarkerElement markerElement = (IMarkerElement)element;
                        markerElement.Symbol = needSym as IMarkerSymbol;
                        mapCon.AddElement(element, 0);
                        createEle = element;
                    }
                    else if (myGeo.GeometryType == esriGeometryType.esriGeometryPolyline)
                    {
                        LineElementClass LineEle = new LineElementClass();
                        LineEle.Geometry = (IGeometry)myGeo;
                        LineEle.Symbol = needSym as ILineSymbol;
                        mapCon.AddElement(LineEle, 0);
                        createEle = LineEle;
                    }
                    else if (myGeo.GeometryType == esriGeometryType.esriGeometryPolygon)
                    {
                        PolygonElementClass polyEle = new PolygonElementClass();
                        polyEle.Geometry = myGeo;
                        polyEle.Symbol = needSym as IFillSymbol;
                        mapCon.AddElement(polyEle, 0);
                        createEle = polyEle;
                    }
                    act.PartialRefresh(esriViewDrawPhase.esriViewBackground, createEle, act.Extent);
                }
                m_CreateEle = createEle;
                //...
            }
            CommonClass.Dispose();
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            //建立并亮闪Buffer:

            IMap myMap = m_MapControl.ActiveView.FocusMap as IMap;
            IActiveView act = myMap as IActiveView;

            //[01] 检查RedLine类型等:
            if (m_SelGeo == null)
            {
                MessageBox.Show("先确定定位参数[RedLine?]  !");
                return;
            }
            double dBuffer = 0.0;
            try
            {
                dBuffer = Convert.ToDouble(this.txtBuffer.Text);
            }
            catch (Exception E)
            {
                MessageBox.Show("Buffer参数:" + E.Message + " !");
                return;
            }
            if (dBuffer < 0.001)
            {
                MessageBox.Show("Buffer距离为0, 不产生Buffer!");
                return;
            }
            bool bSingle = false;	//线Buffer时、前进方向左侧
            if (this.chkGoDirection.Checked == true)
                bSingle = true;

            //[02] 根据RedLine+BufferDist建立Buffer:
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            IPolygon bufferPolygon = null;
            if ((m_SelGeo.GeometryType == esriGeometryType.esriGeometryPolyline) &&
                (bSingle == true))
            {
                //线做单方向Buffer:
                sycCommonFuns CommonClass = new sycCommonLib.sycCommonFuns();
                IPolyline pLine = m_SelGeo as IPolyline;
                bool bRet = CommonClass.syc_CreateSingleSideBuffer(pLine, dBuffer, ref bufferPolygon);
                CommonClass.Dispose();
                if (bRet == false)
                {
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                    MessageBox.Show("根据实体建立单边Buffer错误[Polyline,Buffer=" + m_dBuffer.ToString("F1") + "] !");
                    return;
                }
            }
            else
            {
                //m_SelGeo可能是Point,Line,Area:
                sycCommonFuns CommonClass = new sycCommonLib.sycCommonFuns();
                bool bRet = CommonClass.syc_CreateBuffer(m_SelGeo, dBuffer, ref bufferPolygon);
                CommonClass.Dispose();
                if (bRet == false)
                {
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                    MessageBox.Show("根据实体建立Buffer错误[实体类型:" + m_SelGeo.GeometryType.ToString() + ",Buffer=" + m_dBuffer.ToString("F1") + "] !");
                    return;
                }
            }
            IGeometry newGeo = bufferPolygon as IGeometry;
            object oSymbol = Type.Missing;
            m_MapControl.FlashShape(newGeo, 5, 300, oSymbol);
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            //Flash选中的特性:
            if (m_CurLine != -1 && m_Result != null && m_Result.Count != 0 && m_CurLine < m_Result.Count)
            {
                IFeature curFeat = m_Result[m_CurLine] as IFeature;
                if (curFeat == null || curFeat is System.DBNull)
                    return;

                IGeometry myGeo = (IGeometry)curFeat.ShapeCopy;
                ISymbol mySymbol = m_CommonClass.syc_CreateDefaultSymbol(myGeo.GeometryType);
                m_MapControl.FlashShape(myGeo, 5, 300, mySymbol);
            }
        }

        private void simpleButton10_Click(object sender, EventArgs e)
        {
            //清空所有的选择:
            IMap myMap = m_MapControl.ActiveView.FocusMap;
            myMap.ClearSelection();
            IActiveView act = myMap as IActiveView;
            act.PartialRefresh(esriViewDrawPhase.esriViewGeography | esriViewDrawPhase.esriViewGeoSelection, null, act.Extent);
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {

            //把m_Result中的实体选中:
            if (m_Result == null || m_Result.Count == 0 || m_selFeatLyr == null)
                return;

            IMap myMap = m_MapControl.ActiveView.FocusMap;
            myMap.ClearSelection();
            for (int i = 0; i < m_Result.Count; i++)
            {
                IFeature feat = m_Result[i] as IFeature;
                myMap.SelectFeature((ILayer)m_selFeatLyr, feat);
            }
            IActiveView act = myMap as IActiveView;
            act.PartialRefresh(esriViewDrawPhase.esriViewGeography | esriViewDrawPhase.esriViewGeoSelection, null, act.Extent);
        }


    }
}
