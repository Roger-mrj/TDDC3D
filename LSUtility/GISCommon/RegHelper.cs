using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft;
using Microsoft.Win32;

namespace RCIS.GISCommon
{
    public static class RegHelper
    {
        public static void CurrentUser()
        {
            RegistryKey key = Registry.Users;
            RegistryKey software = key.CreateSubKey("software\\Ls3dsoftkey");
            software.SetValue("isregisted", 1);
            key.Close();
        }

        public static string ReadReg()
        {
            RegistryKey key = Registry.CurrentUser;
            //在HKEY_LOCAL_MACHINE\SOFTWARE下新建名为VangoCalibration的注册表项。如果已经存在则不影响！
            RegistryKey software = key.CreateSubKey("software\\Ls3dsoftkey");
            string l = software.GetValue("YGMXZ", "0").ToString();            
            key.Close();
            return l;
        }



    }
}
