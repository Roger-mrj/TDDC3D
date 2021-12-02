using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Controls;
using System.Collections;


namespace RCIS.ElseTool
{
    public partial class lineToPolygonFrm : Form
    {
        public AxMapControl currMaptrl = null;


        public void CreatePolygonFeaturesFromCursors(IFeatureClass polygonFC,  IFeatureClass lineFC,IQueryFilter pQF)
        {  
            if (polygonFC.ShapeType != esriGeometryType.esriGeometryPolygon)  
            {    
                
                return ;  
            }  //Set IFeatureCursor object, which will be the line source to construct polygons. 
            IFeatureCursor lineFeatureCursor = lineFC.Search(pQF, false);  
            //Set the processing bounds to be the extent of the polygon feature class,  
            //which will be used to search for existing polygons in the target feature.  
            IGeoDataset geoDS = polygonFC as IGeoDataset;  
            IEnvelope processingBounds = geoDS.Extent; 
            //Define an IInValidArea object.  
            IInvalidArea invalidArea = new InvalidAreaClass(); 
            //Define a construct feature object.  
            IFeatureConstruction featureConstruction = new FeatureConstructionClass();  
            //Start an edit session. 
            IDataset dataset = polygonFC as IDataset;  
            IWorkspace workspace = dataset.Workspace;  
            IWorkspaceEdit workspaceEdit = workspace as IWorkspaceEdit;  
           
            workspaceEdit.StartEditOperation(); 

            try  
            {    //**********Construct polygons using the line feature cursor.************//    
                featureConstruction.ConstructPolygonsFromFeaturesFromCursor(null, polygonFC,      
                    processingBounds, true, false, lineFeatureCursor, invalidArea,  - 1, null)      ;  
                //**********AutoComplete the polygons.*************//   
                //IWorkspace selWorkspace = polygonFC.FeatureDataset.Workspace;    
                //ISelectionSet selectionSet;   
                //featureConstruction.AutoCompleteFromFeaturesFromCursor(polygonFC, processingBounds, lineFeatureCursor,   
                // invalidArea, -1, selWorkspace, out selectionSet);   
                //**********Split the polygons.***************//    
                //featureConstruction.SplitPolygonsWithLinesFromCursor(null, polygonFC, processingBounds,   
                // lineFeatureCursor, invalidArea, -1);   
                //Complete the edit operation and stop the edit session.  
                workspaceEdit.StopEditOperation();   
                
            }  catch (Exception e)  {   
                //Abort the edit operation if errors are detected.   
                System.Console.WriteLine("Construct polygons failed. " + e.Message);  
                workspaceEdit.AbortEditOperation();
            }
        }

        public lineToPolygonFrm()
        {
            InitializeComponent();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       
        private void lineToPolygonFrm_Load(object sender, EventArgs e)
        {
            if (this.currMaptrl == null)
                return;
            cmbSrcLine.Items.Clear();
            cmbDestPolygon.Items.Clear();

            for (int i = 0; i < currMaptrl.LayerCount; i++)
            {
                ILayer currLyr = currMaptrl.get_Layer(i);
                if (currLyr is IGroupLayer)
                {
                    ICompositeLayer pComLyr = currLyr as ICompositeLayer;
                    for (int kk = 0; kk < pComLyr.Count; kk++)
                    {
                        ILayer childLyr = pComLyr.get_Layer(kk);
                        if (childLyr is IFeatureLayer)
                        {
                            IFeatureLayer currFeatLyr = childLyr as IFeatureLayer;
                            if (currFeatLyr.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolyline)
                            {
                                cmbSrcLine.Items.Add(i + "|" + currFeatLyr.Name);
                            }
                            else if (currFeatLyr.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                            {
                                cmbDestPolygon.Items.Add(i + "|" + currFeatLyr.Name);
                            }
                        }
                    }

                }
                else if (currLyr is IFeatureLayer)
                {
                   
                    IFeatureLayer currFeatLyr = currLyr as IFeatureLayer;
                    if (currFeatLyr.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolyline)
                    {
                        cmbSrcLine.Items.Add(i + "|" + currFeatLyr.Name);
                    }
                    else if (currFeatLyr.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                    {
                        cmbDestPolygon.Items.Add(i + "|" + currFeatLyr.Name);
                    }
                }

                

            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if ((cmbSrcLine.Text.Trim() == "") || (cmbDestPolygon.Text.Trim() == ""))
                return;
            string srcLine = cmbSrcLine.Text.Trim();
            string destPolygon = cmbDestPolygon.Text.Trim();
            int srcIdx=Convert.ToInt32( srcLine.Substring(0,srcLine.IndexOf("|")));
            int destIdx=Convert.ToInt32(destPolygon.Substring(0,destPolygon.IndexOf("|")));
            
            IFeatureClass srcClass=(this.currMaptrl.get_Layer(srcIdx) as IFeatureLayer).FeatureClass;
            IFeatureClass destClass=(this.currMaptrl.get_Layer(destIdx) as IFeatureLayer).FeatureClass;


           

            this.Cursor=Cursors.WaitCursor;
            try
            {
                if (this.rbCurrExtent.Checked)
                {
                    ISpatialFilter pSF = new SpatialFilterClass();
                    pSF.Geometry = this.currMaptrl.ActiveView.Extent as IGeometry;
                    pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;

                    CreatePolygonFeaturesFromCursors(destClass, srcClass, pSF as IQueryFilter);
                }
                else if (this.rbAllFeatures.Checked)
                {
                    CreatePolygonFeaturesFromCursors(destClass, srcClass,null);
                }
                this.currMaptrl.ActiveView.Refresh();
                MessageBox.Show("×ª»»³É¹¦£¡");
            }
            catch(Exception ex) {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor=Cursors.Default;
            }
        }
    }
}