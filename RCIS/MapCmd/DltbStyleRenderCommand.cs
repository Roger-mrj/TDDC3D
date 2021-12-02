using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;

using RCIS.GISCommon;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System.Collections;
using ESRI.ArcGIS.Display;
using RCIS.Database;

namespace RCIS.MapTool
{
    /// <summary>
    /// Command that works in ArcMap/Map/PageLayout
    /// </summary>
    [Guid("9af45f21-b9a8-41b2-bac6-bf74005b78b5")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("RCIS.MapTool.DltbStyleRenderCommand")]
    public sealed class DltbStyleRenderCommand : BaseCommand
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
        private AxTOCControl tocc = null;
        private IHookHelper m_hookHelper = null;
        public DltbStyleRenderCommand(AxTOCControl _tocc)
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = ""; //localizable text
            base.m_caption = "使用地类图斑符号化显示";  //localizable text 
            base.m_message = "This should work in ArcMap/MapControl/PageLayoutControl";  //localizable text
            base.m_toolTip = "使用地类图斑符号化显示";  //localizable text
            base.m_name = "";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")

            try
            {
                this.tocc = _tocc;
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

        ISymbol getSymbolFromStyleFile(IStyleGallery styleGalley, string stylefile, string symboltype, string symName)
        {

            IStyleGalleryItem styleGalleryItem;
            IEnumStyleGalleryItem enumStyleGalleryItem = null;

            enumStyleGalleryItem = styleGalley.get_Items(symboltype, stylefile, "");
            enumStyleGalleryItem.Reset();
            styleGalleryItem = enumStyleGalleryItem.Next();
            ISymbol symbol = null;
            while (styleGalleryItem != null)
            {
                if (styleGalleryItem.Name.ToUpper().Trim() == symName.ToUpper().Trim())
                {
                    symbol = (ISymbol)styleGalleryItem.Item;
                    break;
                }
                styleGalleryItem = enumStyleGalleryItem.Next();
            }

            System.Runtime.InteropServices.Marshal.ReleaseComObject(enumStyleGalleryItem);
            return symbol as ISymbol;
        }


        #region Overridden Class Methods

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
            // TODO: Add StyleRenderCommand.OnClick implementation

            IMapControl3 mapCtl = m_hookHelper.Hook as IMapControl3;
            object customProperty = mapCtl.CustomProperty;
            if (customProperty == null)
                return;
            ILayer selLyr = customProperty as ILayer;
            IFeatureLayer selFeaLyr = selLyr as IFeatureLayer;
            if (selFeaLyr == null)
                return;
            IFeatureClass selFeaClass = selFeaLyr.FeatureClass;
            string className = (selFeaClass as IDataset).Name.ToUpper();
            if (selLyr is IGeoFeatureLayer)
            {
                IGeoFeatureLayer pGeolyr = selLyr as IGeoFeatureLayer;
                esriGeometryType geoType = selFeaClass.ShapeType;

                ISymbol defaultSym = null;

                string symType = "Fill Symbols";
                defaultSym = SymbolHelper.CreateFillSymbol(System.Drawing.Color.AliceBlue, System.Drawing.Color.Blue) as ISymbol;
                if (esriGeometryType.esriGeometryPoint == geoType)
                {
                    symType = "Market Symbols";
                    defaultSym = SymbolHelper.CreateSimpleMarkerSymbol(System.Drawing.Color.AliceBlue, 5) as ISymbol;
                }
                else if (esriGeometryType.esriGeometryPolyline == geoType)
                {
                    symType = "Line Symbols";
                    defaultSym = SymbolHelper.CreateSimpleLineSymbol(System.Drawing.Color.Blue, 2) as ISymbol;
                }


                //先查找 setup中有没有相关配置
                string sql = "select * from SYS_SYMBOLSETUP where featureclass='DLTB' order by  FIELDVALUE ";
                System.Data.DataTable dt = LS_SetupMDBHelper.GetDataTable(sql, "tmp");
                if (dt.Rows.Count == 0)
                    return;
                //找到所有 值，获取符号名
                System.Data.DataRow dr = dt.Rows[0];
                string field = dr["FIELD"].ToString().Trim();

                string styleFile = RCIS.Global.AppParameters.StylePath + @"\style.ServerStyle";
                IStyleGallery styleGalley = new ESRI.ArcGIS.Display.ServerStyleGalleryClass();
                IStyleGalleryStorage styleGalleryStorage = styleGalley as IStyleGalleryStorage;
                styleGalleryStorage.AddFile(styleFile);


                IUniqueValueRenderer pRender = new UniqueValueRendererClass();


                pRender.FieldCount = 1;
                pRender.set_Field(0, field);
                foreach (System.Data.DataRow aRow in dt.Rows)
                {
                    string symName = aRow["SYMBOLNAME"].ToString();
                    string value = aRow["FIELDVALUE"].ToString();
                    ISymbol symbol = this.getSymbolFromStyleFile(styleGalley, styleFile, symType, symName);
                    if (symbol != null)
                    {
                        pRender.AddValue(value, "", symbol as ISymbol);
                        pRender.set_Label(value, symName);
                        pRender.set_Symbol(value, symbol as ISymbol);
                    }

                }
                pRender.DefaultSymbol = defaultSym;
                pRender.DefaultLabel = "";
                pRender.UseDefaultSymbol = true;

                pGeolyr.Renderer = (IFeatureRenderer)pRender;
                mapCtl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, pGeolyr, mapCtl.ActiveView.Extent);


                this.tocc.Update();


            }

        }

        #endregion
    }
}
