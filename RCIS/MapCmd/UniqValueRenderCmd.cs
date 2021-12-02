using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.SystemUI;

namespace RCIS.MapCmd
{
    /// <summary>
    /// Command that works in ArcMap/Map/PageLayout
    /// </summary>
    [Guid("45ffe0a9-877a-44b0-a45b-561581dd9972")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("RCIS.MapCmd.UniqValueRenderCmd")]
    public sealed class UniqValueRenderCmd : BaseCommand
    {
        #region COM Registration Function(s)
        [ComRegisterFunction()]
        [ComVisible(false)]
        static void RegisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryRegistration(registerType);

            //
            // TODO: Add any COM registration code here
            //
        }

        [ComUnregisterFunction()]
        [ComVisible(false)]
        static void UnregisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryUnregistration(registerType);

            //
            // TODO: Add any COM unregistration code here
            //
        }

        #region ArcGIS Component Category Registrar generated code
        /// <summary>
        /// Required method for ArcGIS Component Category registration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryRegistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            MxCommands.Register(regKey);
            ControlsCommands.Register(regKey);
        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            MxCommands.Unregister(regKey);
            ControlsCommands.Unregister(regKey);
        }

        #endregion
        #endregion

        private AxTOCControl m_tocc = null;
        private IHookHelper m_hookHelper = null;
        public UniqValueRenderCmd(AxTOCControl tocc)
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = ""; //localizable text
            base.m_caption = "唯一值渲染";  //localizable text 
            base.m_message = "This should work in ArcMap/MapControl/PageLayoutControl";  //localizable text
            base.m_toolTip = "";  //localizable text
            base.m_name = "";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")

            try
            {
                this.m_tocc = tocc;
                //
                // TODO: change bitmap name if necessary
                //
                string bitmapResourceName = GetType().Name + ".bmp";
                base.m_bitmap = new Bitmap(GetType(), bitmapResourceName);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
            }
        }

        #region Overriden Class Methods

        private ISymbol CreateTmpSym(esriGeometryType type)
        {
            ISymbol sym = null;
            switch (type)
            {
                case esriGeometryType.esriGeometryPoint:
                    sym = new SimpleMarkerSymbolClass();
                    (sym as ISimpleMarkerSymbol).Size = 3;
                    (sym as ISimpleMarkerSymbol).Style = esriSimpleMarkerStyle.esriSMSDiamond;
                    break;
                case esriGeometryType.esriGeometryPolyline:
                    sym = new SimpleLineSymbolClass();
                    (sym as ISimpleLineSymbol).Width = 1;
                    break;
                case esriGeometryType.esriGeometryPolygon:
                    sym = new SimpleFillSymbolClass();
                    (sym as ISimpleFillSymbol).Style = esriSimpleFillStyle.esriSFSSolid;
                    (sym as ISimpleFillSymbol).Outline.Width = 0.5;
                    break;

            }
            return sym;
        }

        /// <summary>
        /// Occurs when this command is created
        /// </summary>
        /// <param name="hook">Instance of the application</param>
        public override void OnCreate(object hook)
        {
            if (hook == null)
                return;

            try
            {
                m_hookHelper = new HookHelperClass();
                m_hookHelper.Hook = hook;
                if (m_hookHelper.ActiveView == null)
                    m_hookHelper = null;
            }
            catch
            {
                m_hookHelper = null;
            }

            if (m_hookHelper == null)
                base.m_enabled = false;
            else
                base.m_enabled = true;

            // TODO:  Add other initialization code
        }

        /// <summary>
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
        {
            IMapControl3 mapCtl = (IMapControl3)m_hookHelper.Hook;

            object customProperty = mapCtl.CustomProperty;
            if (customProperty == null)
                return;
            ILayer pSelSymLayer = customProperty as ILayer;
            // TODO: Add UniqValueRenderCmd.OnClick implementation
            if (pSelSymLayer == null)
                return;
            if (!(pSelSymLayer is IFeatureLayer))
            {
                return;
            }
            IFeatureClass selFeatureClass = (pSelSymLayer as IFeatureLayer).FeatureClass;
            if (selFeatureClass == null)
                return;

            IGeoFeatureLayer pLyr = (pSelSymLayer as IGeoFeatureLayer);
            if (pLyr == null)
                return;

            RCIS.Controls.FrmUniqvalueRender frm = new RCIS.Controls.FrmUniqvalueRender();
            frm.currFeatClass = selFeatureClass;
            if (frm.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            string field = frm.resultField;
            if (field == "")
                return;

            IRandomColorRamp rx = new RandomColorRampClass();  //随机颜色
            rx.MinSaturation = 20;
            rx.MaxSaturation = 40;
            rx.MinValue = 85;
            rx.MaxValue = 100;
            rx.EndHue = 188;
            rx.UseSeed = true;
            rx.Seed = 43;



            try
            {
                IDataset selDs = selFeatureClass as IDataset;
                IFeatureWorkspace featureWorkspace = (IFeatureWorkspace)(selDs.Workspace);
                IQueryDef queryDef = featureWorkspace.CreateQueryDef();
                queryDef.Tables = selDs.Name;
                queryDef.SubFields = field;
                ICursor cursor;
                cursor = queryDef.Evaluate();


                IUniqueValueRenderer pRender = new UniqueValueRendererClass();
                

                #region 获取唯一值

                ISymbol symd = CreateTmpSym(selFeatureClass.ShapeType);

                pRender.FieldCount = 1;
                pRender.set_Field(0, field);
                pRender.DefaultSymbol = symd as ISymbol;
                pRender.UseDefaultSymbol = true;


                bool ValFound = false;
                bool NoValFound = true;
                IRow pFeat = cursor.NextRow();
                try
                {

                    while (pFeat != null)
                    {

                        ISymbol symx = CreateTmpSym(selFeatureClass.ShapeType);
                        string x = pFeat.get_Value(0).ToString();  //获取该字段的值，
                        ValFound = false;
                        for (int uh = 0; uh < pRender.ValueCount - 1; uh++)
                        {
                            if (pRender.get_Value(uh).ToString().ToUpper().Equals(x.ToUpper()))
                            {
                                NoValFound = true;
                                break;
                            }
                        }
                        //加入新值
                        if (!ValFound)
                        {
                            pRender.AddValue(x, field, symx as ISymbol);
                            pRender.set_Label(x, x);
                            pRender.set_Symbol(x, symx as ISymbol);
                        }


                        pFeat = cursor.NextRow();
                    }

                }
                catch { }
                finally
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(cursor);
                }
                #endregion


                #region 渲染
                rx.Size = pRender.ValueCount;
                bool isOk = false;
                rx.CreateRamp(out isOk);
                IEnumColors RColors = rx.Colors;
                RColors.Reset();

                for (int ny = 0; ny < pRender.ValueCount; ny++)
                {
                    string xv = pRender.get_Value(ny);
                    if (xv != "")
                    {
                        ISymbol jsy = pRender.get_Symbol(xv);
                        switch (selFeatureClass.ShapeType)
                        {
                            case esriGeometryType.esriGeometryPolygon:
                                (jsy as ISimpleFillSymbol).Color = RColors.Next();
                                break;
                            case esriGeometryType.esriGeometryPoint:
                                (jsy as ISimpleMarkerSymbol).Color = RColors.Next();
                                (jsy as ISimpleMarkerSymbol).Size = (jsy as ISimpleMarkerSymbol).Size + 0.5 * ny;
                                break;
                            case esriGeometryType.esriGeometryPolyline:
                                (jsy as ISimpleLineSymbol).Color = RColors.Next();
                                (jsy as ISimpleLineSymbol).Width = (jsy as ISimpleLineSymbol).Width + 0.5 * ny;
                                break;
                        }

                        pRender.set_Symbol(xv, jsy);
                    }
                }

                pRender.ColorScheme = "Custom";
                pRender.set_FieldType(0, true);

                pLyr.Renderer = pRender as IFeatureRenderer;
                pLyr.DisplayField = field;               

                this.m_hookHelper.ActiveView.Refresh();
                this.m_tocc.Update();
                #endregion
            }
            catch (Exception ex) { }
          
        }

        #endregion
    }
}
