using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Management;
using System.Security.Cryptography;

namespace RCIS.Utility
{
    /// <summary>
    /// �����ļ�·������ȡ�ļ�������غ���
    /// </summary>
    public class FileHelper
    {

        public static string ReadLicense(string licensefile, string key)
        {
            if (File.Exists(licensefile))
            {
                string data = ReadFile(licensefile);
                if (data.Split('=').Length == 3)
                {
                    string info = PCHelper.GetCpuID() + "|" + PCHelper.GetDiskID();
                    if (EncryptHelper.MD5(info) == data.Split('=')[0])
                    {
                        Int64 start = Int64.Parse(EncryptHelper.Decrypt(data.Split('=')[1].Replace("\r", "").Replace("\n", ""), key));
                        Int64 end = Int64.Parse(EncryptHelper.Decrypt(data.Split('=')[2].Replace("\r", "").Replace("\n", ""), key));
                        Int64 now = Convert.ToInt64(DateTime.UtcNow.Subtract(DateTime.Parse("1970-01-01 00:00:00")).TotalSeconds);
                        if (now > start && now < end)
                        {
                            WriteLicense(licensefile, data.Split('=')[0], now, end, key);
                            return "OK";
                        }
                        else
                        {
                            return "��ɹ��ڡ�";
                        }
                    }
                    else
                    {
                        return "��ɲ���ȷ��";
                    }
                }
                else
                {
                    return "����ļ���ʽ����ȷ��";
                }
            }
            else
            {
                return "����ļ������ڡ�";
            }
        }
        public static string ReadFile(string file)
        {
            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
                {
                    string s = sr.ReadToEnd();
                    sr.Close();
                    fs.Close();
                    return s;
                }
            }
        }
        public static void WriteFile(string file, string data)
        {
            using (FileStream fs = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    sw.WriteLine(data);
                    sw.Close();
                    fs.Close();
                }
            }
        }
        public static void WriteLicense(string licensefile,string info, Int64 start, Int64 end, string key)
        {
            //string info = PCHelper.GetCpuID() + "|" + PCHelper.GetDiskID();
            WriteFile(licensefile, info + "=" + EncryptHelper.Encryption(start.ToString(), key) + "=" + EncryptHelper.Encryption(end.ToString(), key));
        }
        public static String GetFileFolder(String filePath)
        {
            int index = filePath.LastIndexOf("\\");
            if (index >= 0) return filePath.Substring(0, index);
            else return filePath;
        }
        public static String GetFileTitle(String filePath)
        {
            string result = filePath;
            int index = filePath.LastIndexOf("\\");
            if (index >= 0) result = filePath.Substring(index + 1);
            index = result.LastIndexOf(".");
            if (index >= 0) result = result.Substring(0, index);
            return result;
        }
        public static String GetFileName(String filePath)
        {
            string result = filePath;
            int index = filePath.LastIndexOf("\\");
            if (index >= 0) result = filePath.Substring(index + 1);
            return result;
        }
        public static String GetFileExtentName(String filePath)
        {
            int index = filePath.LastIndexOf(".");
            if (index >= 0)
                return filePath.Substring(index + 1);
            return "";
        }

        public static void DelectDir(string srcPath)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(srcPath);
                FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //����Ŀ¼�������ļ�����Ŀ¼
                foreach (FileSystemInfo i in fileinfo)
                {
                    if (i is DirectoryInfo)            //�ж��Ƿ��ļ���
                    {
                        DirectoryInfo subdir = new DirectoryInfo(i.FullName);
                        subdir.Delete(true);          //ɾ����Ŀ¼���ļ�
                    }
                    else
                    {
                        File.Delete(i.FullName);      //ɾ��ָ���ļ�
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

      
        /// <summary>
        /// �ļ��ϲ�
        /// </summary>
        /// <param name="outputFile"></param>
        /// <param name="inputFiles"></param>
        public static void ConcatenateFiles(string outputFile, string[] inputFiles)
        {
            using (Stream output = File.OpenWrite(outputFile))
            {
                foreach (string inputFile in inputFiles)
                {
                    using (Stream input = File.OpenRead(inputFile))
                    {
                        input.CopyTo(output);
                    }
                }
            }
        }
    }
    public class PCHelper
    {
        /// <summary>
        /// CPU���к�
        /// </summary>
        /// <returns></returns>
        public static string GetCpuID()
        {
            try
            {
                //��ȡCPU���кŴ���  
                string cpuInfo = "";//cpu���к�  
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
        }
        /// <summary>
        /// Ӳ��ID,��С
        /// </summary>
        /// <returns></returns>
        public static string GetDiskID()
        {
            string dic = string.Empty;
            ManagementClass mc = new ManagementClass("Win32_DiskDrive");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                string HDid = (string)mo.Properties["Model"].Value;
                dic += HDid + "|";
            }
            moc = null;
            mc = null;
            return dic.Substring(0, dic.Length - 1);
        }
    }
    public class EncryptHelper
    {
        //����
        public static string Encryption(string data, string key = "@Mojo.c1")
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.Default.GetBytes(data);
            des.Key = ASCIIEncoding.ASCII.GetBytes(key);
            des.IV = ASCIIEncoding.ASCII.GetBytes(key);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            ret.ToString();
            return ret.ToString();
        }

        //����
        public static string Decrypt(string data, string key = "@Mojo.c1")
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = new byte[data.Length / 2];
            for (int x = 0; x < data.Length / 2; x++)
            {
                int i = (Convert.ToInt32(data.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }
            des.Key = ASCIIEncoding.ASCII.GetBytes(key);
            des.IV = ASCIIEncoding.ASCII.GetBytes(key);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            return System.Text.Encoding.Default.GetString(ms.ToArray());
        }
        public static string MD5(string str)
        {
            string cl = str;
            string pwd = "";
            MD5 md5 = System.Security.Cryptography.MD5.Create();//ʵ����һ��md5����
            // ���ܺ���һ���ֽ����͵����飬����Ҫע�����UTF8/Unicode�ȵ�ѡ��
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            // ͨ��ʹ��ѭ�������ֽ����͵�����ת��Ϊ�ַ��������ַ����ǳ����ַ���ʽ������
            for (int i = 0; i < s.Length; i++)
            {
                // ���õ����ַ���ʹ��ʮ���������͸�ʽ����ʽ����ַ���Сд����ĸ�����ʹ�ô�д��X�����ʽ����ַ��Ǵ�д�ַ� 
                pwd = pwd + s[i].ToString("X2");
            }
            return pwd;
        }

        public static string SHA1(string strIN, Boolean isReturnNum = true)
        {
            //string strIN = getstrIN(strIN);
            byte[] tmpByte;
            SHA1 sha1 = new SHA1CryptoServiceProvider();

            tmpByte = sha1.ComputeHash(GetKeyByteArray(strIN));
            sha1.Clear();

            return GetStringValue(tmpByte, isReturnNum);

        }

        public static string SHA256(string strIN, Boolean isReturnNum = true)
        {
            //string strIN = getstrIN(strIN);
            byte[] tmpByte;
            SHA256 sha256 = new SHA256Managed();

            tmpByte = sha256.ComputeHash(GetKeyByteArray(strIN));
            sha256.Clear();

            return GetStringValue(tmpByte, isReturnNum);

        }

        public static string SHA512(string strIN, Boolean isReturnNum = true)
        {
            //string strIN = getstrIN(strIN);
            byte[] tmpByte;
            SHA512 sha512 = new SHA512Managed();

            tmpByte = sha512.ComputeHash(GetKeyByteArray(strIN));
            sha512.Clear();

            return GetStringValue(tmpByte, isReturnNum);

        }
        private static string GetStringValue(byte[] Byte, Boolean isReturnNum)
        {
            string tmpString = "";
            if (isReturnNum == false)
            {
                UTF8Encoding Asc = new UTF8Encoding();
                tmpString = Asc.GetString(Byte);
            }
            else
            {
                int iCounter;
                for (iCounter = 0; iCounter < Byte.Length; iCounter++)
                {
                    tmpString = tmpString + Byte[iCounter].ToString();
                }
            }
            return tmpString;
        }

        private static byte[] GetKeyByteArray(string strKey)
        {

            UTF8Encoding Asc = new UTF8Encoding();

            int tmpStrLen = strKey.Length;
            byte[] tmpByte = new byte[tmpStrLen - 1];

            tmpByte = Asc.GetBytes(strKey);

            return tmpByte;

        }
    }
}
