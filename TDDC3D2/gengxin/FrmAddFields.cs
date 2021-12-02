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
    public partial class FrmAddFields : Form
    {
        public FrmAddFields()
        {
            InitializeComponent();
        }
        public IFeatureClass pFeaClass;
        public IWorkspace pCurrentWorkspace;
        private void FrmAddFields_Load(object sender, EventArgs e)
        {
            //IEnumDataset pEnumDataset = pCurrentWorkspace.get_Datasets(esriDatasetType.esriDTAny);
            //pEnumDataset.Reset();
            //IDataset pDataset = pEnumDataset.Next();
            //while (pDataset != null)
            //{
            //     if(pDataset.Type==esriDatasetType.esriDTFeatureDataset)
            //     {
            //         IFeatureClassContainer pFCC = pDataset as IFeatureClassContainer;
            //         IEnumFeatureClass pEnumFC = pFCC.Classes;
            //         pEnumFC.Reset();
            //         IFeatureClass pFeatureClass = pEnumFC.Next();
            //         while (pFeatureClass != null)
            //         {
            //             comLayer.Properties.Items.Add(pFeatureClass.AliasName + "|" + (pFeatureClass as IDataset).Name);
            //             //Layers.Add(pFeatureClass.AliasName,(pFeatureClass as IDataset).Name);
            //             pFeatureClass=pEnumFC.Next();
            //         }
            //     }
            //     else
            //     {
            //         //comLayer.Properties.Items.Add((pDataset as IFeatureClass).AliasName + "|" + pDataset.Name);
            //     }
            //     pDataset = pEnumDataset.Next();
            //}

            
            
        }

        
        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < chkListFields.Items.Count; i++)
            {
                chkListFields.Items[i].CheckState = chkAll.CheckState;
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("请稍等", "开始执行...");
            wait.Show();
            IFeatureClass pSourceClass = RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(txtSourceLayer.Text);
            for (int i = 0; i < chkListFields.CheckedItems.Count; i++)
            {
                //chkListFields.SelectedItems.Count
                wait.SetCaption("正在追加" + chkListFields.CheckedItems[i].ToString() + "");
                string fieldName = chkListFields.CheckedItems[i].ToString();
                fieldName = fieldName.Substring(fieldName.IndexOf("|")+1);
                //IField pField = new FieldsClass();
                IField pField = new FieldClass();
                pField=pSourceClass.Fields.get_Field(pSourceClass.Fields.FindField(fieldName));
                if (pField.IsNullable == false)
                {
                    IFieldEdit2 pFldEdit = pField as IFieldEdit2;
                    pFldEdit.IsNullable_2 = true;
                }
                IClass pClass = pFeaClass as IClass;
                pClass.AddField(pField);
                //pFeaClass.Fields.
                //IFieldEdit pFieldEdit = pFeaClass.Fields;
            }
            wait.Close();
            MessageBox.Show("追加完成" , "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void txtSourceLayer_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "shapefile(*.shp)|*.shp|All files(*.*)|*.*";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                txtSourceLayer.Text = openFile.FileName;

                chkListFields.Items.Clear();
                string name = txtSourceLayer.Text;
                IFeatureClass pSourceFeaClass = RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(name);
                for (int i = 0; i < pSourceFeaClass.Fields.FieldCount; i++)
                {
                    string fieldName = pSourceFeaClass.Fields.Field[i].Name;
                    if (pFeaClass.Fields.FindField(fieldName) == -1 && fieldName.ToUpper()!="FID"&&fieldName.ToUpper()!="OBJECTID")
                    {
                        chkListFields.Items.Add(pSourceFeaClass.Fields.Field[i].AliasName + "|" + pSourceFeaClass.Fields.Field[i].Name);
                    }
                }
            }
        }
    }
}
