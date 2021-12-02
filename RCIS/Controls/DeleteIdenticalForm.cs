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
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.DataManagementTools;
using ESRI.ArcGIS.Geoprocessor;

using RCIS.GISCommon;
using RCIS.Utility;


namespace RCIS.Controls
{
    public partial class DeleteIdenticalForm : Form
    {
        public DeleteIdenticalForm()
        {
            InitializeComponent();
        }

        private IWorkspace OriginWS = null;
        private string OriginFCName = "";

        private void RunTool(Geoprocessor geoprocessor, IGPProcess process, ITrackCancel TC)
        {

            // Set the overwrite output option to true
            geoprocessor.OverwriteOutput = true;

            // Execute the tool            
            try
            {
                geoprocessor.Execute(process, null);
                ReturnMessages(geoprocessor);

            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                ReturnMessages(geoprocessor);
            }
        }

        // Function for returning the tool messages.
        private void ReturnMessages(Geoprocessor gp)
        {
            this.memoEdit1.Text = "";
            if (gp.MessageCount > 0)
            {
                for (int Count = 0; Count <= gp.MessageCount - 1; Count++)
                {
                    this.memoEdit1.Text += gp.GetMessage(Count) + "\r\n";
                    //Console.WriteLine(gp.GetMessage(Count));
                }
            }

        }

        private void btSounceFC_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            AddDataForm frm = new AddDataForm();
            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                return;
            ILayer currLyr = frm.resultLyr;
            IFeatureLayer currFeaLyr = currLyr as IFeatureLayer;
            if (currFeaLyr == null)
                return;
            IFeatureClass srcClass = currFeaLyr.FeatureClass;

            OriginWS = (srcClass as IDataset).Workspace;
            OriginFCName = LayerHelper.GetClassShortName(srcClass as IDataset);
            string srcFCTxt= OriginWS.PathName + "\\" + OriginFCName;
            
            if (currFeaLyr.DataSourceType.ToUpper()=="Shapefile Feature Class".ToUpper())
            {
                 srcFCTxt += ".SHP";
            }
            this.btSounceFC.Text = srcFCTxt;

            //加载所有字段
            this.chklistFields.Items.Clear();
            for (int i = 0; i < srcClass.Fields.FieldCount; i++)
            {
                string fldName = srcClass.Fields.get_Field(i).Name.ToUpper();
                string fldAlias = srcClass.Fields.get_Field(i).AliasName;
                int idx=this.chklistFields.Items.Add(fldName + "|" + fldAlias);
                if (fldName.Contains("SHAPE") || fldName.Contains("SHP"))
                {
                    this.chklistFields.SetItemChecked(idx, true);
                }
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (this.btSounceFC.Text.Trim() == "")
                return;
            if (this.chklistFields.CheckedItemsCount==0)
            {
                MessageBox.Show("请先选择字段！","提示",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                return;
            }
            this.Enabled = false;
            this.Cursor = Cursors.WaitCursor;
            try
            {
                //执行
                Geoprocessor GP = new Geoprocessor();
                DeleteIdentical delIdentical = new DeleteIdentical();
                delIdentical.in_dataset = this.btSounceFC.Text;

                string param = "";
                for (int i = 0; i < this.chklistFields.CheckedItemsCount; i++)
                {
                    param += OtherHelper.GetLeftName(this.chklistFields.CheckedItems[i].ToString()) + ";";
                }
                if (param.Length > 1) param = param.Remove(param.Length - 1, 1);
                delIdentical.fields = param;

                RunTool(GP, delIdentical, null);
                this.xtraTabControl1.SelectedTabPageIndex = 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Enabled = true;
                this.Cursor = Cursors.Default;
            }
            

        }

        private void DeleteIdenticalForm_Load(object sender, EventArgs e)
        {
            this.xtraTabControl1.SelectedTabPageIndex = 0;
        }
    }
}
