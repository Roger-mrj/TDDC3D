using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;

using RCIS.GISCommon;
using RCIS.Utility;

namespace TDDC3D.datado
{
    public partial class SetValByWhereForm : Form
    {
        public SetValByWhereForm()
        {
            InitializeComponent();
        }

        public IMap currMap = null;
        
        private void SetValByWhereForm_Load(object sender, EventArgs e)
        {
            if (currMap != null)
            {
                LayerHelper.LoadLayer2Combox(this.cmbWhereTable1, currMap);

            }

            if (this.cmbWhereTable1.Properties.Items.Count > 0)
            {
                this.cmbWhereTable1.SelectedIndex = 0;
            }
               
        }
        private DataTable WhereClauseTable = null;  //设置条件表达式  的数据表

        private DataTable buildWhereClauseTab()
        {
            DataTable dt = new DataTable();
            DataColumn dc = new DataColumn("ZUHE", typeof(string));
            dt.Columns.Add(dc);
            dc = new DataColumn("WHERE", typeof(string));
            dt.Columns.Add(dc);
            

            return dt;
        }

        private string getWhereClause()
        {
            //形成条件语句
            string where = "";
            if (this.cmbWhereTable1.Text.Trim() == "")
                return where;
            if (this.cmbWhereFld1.SelectedItem == null)
                return where;
            if (this.cmbOperate.Text.Trim() == "")
                return where;

            
            fieldInfo curFld1 = this.cmbWhereFld1.SelectedItem as fieldInfo;           
            where = curFld1.fieldName ;
            //值
            where += (this.cmbOperate.SelectedItem as OpCodeItem).OpCode ;
            if (curFld1.fieldType2.ToUpper() == "INT" || curFld1.fieldType2.ToUpper() == "FLOAT")
            {
                double dValue = 0;
                double.TryParse(this.cmbValues.Text.Trim(), out dValue);
                where += dValue;
            }
            else
            {
                where += " '" + this.cmbValues.Text + "' ";
            }
            return where;

        }


        private void cmbWhereTable1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbWhereTable1.Text.Trim() == "")
                return;
            this.cmbWhereFld1.Properties.Items.Clear();
            this.cmbFld2.Properties.Items.Clear();

