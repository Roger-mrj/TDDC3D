using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace RCIS.GISCommon
{
    public class LogHelper
    {
        /// <summary>
        /// 向系统运行目录下写入日志
        /// </summary>
        /// <param name="txtlog">要写入的内容</param>
        public static void WriteLog(string txtlog)
        {
            try
            {
                txtlog = "\r\n" + DateTime.Now + "|" + txtlog;
                string logDirPath = AppDomain.CurrentDomain.BaseDirectory + "\\日志";
                if (!Directory.Exists(logDirPath))
                {
                    Directory.CreateDirectory(logDirPath);
                }
                string fileName = DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
                string logPath = logDirPath + "\\" + fileName;
                if (!File.Exists(logPath))
                {
                    File.Create(logPath);
                }
                using (FileStream fs = File.Open(logPath, FileMode.Append, FileAccess.Write, FileShare.Write))
                {
                    //将字符串转成byte数组
                    byte[] byteFile = Encoding.UTF8.GetBytes(txtlog);
                    //参数：要写入到文件的数据数组，从数组的第几个开始写，一共写多少个字节
                    fs.Write(byteFile, 0, byteFile.Length);
                }
            }
            catch (Exception ex)
            { }
        }
    }
}
