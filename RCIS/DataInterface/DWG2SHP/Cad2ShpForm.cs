using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geoprocessor;
using ESRI.ArcGIS.Geoprocessing;
using ESRI.ArcGIS.ConversionTools;


namespace RCIS.DataExchange.DWG2SHP
{
    public partial class Cad2ShpForm : Form
    {
        public Cad2ShpForm()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void beDWG_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "DWGÎÄ¼þ|*.dwg";

            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            string fileName = dlg.FileName;
            beDWG.Text = fileName;

            lstSrcClasses.Items.Clear();
            lstSrcClasses.Items.Add(fileName + @"\Annotation");
            lstSrcClasses.Items.Add(fileName + @"\Point");
            lstSrcClasses.Items.Add(fileName + @"\Polyline");
            lstSrcClasses.Items.Add(fileName + @"\Polygon");

        }

        private void beDestFolder_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            beDestFolder.Text = dlg.SelectedPath;

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (beDWG.Text.Trim().Equals(""))
                return;
            if (beDestFolder.Text.Trim().Equals(""))
                return;
            this.Cursor = Cursors.WaitCursor;
            this.tabMain.SelectedTabPageIndex = 1;
            try
            {

                Geoprocessor gp = new Geoprocessor();
                FeatureClassToShapefile convert = new FeatureClassToShapefile();
                if (lstSrcClasses.Items.Count>0)
                {
                    convert.Input_Features=lstSrcClasses.Items[0].ToString();
                }
                for (int i = 1; i < lstSrcClasses.Items.Count; i++)
                {
                    convert.Input_Features += ";" + lstSrcClasses.Items[i].ToString();
                }
                convert.Output_Folder = beDestFolder.Text.Trim();

                IGeoProcessorResult results = (IGeoProcessorResult)gp.Execute(convert, null);

                if (gp.MessageCount > 0)
                {
                    for (int Count = 0; Count <= gp.MessageCount - 1; Count++)
                    {
                        this.txtLog.Text+=  gp.GetMessage(Count) + "\r\n";
                    }
                }
                
            }
            catch(Exception ex)
            {
                txtLog.Text += ex.ToString() + "\r\n";
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            
        }
    }
}