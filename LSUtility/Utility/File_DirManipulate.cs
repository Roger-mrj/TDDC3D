using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace RCIS.Utility
{
    /// <summary>
    /// 文件及文件夹复制，移动
    /// </summary>
    public class File_DirManipulate
    {
        /// <summary>
        /// FileCopy
        /// </summary>
        /// <param name="srcFilePath">源路径</param>
        /// <param name="destFilePath">目标路径</param>
        public static void FileCopy(string srcFilePath, string destFilePath)
        {
            File.Copy(srcFilePath, destFilePath);
        }
        /// <summary>
        /// FileMove
        /// </summary>
        /// <param name="srcFilePath">源路径</param>
        /// <param name="destFilePath">目标路径</param>
        public static void FileMove(string srcFilePath, string destFilePath)
        {
            File.Move(srcFilePath, destFilePath);
        }
        /// <summary>
        /// FileDelete
        /// </summary>
        /// <param name="delFilePath"></param>
        public static void FileDelete(string delFilePath)
        {
            File.Delete(delFilePath);
        }
        /// <summary>
        /// 删除文件夹及文件夹中的内容
        /// </summary>
        /// <param name="delFolderPath"></param>
        public static void FolderDelete(string delFolderPath)
        {
            if (delFolderPath[delFolderPath.Length - 1] != Path.DirectorySeparatorChar)
                delFolderPath += Path.DirectorySeparatorChar;
            //string[] fileList = Directory.GetFileSystemEntries(delFolderPath);

            foreach (string item in Directory.GetFileSystemEntries(delFolderPath))
            {
                if (File.Exists(item))
                {
                    FileInfo fi = new FileInfo(item);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)//改变只读文件属性，否则删不掉
                        fi.Attributes = FileAttributes.Normal;
                    File.Delete(item);
                }//删除其中的文件
                else
                    FolderDelete(item);//递归删除子文件夹
            }
            Directory.Delete(delFolderPath);//删除已空文件夹

        }
        /// <summary>
        /// 文件夹拷贝
        /// </summary>
        /// <param name="srcFolderPath"></param>
        /// <param name="destFolderPath"></param>
        public static void FolderCopy(string srcFolderPath, string destFolderPath)
        {
            //检查目标目录是否以目标分隔符结束，如果不是则添加之
            if (destFolderPath[destFolderPath.Length - 1] != Path.DirectorySeparatorChar)
                destFolderPath += Path.DirectorySeparatorChar;
            //判断目标目录是否存在，如果不在则创建之
            if (!Directory.Exists(destFolderPath))
                Directory.CreateDirectory(destFolderPath);
            string[] fileList = Directory.GetFileSystemEntries(srcFolderPath);
            foreach (string file in fileList)
            {
                if (Directory.Exists(file))
                    FolderCopy(file, destFolderPath + Path.GetFileName(file));
                else
                {
                    FileInfo fi = new FileInfo(file);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)//改变只读文件属性，否则删不掉
                        fi.Attributes = FileAttributes.Normal;
                    try
                    { File.Copy(file, destFolderPath + Path.GetFileName(file), true); }
                    catch (Exception e)
                    {

                    }
                }

            }
        }
        /// <summary>
        /// 文件夹移动
        /// </summary>
        /// <param name="srcFolderPath"></param>
        /// <param name="destFolderPath"></param>
        public static void FolderMove(string srcFolderPath, string destFolderPath)
        {
            //检查目标目录是否以目标分隔符结束，如果不是则添加之
            if (destFolderPath[destFolderPath.Length - 1] != Path.DirectorySeparatorChar)
                destFolderPath += Path.DirectorySeparatorChar;
            //判断目标目录是否存在，如果不在则创建之
            if (!Directory.Exists(destFolderPath))
                Directory.CreateDirectory(destFolderPath);
            string[] fileList = Directory.GetFileSystemEntries(srcFolderPath);
            foreach (string file in fileList)
            {
                if (Directory.Exists(file))
                {
                    FolderMove(file, destFolderPath + Path.GetFileName(file));
                    //Directory.Delete(file);
                }
                else
                    File.Move(file, destFolderPath + Path.GetFileName(file));
            }
            Directory.Delete(srcFolderPath);
        } 
    }
}
