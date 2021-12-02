using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.ConversionTools;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geoprocessor;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;
using RCIS.GISCommon;

namespace TDDC3D.raster
{
    public partial class Raster2PolygonFrm : Form
    {
        public Raster2PolygonFrm()
        {
            InitializeComponent();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void beInputRaster_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "栅格文件|*.img";
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            this.beInputRaster.Text = dlg.FileName;
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
        private void RunTool(Geoprocessor geoprocessor, IGPProcess process, ITrackCancel TC)  
         {  
         // Set the overwrite output option to true  
             geoprocessor.OverwriteOutput = true;  
   
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
             string ms = "";
             if (gp.MessageCount > 0)
             {
                 for (int Count = 0; Count <= gp.MessageCount - 1; Count++)
                 {
                     ms += gp.GetMessage(Count);
                 }
             }
             this.memoEdit1.Text += "\r\n" + ms;
         }  
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (this.beOutputShp.Text.Trim() == "") return;
            if (this.beInputRaster.Text.Trim() == "") return;
            string outShp=this.beOutputShp.Text.Trim().ToUpper();
            if (!outShp.EndsWith(".SHP"))
            {
                outShp += ".SHP";
            }
            //SIMPLIFY VALUE
            this.Cursor = Cursors.WaitCursor;

            GPMessageEventHandler gpEventHandler = new GPMessageEventHandler();

            //get an instance of the geoprocessor
            Geoprocessor GP = new Geoprocessor();
            //register the event helper in order to be able to listen to GP events
            GP.RegisterGeoProcessorEvents(gpEventHandler);

            //wire the GP events
            gpEventHandler.GPMessage += new MessageEventHandler(OnGPMessage);
            gpEventHandler.GPPreToolExecute += new PreToolExecuteEventHandler(OnGPPreToolExecute);
            gpEventHandler.GPPostToolExecute += new PostToolExecuteEventHandler(OnGPPostToolExecute);

            //instruct the geoprocessing engine to overwrite existing datasets
            GP.OverwriteOutput = true;


            //构造Geoprocessor  
            Geoprocessor gp = new Geoprocessor();
            //设置参数  
            ESRI.ArcGIS.ConversionTools.RasterToPolygon raster2polygon = new RasterToPolygon();
            raster2polygon.in_raster = this.beInputRaster.Text.Trim();
            raster2polygon.out_polygon_features = outShp;
            raster2polygon.raster_field = "VALUE";
            raster2polygon.simplify = "SIMPLIFY";
            //执行Intersect工具  
            RunTool(gp, raster2polygon, null);


            gpEventHandler.GPMessage -= new MessageEventHandler(OnGPMessage);
            gpEventHandler.GPPreToolExecute -= new PreToolExecuteEventHandler(OnGPPreToolExecute);
            gpEventHandler.GPPostToolExecute -= new PostToolExecuteEventHandler(OnGPPostToolExecute);

            //unregister the event helper
            GP.UnRegisterGeoProcessorEvents(gpEventHandler);

            this.Cursor = Cursors.Default;

        }

        void OnGPPostToolExecute(object sender, GPPostToolExecuteEventArgs e)
        {
            memoEdit1.Text += "\r\n" + e.Result.ToString();
            Application.DoEvents();
            //System.Diagnostics.Trace.WriteLine(e.Result.ToString());
        }

       

        void OnGPPreToolExecute(object sender, GPPreToolExecuteEventArgs e)
        {
            memoEdit1.Text += "\r\n" + e.Description;
            Application.DoEvents();
            //System.Diagnostics.Trace.WriteLine(e.Description);
        }

        void OnGPMessage(object sender, GPMessageEventArgs e)
        {
            memoEdit1.Text += "\r\n" + e.Message;
            //System.Diagnostics.Trace.WriteLine(e.Message);
        }

    }
}
