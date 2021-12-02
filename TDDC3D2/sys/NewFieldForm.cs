using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace TDDC3D.sys
{
    public partial class NewFieldForm : Form
    {
        public NewFieldForm()
        {
            InitializeComponent();
        }

        public IFeatureClass sendClass = null;
        private void comboFldType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string txt = this.comboFldType.Text.Trim();
            if (txt == "字符型")
            {
                spinEditLength.Text = "250";
                this.spinScale.Enabled = false;
            }
            else
            {
                spinEditLength.Text = "13";
                this.spinScale.Enabled = true;
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (sendClass == null)
                return;
            if ((this.txtFieldName.Text.Trim()=="") || (this.comboFldType.Text.Trim()==""))
            {
                return;
            }
            string fldName = this.txtFieldName.Text.Trim().ToUpper();
            if (sendClass.FindField(fldName) > -1)
            {
                MessageBox.Show("该字段已存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                string aliasName = this.txtFldAlias.Text.Trim();
                if (aliasName == "")
                {
                    aliasName = fldName;
                }
                esriFieldType fldType = esriFieldType.esriFieldTypeString;
                switch (this.comboFldType.Text.Trim())
                {
                    case "日期型":
                        fldType = esriFieldType.esriFieldTypeDate;
                        break;
                    case "整型":
                        fldType = esriFieldType.esriFieldTypeInteger;
                        break;
                    case "字符型":
                        fldType = esriFieldType.esriFieldTypeString;

                        break;
                    case "浮点型":
                        fldType = esriFieldType.esriFieldTypeDouble;
                        break;

                }
                int iLen = (int)this.spinEditLength.Value;
                IField pField = new FieldClass();
                IFieldEdit2 pFldEdit = pField as IFieldEdit2;
                pFldEdit.Name_2 = fldName;
                pFldEdit.AliasName_2 = aliasName;
                pFldEdit.Type_2 = fldType;
                if (fldType == esriFieldType.esriFieldTypeString)
                {
                    pFldEdit.Length_2 = iLen;
                }
                else if (fldType == esriFieldType.esriFieldTypeDouble)
                {
                    int iScale = 2;
                    int.TryParse(this.spinScale.Text, out iScale);
                    pFldEdit.Precision_2 = iLen;
                    pFldEdit.Scale_2 = iScale;
                }
                IClass pClass = sendClass as IClass;
                pClass.AddField(pField);

                MessageBox.Show("添加成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            

        }

        private void NewFieldForm_Load(object sender, EventArgs e)
        {

        }
    }
}
