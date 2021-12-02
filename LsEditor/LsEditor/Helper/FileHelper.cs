
using System;
using System.IO;
using System.Drawing ;
using System.Windows .Forms ;
using System.Runtime .InteropServices ;
using System.Diagnostics;


namespace RCIS
{
    /// <summary>
    /// FileHelper 的摘要说明。
    /// 把文件都提到可执行程序的上一级目录的下面去
    /// 这样Debug和Release可以使用同一个配置文件。
    /// </summary>
    public class FileHelper
    {
        //#region 导入DLL
        //[DllImport("shell32.dll", EntryPoint = "ShellExecute")]
        //public static extern int ShellExecute(
        //int hwnd,
        //string lpOperation,
        //string lpFile,
        //string lpParameters,
        //string lpDirectory,
        //int nShowCmd
        //);
        //#endregion
        public FileHelper()
        {

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
                FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录
                foreach (FileSystemInfo i in fileinfo)
                {
                    if (i is DirectoryInfo)            //判断是否文件夹
                    {
                        DirectoryInfo subdir = new DirectoryInfo(i.FullName);
                        subdir.Delete(true);          //删除子目录和文件
                    }
                    else
                    {
                        File.Delete(i.FullName);      //删除指定文件
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }


        /// <summary>
        /// 文件合并
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

        public static  string  FindFolder(string spath)
        {
            return Application.StartupPath + "\\"+spath;
        }

        public static Image LoadImage(string pImageName)
        {
            Image rImg = null;
            string file  = Application.StartupPath + "\\img\\"+pImageName;
            if (File.Exists(file))
            {
                try
                {
                    rImg = Image.FromFile(file);
                }
                catch { return null; }
            }

            return rImg;
        }

      
    }
}
