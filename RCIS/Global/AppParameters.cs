using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.esriSystem;

using RCIS.Utility;
namespace RCIS.Global
{
    public class AppParameters
    {
        public static string LAYER_DLTB = "DLTB";
        public static string LAYER_XZQ = "XZQ";
        
        public static string DATASET_DEFAULT_NAME = "TDDC";
        public static string DATASET_GX_NAME = "TDGX";
        public static string DATASET_TF_NAME = "TF";

        /// <summary>
        /// 是否更新
        /// </summary>
        public static bool GX_HISTORY = false;

        /// <summary>
        /// 移动粘滞容差
        /// </summary>
        public static int EDIT_STICKMOVETOLERANCE = 0;

        /// <summary>
        /// 是否过滤显示
        /// </summary>
        public static bool DISPLAY_FILTER = false;



        /// <summary>
        /// 初始化一些全局配置的选项
        /// </summary>
        public static void InitElseOption()
        {
            INIHelper ini = new INIHelper(RCIS.Global.AppParameters.ConfPath + "\\Setup.ini");
            string sb = ini.IniReadValue("system", "gxhistory");
            bool bb = false;
            bool.TryParse(sb, out bb);
            RCIS.Global.AppParameters.GX_HISTORY = bb; //是否开启历史

            sb = ini.IniReadValue("system", "filter");
            bb = false;
            bool.TryParse(sb, out bb);
            RCIS.Global.AppParameters.DISPLAY_FILTER = bb;

            string stickMoveTolerance = ini.IniReadValue("system", "stickmovetolerance");
            int iTol = 10;
            int.TryParse(stickMoveTolerance, out iTol);
            RCIS.Global.AppParameters.EDIT_STICKMOVETOLERANCE = iTol;



        }

        /// <summary>
        /// VCT导入临时路径
        /// </summary>
        public static string VCTIN_TMP
        {
            get
            {
                string temppath = System.Environment.GetEnvironmentVariable("TEMP");
                if (temppath.EndsWith("\\"))
                    temppath += "VCTProcess";
                else
                    temppath += "\\VCTProcess";

                return temppath;
            }
        }

        /// <summary>
        /// VCT导出临时路径
        /// </summary>
        public static string VCTOut_TMP
        {
            get
            {
                return System.Windows.Forms.Application.StartupPath + "\\VCTEX";
            }
        }
        

        /// <summary>
        /// 配置数据库及配置文件所在路径
        /// </summary>
        /// <returns></returns>
        public static string ConfPath
        {
            get
            {
                return System.Windows.Forms.Application.StartupPath + @"\SystemConf";
            }
        }
         

        /// <summary>
        /// 图像路径
        /// </summary>
        /// <returns></returns>
        public static string ImgPath
        {
            get
            {
                return System.Windows.Forms.Application.StartupPath + @"\image";
            }
        }

        /// <summary>
        /// 符号库所在路径
        /// </summary>
        /// <returns></returns>
        public static string StylePath
        {
            get
            {
                return System.Windows.Forms.Application.StartupPath + @"\style";
            }
        }

        public static string TemplatePath
        {
            get
            {
                return System.Windows.Forms.Application.StartupPath + @"\template";
            }
        }

        public static string OutputPath
        {
            get
            {
                return System.Windows.Forms.Application.StartupPath + @"\output";
            }
        }


        static private IAoInitialize m_AoInitialize = new AoInitializeClass();


        //public static bool getDbeditEngine()
        //{
        //    ESRI.ArcGIS.esriSystem.esriLicenseStatus licenseStatus =
        //          (ESRI.ArcGIS.esriSystem.esriLicenseStatus)m_AoInitialize.IsProductCodeAvailable(esriLicenseProductCode.esriLicenseProductCodeEngineGeoDB); // .esriLicenseProductCodeEngineGeoDB);
        //    if (licenseStatus == esriLicenseStatus.esriLicenseAvailable)
        //    {
        //        licenseStatus = (esriLicenseStatus)m_AoInitialize.Initialize(esriLicenseProductCode.esriLicenseProductCodeEngineGeoDB);
        //        if (licenseStatus != esriLicenseStatus.esriLicenseCheckedOut)
        //        {

