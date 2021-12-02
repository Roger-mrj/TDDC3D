using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using System.Windows.Forms;
using System.IO;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;

using RCIS.Utility;
using RCIS.GISCommon;
namespace RCIS.MapTool
{
    /// <summary>
    /// Command that works in ArcMap/Map/PageLayout
    /// </summary>
    [Guid("125377c3-1f53-4d6c-8886-8f5f89d328d1")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("RCIS.MapTool.GeometryExportCommand")]
    public sealed class GeometryExportCommand : BaseCommand
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

        private IHookHelper m_hookHelper = null;
        public GeometryExportCommand()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = ""; //localizable text
            base.m_caption = "导出图形到绝对坐标";  //localizable text 
            base.m_message = "This should work in ArcMap/MapControl/PageLayoutControl";  //localizable text
            base.m_toolTip = "";  //localizable text
            base.m_name = "";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")

            try
            {
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
            // TODO: Add GeometryExportCommand.OnClick implementation
            if (m_hookHelper.ActiveView == null)
            {
                return;
            }
            IMap aMap = m_hookHelper.ActiveView.FocusMap;
            IEnumFeature aEnumObj = aMap.FeatureSelection as IEnumFeature;
            if (aEnumObj.Next() == null)
            {
                MessageBox.Show("当前地图没有被选中要素!");
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "将选中图形坐标输出到文件";
            sfd.Filter = "文本文件(*.txt)|*.txt";
            sfd.AddExtension = true;
            sfd.OverwritePrompt = true;
            sfd.CreatePrompt = true;
            sfd.ValidateNames = true;
            sfd.FileName = "pts" + TextHelper.FormatDateTime(DateTime.Now, "");
            if (sfd.ShowDialog()
                == DialogResult.OK)
            {
                string aFilePath = sfd.FileName;
                StreamWriter aWriter = null;
                if (File.Exists(aFilePath))
                {
                    aWriter = new StreamWriter(File.OpenWrite(aFilePath));
                }
                else
                {
                    aWriter = File.CreateText(aFilePath);
                }

                aEnumObj.Reset();
                IFeature aObj = aEnumObj.Next();
                while (aObj != null)
                {
                    IGeometry aGeom = aObj.ShapeCopy;
                    if (aGeom != null && !aGeom.IsEmpty)
                    {
                        if (aGeom is IPoint)
                        {
                            string aLine = GeometryHelper.FormatPoint(aGeom as IPoint, false, ',');
                            aWriter.WriteLine(aLine);
                        }
                        else if (aGeom is IPointCollection)
                        {
                            IPointCollection aPtCol = aGeom as IPointCollection;
                            int pc = aPtCol.PointCount;
                            for (int pi = 0; pi < pc; pi++)
                            {
                                IPoint aPt = aPtCol.get_Point(pi);
                                string aLine = GeometryHelper.FormatPoint(aPt, false, ',');
                                aWriter.WriteLine(aLine);
                            }
                        }
                        aWriter.WriteLine();
                    }

                    aObj = aEnumObj.Next();
                }
                aWriter.Close();
                MessageBox.Show("坐标导出结束!", "坐标点导出"
                    , MessageBoxButtons.OK
                    , MessageBoxIcon.Information);
            }
        }

        #endregion
    }
}