            string className=OtherHelper.GetLeftName(this.cmbWhereTable1.Text);
            IGeoFeatureLayer pLyr= LayerHelper.QueryLayerByModelName(currMap, className);
            IFeatureClass pFC=pLyr.FeatureClass;
            List<IField> lstFld = FeatureHelper.getAllFld(pFC);
            foreach (IField aFld in lstFld)
            {
                fieldInfo aFldInfo = new fieldInfo();
                aFldInfo.fieldName = aFld.Name;
                aFldInfo.fieldComment = aFld.AliasName;
                if (aFld.Type == esriFieldType.esriFieldTypeInteger || aFld.Type == esriFieldType.esriFieldTypeSmallInteger)
                {
                    aFldInfo.fieldType2 = "INT";
                }
                else if (aFld.Type == esriFieldType.esriFieldTypeSingle || aFld.Type == esriFieldType.esriFieldTypeDouble)
                {
                    aFldInfo.fieldType2 = "FLOAT";
                }
                else if (aFld.Type == esriFieldType.esriFieldTypeString)
                {
                    aFldInfo.fieldType2 = "CHAR";
                }
                else if (aFld.Type == esriFieldType.esriFieldTypeDate)
                {
                    aFldInfo.fieldType2 = "DATE";
                }

                this.cmbWhereFld1.Properties.Items.Add(aFldInfo);
                
                this.cmbFld2.Properties.Items.Add(aFldInfo);
            }


        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //添加一个条件
            if (this.gridControlwhereClause.DataSource == null)
            {
                WhereClauseTable = buildWhereClauseTab();
                this.gridControlwhereClause.DataSource = WhereClauseTable;
            }
            string where = getWhereClause();
            int icount = this.gridViewWhere.RowCount;
            //WhereClauseTable = (DataTable)(this.gridControlwhereClause.DataSource);
            DataRow aNewR = WhereClauseTable.NewRow();
            aNewR["WHERE"] = where;
            if (icount > 0)
            {
                string zuhe = " and ";
                if (this.radioGroup2.SelectedIndex == 1)
                {
                    zuhe = " or ";
                }

                aNewR["ZUHE"] = zuhe;                
            }
            WhereClauseTable.Rows.Add(aNewR);

        }

        public class fieldInfo
        {
            public string fieldName = "";
            
            /// <summary>
            /// INT NUMBER,NVARCHAR,DATE 等等
            /// </summary>
            public string fieldType2 = "";
            public string fieldComment = "";
            public string tableName = "";
            public override string ToString()
            {
                return fieldComment;

            }
            public fieldInfo()
            {

            }

            public fieldInfo(string _name, string _type , string _alias, string _tabName)
            {
                this.fieldName = _name;                
                this.fieldComment = _alias;
                this.tableName = _tabName;
                this.fieldType2 = _type;
            }
        }

        class OpCodeItem
        {
            public string OpName = "";
            public string OpCode = "";
            public OpCodeItem(string name, string code)
            {
                this.OpName = name;
                this.OpCode = code;
            }
            public override string ToString()
            {
                return this.OpName;
            }
            public static OpCodeItem OpCodeEquals = new OpCodeItem("等于", " = ");
            public static OpCodeItem OpCodeContains = new OpCodeItem("包含", " like ");
            public static OpCodeItem OpCodeGreat = new OpCodeItem("大于", ">");
            public static OpCodeItem OpCodeGreatEqual = new OpCodeItem("大于等于", " >= ");
            public static OpCodeItem OpCodeLess = new OpCodeItem("小于", "<");
            public static OpCodeItem OpCodeLessEqual = new OpCodeItem("小于等于", " <= ");
            public static OpCodeItem OpCodeNotEqual = new OpCodeItem("不等于", " <> ");
            public static OpCodeItem OpCodeNull = new OpCodeItem("为空", " is null ");
            public static OpCodeItem OpCodeNotNull = new OpCodeItem("不为空", " is not null ");
        }

        private void cmbWhereFld1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cmbOperate.Properties.Items.Clear();

            fieldInfo curFld = this.cmbWhereFld1.SelectedItem as fieldInfo;
            if (curFld != null)
            {
                switch (curFld.fieldType2)
                {
                    case "CHAR":
                    case "DATE":
                        {
                            this.cmbOperate.Properties.Items.Add(OpCodeItem.OpCodeEquals);
                            this.cmbOperate.Properties.Items.Add(OpCodeItem.OpCodeNotEqual);
                            this.cmbOperate.Properties.Items.Add(OpCodeItem.OpCodeContains);
                            this.cmbOperate.Properties.Items.Add(OpCodeItem.OpCodeNotNull);
                            this.cmbOperate.Properties.Items.Add(OpCodeItem.OpCodeNull);
                            break;
                        }
                    default:
                        {
                            this.cmbOperate.Properties.Items.Add(OpCodeItem.OpCodeEquals);
                            this.cmbOperate.Properties.Items.Add(OpCodeItem.OpCodeNotEqual);
                            this.cmbOperate.Properties.Items.Add(OpCodeItem.OpCodeLessEqual);
                            this.cmbOperate.Properties.Items.Add(OpCodeItem.OpCodeLess);
                            this.cmbOperate.Properties.Items.Add(OpCodeItem.OpCodeGreat);
                            this.cmbOperate.Properties.Items.Add(OpCodeItem.OpCodeGreatEqual);                            
                            break;
                        }
                }
                this.cmbOperate.SelectedItem = this.cmbOperate.Properties.Items[0];
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (this.gridViewWhere.SelectedRowsCount == 0)
                return;
            this.gridViewWhere.DeleteSelectedRows();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {

            if (WhereClauseTable != null)
            {
                WhereClauseTable.Rows.Clear();
                this.gridControlwhereClause.RefreshDataSource();
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (this.cmbWhereTable1.Text.Trim() == "")
                return;

            string tabName=OtherHelper.GetLeftName(this.cmbWhereTable1.Text);

            fieldInfo setValFld = this.cmbFld2.SelectedItem as fieldInfo;
            if (setValFld == null || WhereClauseTable == null)
            {
                MessageBox.Show("条件输入不全！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string value = this.cmbSetValue.Text.Trim();
            if (value == "")
            {
                if (MessageBox.Show("确实要赋值为空值么？","询问",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.No)
                    return;
            }
            
            
            //添加where条件
            StringBuilder sb = new System.Text.StringBuilder();
            if (WhereClauseTable.Rows.Count > 0)
            {
                sb.Append(WhereClauseTable.Rows[0]["WHERE"].ToString());
                for (int i = 1; i < WhereClauseTable.Rows.Count; i++)
                {
                    DataRow aRow = WhereClauseTable.Rows[i];
                    sb.Append(aRow["ZUHE"] + "  " + aRow["WHERE"]);
                }
            }
            string where = sb.ToString();
            if (where.Trim() == "")
            {
                MessageBox.Show("条件输入不全！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            this.Cursor = Cursors.WaitCursor;

            IWorkspaceEdit pWSEdit =RCIS.Global.GlobalEditObject.GlobalWorkspace as IWorkspaceEdit;
            pWSEdit.StartEditing(false);
            pWSEdit.StartEditOperation();

            string className = OtherHelper.GetLeftName(this.cmbWhereTable1.Text);
            IGeoFeatureLayer pLyr = LayerHelper.QueryLayerByModelName(currMap, className);
            IFeatureClass pFC = pLyr.FeatureClass;
            IQueryFilter pQf = new QueryFilter();
            pQf.WhereClause = where;
            int icount = pFC.FeatureCount(pQf);
            IFeatureCursor updateCursor = pFC.Update(pQf, false);
            IFeature aFea = null;
            try
            {
                int idx = pFC.FindField(setValFld.fieldName);
                if (idx > 0)
                {
                        
                    while ((aFea = updateCursor.NextFeature()) != null)
                    {
                        string sValue = this.cmbSetValue.Text.Trim();
                        if (setValFld.fieldType2 == "INT")
                        {
                            if (string.IsNullOrWhiteSpace(sValue))
                            {

                                aFea.set_Value(idx, DBNull.Value);
                            }
                            else
                            {
                                int iVal = 0;
                                int.TryParse(sValue, out iVal);
                                aFea.set_Value(idx, iVal);
                            }
                        }
                        else if (setValFld.fieldType2 == "FLOAT")
                        {
                            if (string.IsNullOrWhiteSpace(sValue))
                            {
                                aFea.set_Value(idx, DBNull.Value);
                            }
                            else
                            {
                                int dVal = 0;
                                int.TryParse(sValue, out dVal);
                                aFea.set_Value(idx, dVal);
                            }
                        }
                        else
                        {
                            aFea.set_Value(idx, sValue);
                        }
                        updateCursor.UpdateFeature(aFea);
                    }
                    
                }
                pWSEdit.StopEditOperation();
                pWSEdit.StopEditing(true);
                this.Cursor = Cursors.Default;
                MessageBox.Show("赋值完毕,共更新" + icount + "条记录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                   
            }
            catch (Exception ex)
            {
                pWSEdit.AbortEditOperation();
                pWSEdit.StopEditing(false);
                this.Cursor = Cursors.Default;
                MessageBox.Show("数据更新失败！"+ex.Message);
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(updateCursor);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pQf);
            }
            

        }
    }
}
