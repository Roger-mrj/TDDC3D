using System;
using System.Windows.Forms;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS;

namespace TDDC3D
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            

            //string regstr = RegHelper.ReadReg();
            //if (regstr != "1")
            //{
            //    sys.RegSoftForm frm = new sys.RegSoftForm();
            //    if (frm.ShowDialog() == DialogResult.OK)
            //    {
            //        try
            //        {
            //            ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.EngineOrDesktop);
            //            RCIS.Global.AppParameters.Startup();


            //            Application.EnableVisualStyles();
            //            Application.SetCompatibleTextRenderingDefault(false);

            //            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("zh-Hans");
            //            Application.Run(new LSMainForm());

            //            RCIS.Global.AppParameters.Shutdown();

            //        }
            //        catch (Exception ex)
            //        {
            //            MessageBox.Show(ex.Message);
            //        }
            //    }
            //    else
            //    {
            //        Application.Exit();
            //    }
            //}

            //ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.EngineOrDesktop);
          //  RCIS.Global.AppParameters.Startup();
            if (!RuntimeManager.Bind(ProductCode.Desktop))
            {
                MessageBox.Show("Unable to bind to ArcGIS runtime. Application will be shut down.");
                return;
            }
            //IAoInitialize pAoInit = new AoInitializeClass(); ;
            //pAoInit.Initialize(esriLicenseProductCode.esriLicenseProductCodeAdvanced);//初始化桌面高级许可
            ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.EngineOrDesktop);


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("zh-Hans");
            Application.Run(new LSMainForm());

          //  RCIS.Global.AppParameters.Shutdown();
            //pAoInit.Shutdown();


            
        }
    }
}
