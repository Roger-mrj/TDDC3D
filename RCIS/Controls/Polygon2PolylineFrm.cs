using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geoprocessor;
using ESRI.ArcGIS.Geoprocessing;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.DataManagementTools;
using RCIS.Utility;
using RCIS.GISCommon;

namespace RCIS.MapTool
{
    public partial class Polygon2PolylineFrm : Form
    {
        public Polygon2PolylineFrm()
        {
            InitializeComponent();
        }

        public IWorkspace CurrWorkspace = null;
        private IFeatureDataset pFeatureDataset = null;

        private void Polygon2PolylineFrm_Load(object sender, EventArgs e)
        {
            pFeatureDataset = (CurrWorkspace as IFeatureWorkspace).OpenFeatureDataset(RCIS.Global.AppParameters.DATASET_DEFAULT_NAME);
            IFeatureClassContainer classContainer=pFeatureDataset as IFeatureClassContainer;
            for (int i = 0; i < classContainer.ClassCount; i++)
            {
                IFeatureClass aClass= classContainer.get_Class(i);
                if (aClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                {
                    string aName = LayerHelper.GetClassShortName(aClass as IDataset);
                    cmbPolygonLayer.Properties.Items.Add(aName+"|"+aClass.AliasName);
                }
                else if (aClass.ShapeType == esriGeometryType.esriGeometryPolyline)
                {
                    string aName = LayerHelper.GetClassShortName(aClass as IDataset);
                    cmbPolylineLayer.Properties.Items.Add(aName + "|" + aClass.AliasName);
                }
            }

        }

        private bool gpExcute(string inFeature,string outFeature,ref string err)
        {
            try
            {
                Geoprocessor gp = new Geoprocessor();
                ESRI.ArcGIS.DataManagementTools.PolygonToLine toLine = new PolygonToLine();
                toLine.in_features = CurrWorkspace.PathName + "\\"+RCIS.Global.AppParameters.DATASET_DEFAULT_NAME+"\\" + inFeature;
                toLine.out_feature_class = CurrWorkspace.PathName + "\\"+RCIS.Global.AppParameters.DATASET_DEFAULT_NAME+"\\" + outFeature;
                toLine.neighbor_option = "IDENTIFY_NEIGHBORS";
                
                IGeoProcessorResult results = (IGeoProcessorResult)gp.Execute(toLine, null);

                if (gp.MessageCount > 0)
                {
                    for (int Count = 0; Count <= gp.MessageCount - 1; Count++)
                    {
                        lblstatus.Text += gp.GetMessage(Count) + "\r\n";
                    }
                }
                if (lblstatus.Text.Contains("ERROR") || lblstatus.Text.Contains("失败"))
                {
                    err = lblstatus.Text;
                    return false;
                }
                else
                {
                    return true;
                }
                
            }
            catch (Exception ex)
            {
                err = ex.Message;
                return false;
            }
            
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (cmbPolygonLayer.Text.Trim()=="")
                return;
            if (cmbPolylineLayer.Text.Trim()=="")
                return;
            lblstatus.Text = "开始转化...";
            Application.DoEvents();
            this.Cursor = Cursors.WaitCursor;
            string inLayerName = OtherHelper.GetLeftName(this.cmbPolygonLayer.Text);
            string tmpLayerName = inLayerName + "_tmpLine";
            string err="";
            if (!gpExcute(inLayerName, tmpLayerName,ref err))
            {
                MessageBox.Show("GP执行失败，第一步没过去。可能权限不够。\r\n"+err);
                this.Cursor = Cursors.Default;
                return;
            }
            try
            {
                //将 所有要素类数据拷贝到 线层
                IFeatureClass srcClass = (CurrWorkspace as IFeatureWorkspace).OpenFeatureClass(tmpLayerName);
                IFeatureClass destClass = (CurrWorkspace as IFeatureWorkspace).OpenFeatureClass(OtherHelper.GetLeftName( cmbPolylineLayer.Text.Trim()));

                IFeatureCursor featureCursor = null;
                IWorkspaceEdit pWSEDit = CurrWorkspace as IWorkspaceEdit;
                pWSEDit.StartEditing(true);
                pWSEDit.StartEditOperation();
                
                try
                {
                    
                    IFeatureBuffer pfeatureBuffer = destClass.CreateFeatureBuffer();
                    featureCursor = destClass.Insert(true);
                    IFeatureLayer pSrcLyr = new FeatureLayerClass();
                    pSrcLyr.FeatureClass = srcClass;
                    IIdentify pIdSrc = pSrcLyr as IIdentify;
                    IArray pArSrc = pIdSrc.Identify((srcClass as IGeoDataset).Extent);
                    if (pArSrc != null)
                    {
                        for (int i = 0; i < pArSrc.Count; i++)
                        {
                            IFeatureIdentifyObj obj = pArSrc.get_Element(i) as IFeatureIdentifyObj;
                            IRowIdentifyObject aRow = obj as IRowIdentifyObject;
                            IFeature aLine = aRow.Row as IFeature;

                            pfeatureBuffer.Shape = aLine.ShapeCopy;
                            featureCursor.InsertFeature(pfeatureBuffer);
                            lblstatus.Text = "当前导入第" + (i + 1) + "条数据...";
                            Application.DoEvents();
                        }
                        featureCursor.Flush();
                    }

                    pWSEDit.StopEditOperation();
                    pWSEDit.StopEditing(true);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(featureCursor);
                }
                catch(Exception ex)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(featureCursor);
                    pWSEDit.AbortEditOperation();
                    pWSEDit.StopEditing(false);
                }
                
                //删除临时图层
                lblstatus.Text = "删除临时图层";
                Application.DoEvents();
                (srcClass as IDataset).Delete();
                MessageBox.Show("转换完成！");
                
            }
            catch (Exception ex) { MessageBox.Show("失败"+ex.Message+".\r\n"); }
            finally
            {
                this.lblstatus.Text = "";
                this.Cursor = Cursors.Default;
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}