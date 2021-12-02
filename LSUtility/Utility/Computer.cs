using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management; 
namespace RCIS.Utility
{
    public class Computer
    {
        public string CpuID; //1.cpu序列号
        public string MacAddress; //2.mac序列号
        public string DiskID; //3.硬盘id
        public string IpAddress; //4.ip地址

        public Computer()
        {
            CpuID = GetCpuID();
            MacAddress = GetMacAddress();
            DiskID = GetDiskID();
            IpAddress = GetIPAddress();

        }
        //1.获取CPU序列号代码 

        string GetCpuID()
        {
            try
            {
                string cpuInfo = "";//cpu序列号 
                ManagementClass mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    cpuInfo = mo.Properties["ProcessorId"].Value.ToString();
                }
                moc = null;
                mc = null;
                return cpuInfo;
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }

        }

        //2.获取网卡硬件地址 

        string GetMacAddress()
        {
            try
            {
                string mac = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        mac = mo["MacAddress"].ToString();
                        break;
                    }
                }
                moc = null;
                mc = null;
                return mac;
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }

        }

        //3.获取硬盘ID 
        string GetDiskID()
        {
            try
            {
                String HDid = "";
                ManagementClass mc = new ManagementClass("Win32_DiskDrive");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    HDid = (string)mo.Properties["Model"].Value;
                }
                moc = null;
                mc = null;
                return HDid;
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }

        }


        //4.获取IP地址 

        string GetIPAddress()
        {
            try
            {
                string st = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        //st=mo["IpAddress"].ToString(); 
                        System.Array ar;
                        ar = (System.Array)(mo.Properties["IpAddress"].Value);
                        st = ar.GetValue(0).ToString();
                        break;
                    }
                }
                moc = null;
                mc = null;
                return st;
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }

        }


    }
}
