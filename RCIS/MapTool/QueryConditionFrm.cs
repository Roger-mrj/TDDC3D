using DevExpress.XtraEditors;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RCIS.MapTool
{
    public partial class QueryConditionFrm : Form
    {
        public IFeatureLayer currLayer = null;   //当前图层
        private Dictionary<string, esriFieldType> fileTypeDic;
        private DataTable valueTable = null;
        public IMap pMap = null;


        public QueryConditionFrm()
        {
            InitializeComponent();

        }


        private void QueryConditionFrm_Load(object sender, EventArgs e)
        {
            try
            {
                if (currLayer != null)
                {
                    //加载字段信息
                    LsbcFields.Items.Clear();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("name", typeof(string));
                    dt.Columns.Add("value", typeof(string));
                    fileTypeDic = new Dictionary<string, esriFieldType>();
                    IFields pFields = currLayer.FeatureClass.Fields;
                    for (int i = 0; i < pFields.FieldCount; i++)
                    {
                        IField pField = pFields.get_Field(i);
                        string sName = pField.Name.Trim().ToUpper();
                        if ((sName.Equals("SHAPE") == true) ||
                            (sName.Equals("SHAPE_LENGTH") == true) ||
                            (sName.Equals("SHAPE_AREA") == true) ||
                            (sName.Equals("SHAPE.LEN") == true) ||
                            (sName.Equals("SHAPE.AREA") == true))
                            continue;

                        fileTypeDic.Add(sName, pField.Type);
                        dt.Rows.Add("\"" + sName + "\"", sName);
                    }
                    LsbcFields.DataSource = dt;
                    LsbcFields.DisplayMember = "name";
                    LsbcFields.ValueMember = "value";

                    //加载查询条件
                    IFeatureLayerDefinition pFeatLyrDef = currLayer as IFeatureLayerDefinition;
                    txtCondition.Text = pFeatLyrDef.DefinitionExpression;

                    //设置查询前段部分
                    lblSelection.Text = "SELECT * FROM "+currLayer.FeatureClass.AliasName+" WHERE:";
                }

            }
            catch (Exception)
            {
                MessageBox.Show("加载字段出错", "查询构建器", MessageBoxButtons.OK);
            }



        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtCondition.Clear();
        }


        private void btn_Click_AppendOperator(object sender, EventArgs e)
        {
            SimpleButton btn = (SimpleButton)sender;
            TxtConditionAppendText(btn.Text.ToUpper());
        }


        /// <summary>
        /// 添加表达式
        /// </summary>
        /// <param name="appendText"></param>
        private void TxtConditionAppendText(string appendText)
        {

            txtCondition.Focus();  //获取鼠标焦点

            if (string.IsNullOrWhiteSpace(txtCondition.Text))
            {
                txtCondition.Text = appendText;
                this.txtCondition.SelectionStart = appendText.Length;
            }
            else
            {
                //根据鼠标位置将字段截取为2段
                string endStr = txtCondition.Text.Substring(txtCondition.SelectionStart).Trim();
                string startStr = txtCondition.Text.Substring(0, txtCondition.SelectionStart).Trim();
                if (startStr.Length == 0)  //鼠标位置在开始位置
                {
                    txtCondition.Text = appendText + " " + txtCondition.Text;
                    this.txtCondition.SelectionStart = appendText.Length;
                }
                else if (endStr.Length == 0)   //鼠标位置末尾
                {
                    txtCondition.AppendText(" " + appendText);
                    this.txtCondition.SelectionStart = txtCondition.Text.Length;
                }
                else   //鼠标位置在中间
                {
                    txtCondition.Text = startStr + " " + appendText + " " + endStr;
                    this.txtCondition.SelectionStart = startStr.Length + appendText.Length + 1;
                }

            }

        }

        /// <summary>
        /// 当前字段选择改变时，注销值和搜索框
        /// </summary>
        private void LsbcFields_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.btn_GetValue.Enabled = true;


            this.lsbcValue.Enabled = false;
            valueTable = null;
            this.lsbcValue.DataSource = valueTable;

            this.txtSearch.Text = "";
            this.txtSearch.Enabled = false;

        }

        /// <summary>
        /// 根据字段查询
        /// </summary>
        private void btn_GetValue_Click(object sender, EventArgs e)
        {
            try
            {
                btn_GetValue.Enabled = false;
                lsbcValue.Enabled = true;
                txtSearch.Enabled = true;


                if (LsbcFields.SelectedItem == null || string.IsNullOrEmpty(LsbcFields.SelectedItem.ToString()) || currLayer == null)
                {
                    return;
                }
                string fielName = ((DataRowView)LsbcFields.SelectedItem)["value"].ToString();
                if (!string.IsNullOrEmpty(fielName))
                {
                    List<string> vList = GetUniqueValue(currLayer.FeatureClass, fielName);
                    lsbcValue.Items.Clear();
                    valueTable = new DataTable();
                    valueTable.Columns.Add("name", typeof(string));
                    valueTable.Columns.Add("value", typeof(string));
                    //if (vList.Count > 0) //添加默认为空的选项
                    //{
                    //    if (fileTypeDic[fielName] == esriFieldType.esriFieldTypeString)
                    //    {
                    //        valueTable.Rows.Add("''", "''");
                    //    }
                    //    if (fileTypeDic[fielName] == esriFieldType.esriFieldTypeInteger)
                    //    {
                    //        valueTable.Rows.Add("0", "0");
                    //    }

                    //}
                    foreach (string value in vList)
                    {
                        if (fileTypeDic[fielName] == esriFieldType.esriFieldTypeString)
                        {
                            valueTable.Rows.Add("'" + value + "'", value);
                        }
                        if (fileTypeDic[fielName] == esriFieldType.esriFieldTypeInteger|| fileTypeDic[fielName] == esriFieldType.esriFieldTypeDouble|| fileTypeDic[fielName] == esriFieldType.esriFieldTypeSmallInteger || fileTypeDic[fielName] == esriFieldType.esriFieldTypeOID)
                        {
                            valueTable.Rows.Add(value, value);
                        }

                    }
                    lsbcValue.DataSource = valueTable;
                    lsbcValue.DisplayMember = "name";
                    lsbcValue.ValueMember = "value";
                }
            }
            catch
            {
                MessageBox.Show("获取唯一值失败!", "查询构建器", MessageBoxButtons.OK);
            }
        }


        /// <summary>
        /// 根据字段获取唯一值
        /// </summary>
        /// <param name="pFeatureClass">表</param>
        /// <param name="fieldName">字段名称</param>
        /// <returns></returns>
        private List<string> GetUniqueValue(IFeatureClass pFeatureClass, string fieldName)
        {
            HashSet<string> hashset = new HashSet<string>();
            int fieldIndex = pFeatureClass.Fields.FindField(fieldName);

            // 属性过滤器 
            IQueryFilter pQueryFilter = new QueryFilter();
            pQueryFilter.AddField(fieldName);

            // 要素游标
            IFeatureCursor pFeatureCursor = pFeatureClass.Search(pQueryFilter, true);
            IFeature pFeature = pFeatureCursor.NextFeature();
            while (pFeature != null)
            {
                string fieldValue = pFeature.get_Value(fieldIndex).ToString();
                if (!hashset.Contains(fieldValue))
                {
                    hashset.Add(fieldValue);
                }
                pFeature = pFeatureCursor.NextFeature();
            }

            // 释放游标
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
            return hashset.ToList();
        }

        private void lsbcValue_DoubleClick(object sender, EventArgs e)
        {
            if (this.lsbcValue.SelectedItem != null)
            {
                string appendText = ((DataRowView)lsbcValue.SelectedItem)["name"].ToString();
                TxtConditionAppendText(appendText);

            }
        }

        private void LsbcFields_DoubleClick(object sender, EventArgs e)
        {
            if (this.LsbcFields.SelectedItem != null)
            {
                string appendText = ((DataRowView)LsbcFields.SelectedItem)["name"].ToString();
                TxtConditionAppendText(appendText);

            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string searchText = txtSearch.Text;
                if (string.IsNullOrWhiteSpace(searchText))
                {
                    lsbcValue.DataSource = valueTable;
                    lsbcValue.DisplayMember = "name";
                    lsbcValue.ValueMember = "value";

                }
                else
                {

                    if (valueTable != null && valueTable.Rows.Count > 0)
                    {
                        DataRow[] drs = valueTable.Select("value like '%"+searchText+"%'");
                        DataTable dt = new DataTable();
                        dt.Columns.Add("name", typeof(string));
                        dt.Columns.Add("value", typeof(string));
                        for (int i = 0; i < drs.Length; i++)
                        {
                            dt.Rows.Add(drs[i]["name"], drs[i]["value"]);
                        }

                        lsbcValue.DataSource = dt;
                        lsbcValue.DisplayMember = "name";
                        lsbcValue.ValueMember = "value";
                    }
                }
            }
            catch
            {
                MessageBox.Show("查询数据失败！", "查询构建器", MessageBoxButtons.OK);
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            //验证查询字段是否合理
            try
            {
                IQueryFilter pQf = new QueryFilterClass();
                pQf.WhereClause = txtCondition.Text.Trim();
                if (currLayer != null)
                {
                    IFeatureCursor pCursor = currLayer.FeatureClass.Search(pQf, false);
                    IFeature aXzqFea = null;
                    if ((aXzqFea = pCursor.NextFeature()) == null)
                    {
                        MessageBox.Show("已成功验证表达式/n未返回任何记录", "查询构建器", MessageBoxButtons.OK);
                    }
                    else
                    {
                        MessageBox.Show("已成功验证表达式", "查询构建器", MessageBoxButtons.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "查询构建器", MessageBoxButtons.OK);
            }

        }

        private void txtCondition_TextChanged(object sender, EventArgs e)
        {
            string currText = txtCondition.Text;
            if (currText.Length == 0)
            {
                btnTest.Enabled = false;
                btnClear.Enabled = false;
            }
            if (currText.Trim().Length == 0)
            {
                btnTest.Enabled = false;
            }
            if (currText.Length > 0)
            {
                btnClear.Enabled = true;
            }
            if (currText.Trim().Length > 0)
            {
                btnTest.Enabled = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                IQueryFilter pQf = new QueryFilterClass();
                pQf.WhereClause = txtCondition.Text.Trim();
                if (currLayer != null)
                {
                    IFeatureCursor pCursor = currLayer.FeatureClass.Search(pQf, false);
                    IFeatureLayerDefinition pFeatLyrDef = currLayer as IFeatureLayerDefinition;
                    pFeatLyrDef.DefinitionExpression = txtCondition.Text.Trim();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "查询构建器", MessageBoxButtons.OK);
            }

        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                IQueryFilter pQf = new QueryFilterClass();
                pQf.WhereClause = txtCondition.Text.Trim();
                if (currLayer != null)
                {
                    IFeatureCursor pCursor = currLayer.FeatureClass.Search(pQf, false);
                    IFeature pFeature;
                    while((pFeature=pCursor.NextFeature())!=null)
                    {
                        pMap.SelectFeature(currLayer, pFeature);
                    }
                    (pMap as IActiveView).PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, currLayer, null);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "查询构建器", MessageBoxButtons.OK);
            }
        }
    }
}
