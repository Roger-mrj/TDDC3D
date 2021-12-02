using ESRI.ArcGIS.DataManagementTools;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geoprocessor;
using RCIS.GISCommon;
using RCIS.Utility;
using System;
using System.Windows.Forms;

namespace TDDC3D.datado
{
    public partial class DissovleForm : Form
    {
        public DissovleForm()
        {
            InitializeComponent();
        }
        
        private void btSounceFC_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "SHP文件|*.shp";
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            try
            {
                this.btSounceFC.Text = dlg.FileName;
                IWorkspace shpWs = WorkspaceHelper2.GetShapefileWorkspace(this.btSounceFC.Text);
                IFeatureClass srcClass = (shpWs as IFeatureWorkspace).OpenFeatureClass(System.IO.Path.GetFileNameWithoutExtension(this.btSounceFC.Text));
                
                //加载所有字段
                this.chklistFields.Items.Clear();
                for (int i = 0; i < srcClass.Fields.FieldCount; i++)
                {
                    string fldName = srcClass.Fields.get_Field(i).Name.ToUpper();
                    string fldAlias = srcClass.Fields.get_Field(i).AliasName;
                    int idx = this.chklistFields.Items.Add(fldName + "|" + fldAlias);
                    if (fldName.Contains("SHAPE") || fldName.Contains("SHP"))
                    {
                        this.chklistFields.SetItemChecked(idx, true);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

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

        private void beOutputShp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "SHP文件|*.shp";
            if (dlg.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            this.beOutputShp.Text = dlg.FileName;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (this.btSounceFC.Text.Trim() == "")
                return;
            if (this.beOutputShp.Text.Trim() == "") return;
            string outShp = this.beOutputShp.Text.Trim().ToUpper();
            if (!outShp.EndsWith(".SHP"))
            {
                outShp += ".SHP";
            }

            if (this.chklistFields.CheckedItemsCount == 0)
            {
                MessageBox.Show("请先选择字段！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            Geoprocessor GP = new Geoprocessor();
            Dissolve dissovle = new Dissolve();
            dissovle.in_features = this.btSounceFC.Text;

            string param = "";
            for (int i = 0; i < this.chklistFields.CheckedItemsCount; i++)
            {
                param += OtherHelper.GetLeftName(this.chklistFields.CheckedItems[i].ToString()) + ";";
            }
            if (param.Length > 1) param = param.Remove(param.Length - 1, 1);
            dissovle.dissolve_field = param;
            dissovle.multi_part = "SINGLE_PART";
            dissovle.out_feature_class = outShp;

            this.Enabled = false;
            this.Cursor = Cursors.WaitCursor;
            try
            {
                //执行
                
                
                RunTool(GP, dissovle, null);
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
        //D:\第三次土地调查\测试数据\510722三台县.mdb\DLTB C:\Users\Thinkpad\Desktop\aa.shp DLBM;DLMC # SINGLE_PART DISSOLVE_LINES


    }
}
