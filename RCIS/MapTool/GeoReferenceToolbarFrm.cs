using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Display;

using RCIS.Global;

namespace RCIS.MapTool
{
    public partial class GeoReferenceToolbarFrm : Form
    {
        public GeoReferenceToolbarFrm(IHookHelper hook)
        {
            InitializeComponent();
            this.m_hookHelper = hook;

            pMap = this.m_hookHelper.FocusMap;



            this.cmbLayers.Properties.Items.Clear();
            for (int i = 0; i < pMap.LayerCount; i++)
            {
                ILayer currLyr = pMap.get_Layer(i);
                if (currLyr is IGroupLayer)
                {
                    ICompositeLayer pComLyr = currLyr as ICompositeLayer;
                    for (int kk = 0; kk < pComLyr.Count; kk++)
                    {
                        ILayer childLyr = pComLyr.get_Layer(kk);
                        if (childLyr is IRasterLayer)
                        {
                            this.cmbLayers.Properties.Items.Add(currLyr.Name);
                        }
                        else if (childLyr is IFeatureLayer)
                        {
                            string name = ((currLyr as IFeatureLayer).FeatureClass as IDataset).Name.ToUpper();
                            this.cmbFeatureLayer.Properties.Items.Add(name + "|" + currLyr.Name);
                        }
                    }
                }
                else 
                if (currLyr is IRasterLayer)
                {
                    this.cmbLayers.Properties.Items.Add(   currLyr.Name);
                }
                else if (currLyr is IFeatureLayer)
                {
                    string name = ((currLyr as IFeatureLayer).FeatureClass as IDataset).Name.ToUpper();
                    this.cmbFeatureLayer.Properties.Items.Add(name+"|"+currLyr.Name);
                }
                

            }
            if (this.cmbLayers.Properties.Items.Count > 0)
            {
                this.cmbLayers.SelectedIndex = 0;
                
            }
            if (this.cmbFeatureLayer.Properties.Items.Count > 0)
            {
                this.cmbFeatureLayer.SelectedIndex = 0;
            }

        }

        private IHookHelper m_hookHelper = null;
        private PointType ptType = PointType.Null;

        public PointType PtType
        {
            get { return ptType; }
            set { ptType = value; }
        }

        private IMap pMap;

        public IRasterLayer currRasterLyr
        {
            get
            {
                string txt = this.cmbLayers.Text.Trim();  //获取影像图层名
                if (txt=="")  return null;
                else
                {
                    for (int i = 0; i < pMap.LayerCount; i++)
                    {
                        ILayer currLyr = pMap.get_Layer(i);
                        if (currLyr is IGroupLayer)
                        {
                            ICompositeLayer pComLyr = currLyr as ICompositeLayer;
                            for (int kk = 0; kk < pComLyr.Count; kk++)
                            {
                                ILayer childLyr = pComLyr.get_Layer(kk);
                                if (currLyr.Name.ToUpper() == txt.ToUpper())
                                    return (currLyr as IRasterLayer);
                            }
                        }
                        else
                        {
                            if (currLyr.Name.ToUpper() == txt.ToUpper())
                                return (currLyr as IRasterLayer);
                        }

                    }
                    return null;
                }
            }
        }
             

        private void GeoReferenceToolbarFrm_Load(object sender, EventArgs e)
        {
            
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.ptType = PointType.ImgPoint;

           
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.ptType = PointType.GeoPoint;

           

        }

        
        public event GeoReference_EventHandler geoRefEvent; //定义一个委托
        public event GeoReference_ClearPoint_EventHandler clearPtEvent;
        public event GeoReference_Reset_EventHandler resetEvent;
        public event GeoReference_Register_EventHandler registerEvent; 

        private void simpleButton3_Click(object sender, EventArgs e)
        {
           
            //获取对应图层，执行配准
            geoRefEvent();

        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
           
            clearPtEvent();
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
           
            resetEvent();
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            
            registerEvent();
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            IMapControl3 mapctl = this.m_hookHelper.Hook as IMapControl3;
            mapctl.CurrentTool = null;
            Close();
        }
    }
}
