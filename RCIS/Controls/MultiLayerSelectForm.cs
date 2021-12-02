using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using RCIS.Utility;
using RCIS.GISCommon;

namespace RCIS.Controls
{
    public partial class MultiLayerSelectForm : Form
    {
        public MultiLayerSelectForm(IMap pMap            
            ,string pCaption
            ,string pTitle)
        {
            InitializeComponent();
            this.m_pMap = pMap;
            
            this.Text = pCaption;
            
        }
        
        
        private IMap m_pMap;
        private esriGeometryType m_gt;
        private List<IGeoFeatureLayer> m_selList;


        private ISymbol retSymbol = null;

        public ISymbol RetSymbol
        {
            get { return retSymbol; }
            set { retSymbol = value; }
        }


      
        
        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            for (int ii = 0; ii < this.layerList.Items.Count; ii++)
                this.layerList.SetItemChecked(ii, true);
        }

        private void btnUnSelectAll_Click(object sender, EventArgs e)
        {
            for (int ii = 0; ii < this.layerList.Items.Count; ii++)
                this.layerList.SetItemChecked(ii, false);
        }
        public List<IGeoFeatureLayer> SelectedLayerList
        {
            get
            {
                return this.m_selList;
            }
        }
        public void GetSelectedLayerList()
        {
            
                this.m_selList = new List<IGeoFeatureLayer>();
                for (int ii = 0; ii < this.layerList.Items.Count; ii++)
                {
                    ListBoxItem aItem = this.layerList.Items[ii] as ListBoxItem;
                    if (this.layerList.GetItemChecked(ii))
                    {
                        IGeoFeatureLayer aLayer = aItem.ItemObject as IGeoFeatureLayer;
                        if (aLayer != null)
                        {
                            if (aLayer.Valid == true)
                            {
                                this.m_selList.Add(aLayer);
                            }
                        }
                    }
                }
            
        }

        private ISymbol CreateSymbol()
        {
            try
            {
                IColor aFillColor = ColorHelper.CreateColor(this.ceFill.Color);
                IColor aOutlineColor = ColorHelper.CreateColor(this.ceOutline.Color);
                double aSize = 1;
                double.TryParse(this.teSize.Text, System.Globalization.NumberStyles.Any
                    , new System.Globalization.NumberFormatInfo()
                    , out aSize);
                double aWidth = 1;
                double.TryParse(this.teWidth.Text, System.Globalization.NumberStyles.Any
                    , new System.Globalization.NumberFormatInfo()
                    , out aWidth);

               
                    SimpleFillSymbolClass aStyle = new SimpleFillSymbolClass();
                    aStyle.Color = ColorHelper.CreateColor((int)this.ceFill.Color.R, (int)this.ceFill.Color.G, (int)this.ceFill.Color.B);
                    SimpleLineSymbolClass aOutline = new SimpleLineSymbolClass();
                    aOutline.Color = aOutlineColor;
                    aOutline.Width = aWidth;
                    aStyle.Outline = aOutline;

                    if (this.ceFill.Color.Name.ToUpper().Equals("TRANSPARENT"))
                    {
                        aStyle.Style = esriSimpleFillStyle.esriSFSHollow;
                    }

                    return aStyle as ISymbol;
                
            }
            catch
            {
            }
            return null;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.m_selList = new List<IGeoFeatureLayer>();
            this.GetSelectedLayerList();


            this.retSymbol = this.CreateSymbol();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.m_selList = new List<IGeoFeatureLayer>();
            
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void MultiLayerSelectForm_Load(object sender, EventArgs e)
        {
            this.m_selList = new List<IGeoFeatureLayer>();
            if (this.m_pMap != null)
            {
                int order = 1;
                for (int li = 0; li < this.m_pMap.LayerCount; li++)
                {
                    ILayer pLayer = this.m_pMap.get_Layer(li);
                    if (pLayer.Valid == false)
                        continue;
                    if (!pLayer.Visible)
                    {
                        continue;
                    }
                    if (pLayer is IGroupLayer)
                    {
                        ICompositeLayer pCLyr = pLayer as ICompositeLayer;
                        for (int j = 0; j < pCLyr.Count; j++)
                        {
                            IFeatureLayer flyr = pCLyr.get_Layer(j) as IFeatureLayer;
                            if (flyr == null) continue;
                            IGeoFeatureLayer aLayer = pLayer as IGeoFeatureLayer;
                            esriGeometryType aGT = aLayer.FeatureClass.ShapeType;
                            if (aGT == esriGeometryType.esriGeometryPolygon)
                            {                                ListBoxItem aItem = new ListBoxItem(aLayer, aLayer.Name, order);
                                order++;
                                if (aLayer.Name.Contains("DLTB"))
                                {
                                    this.layerList.Items.Add(aItem, true);
                                }
                                else
                                {
                                    this.layerList.Items.Add(aItem, false);
                                }

                            }
                        }

                    }
                    else if (pLayer is IFeatureLayer)
                    {
                        IGeoFeatureLayer aLayer = pLayer as IGeoFeatureLayer;
                        esriGeometryType aGT = aLayer.FeatureClass.ShapeType;
                        if (aGT == esriGeometryType.esriGeometryPolygon)
                        {

                            ListBoxItem aItem = new ListBoxItem(aLayer, aLayer.Name, order);
                            order++;
                            if (aLayer.Name.Contains("DLTB"))
                            {
                                this.layerList.Items.Add(aItem, true);
                            }
                            else
                            {
                                this.layerList.Items.Add(aItem, false);
                            }

                        }
                    }


                   

                   
                }
            }
        }

    }

    public class ListBoxItem : RCIS.Utility.ComboBoxItem
    {
        public ListBoxItem(object pTag, string pText, int pIndex)
            : base(pTag, pText, pIndex)
        {

        }

    }

}