        //            System.Windows.Forms.MessageBox.Show("没有ArcEngine中的Standard许可，可能部分功能受限!", "系统提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
        //            return false;
        //        }
        //    }
        //    else
        //    {
        //        System.Windows.Forms.MessageBox.Show("没有ArcEngine中的Standard许可，可能部分功能受限!", "系统提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
        //        return false;
        //    }
        //}


        static public bool Startup()
        {
            try
            {
                if (m_AoInitialize == null)
                {
                    System.Windows.Forms.MessageBox.Show("没有安装ArcEngine,系统无法运行!", "系统提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }

                ESRI.ArcGIS.esriSystem.esriLicenseStatus
                        licenseStatus =
                //       (ESRI.ArcGIS.esriSystem.esriLicenseStatus)m_AoInitialize.IsProductCodeAvailable(esriLicenseProductCode.esriLicenseProductCodeEngineGeoDB);
                //if (licenseStatus == esriLicenseStatus.esriLicenseAvailable)
                //{
                //    licenseStatus = (esriLicenseStatus)m_AoInitialize.Initialize(esriLicenseProductCode.esriLicenseProductCodeEngineGeoDB);
                //    if (licenseStatus != esriLicenseStatus.esriLicenseCheckedOut)
                //    {

                //        System.Windows.Forms.MessageBox.Show("没有基础编辑工具许可!", "系统提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                //        return false;
                //    }
                //}
                //else
                //{
                //    System.Windows.Forms.MessageBox.Show("没有基础编辑工具许可!", "系统提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                //    return false;
                //}

                licenseStatus =
                (ESRI.ArcGIS.esriSystem.esriLicenseStatus)m_AoInitialize.IsProductCodeAvailable(esriLicenseProductCode.esriLicenseProductCodeStandard); // .esriLicenseProductCodeEngineGeoDB);
                if (licenseStatus == esriLicenseStatus.esriLicenseAvailable)
                {
                    licenseStatus = (esriLicenseStatus)m_AoInitialize.Initialize(esriLicenseProductCode.esriLicenseProductCodeStandard);
                    if (licenseStatus != esriLicenseStatus.esriLicenseCheckedOut)
                    {

                        System.Windows.Forms.MessageBox.Show("没有ArcEngine中的Standard许可，可能部分功能受限!", "系统提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        return false;
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("没有ArcEngine中的Standard许可，可能部分功能受限!", "系统提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }

                licenseStatus =
                   (ESRI.ArcGIS.esriSystem.esriLicenseStatus)m_AoInitialize.IsProductCodeAvailable(esriLicenseProductCode.esriLicenseProductCodeAdvanced);
                if (licenseStatus == esriLicenseStatus.esriLicenseAvailable)
                {
                    licenseStatus = (esriLicenseStatus)m_AoInitialize.Initialize(esriLicenseProductCode.esriLicenseProductCodeAdvanced);
                    if (licenseStatus != esriLicenseStatus.esriLicenseCheckedOut)
                    {

                        System.Windows.Forms.MessageBox.Show("没有部分toolbox工具许可!", "系统提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        return false;
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("没有部分toolbox工具许可!", "系统提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }

                //licenseStatus = m_AoInitialize.IsExtensionCodeAvailable(esriLicenseProductCode.esriLicenseProductCodeEngine,
                //    esriLicenseExtensionCode.esriLicenseExtensionCodeNetwork);
                //licenseStatus = (ESRI.ArcGIS.esriSystem.esriLicenseStatus)m_AoInitialize.CheckOutExtension(esriLicenseExtensionCode.esriLicenseExtensionCodeNetwork);
                //if (licenseStatus == esriLicenseStatus.esriLicenseCheckedOut)
                //{

                //}
                //else
                //{
                //    System.Windows.Forms.MessageBox.Show("没有ArcEngine中的Network许可!", "系统提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                //    return false;
                //}
            }
            catch
            {
                //System.Windows.Forms.MessageBox.Show(ex.Message);  
                System.Windows.Forms.MessageBox.Show("ArcEngine的许可出错!", "系统提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        //关闭系统  
        static public void Shutdown()
        {
            if (m_AoInitialize != null)
            {

                m_AoInitialize.Shutdown();
            }
        } 

    }
}
