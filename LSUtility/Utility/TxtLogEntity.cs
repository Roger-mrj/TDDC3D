using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
namespace RCIS.Utility
{
    public class TxtLogEntity
    {
        private string logFile;  
        private StreamWriter writer;  
        private FileStream fileStream = null;


        private int errCount = 0;
        /// <summary>
        /// 错误数量
        /// </summary>
        public int ErrCount
        {
            get { return errCount; }
            set { errCount = value; }
        }

        public TxtLogEntity(string fileName)  
        {  
            logFile = fileName;  
            CreateDirectory(logFile);  

            
        }  
  
        public void log(string info)  
        {  
  
            try  
            {  
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(logFile);  
                if (!fileInfo.Exists)  
                {  
                    fileStream = fileInfo.Create();  
                    writer = new StreamWriter(fileStream);  
                }  
                else  
                {  
                    fileStream = fileInfo.Open(FileMode.Append, FileAccess.Write);  
                    writer = new StreamWriter(fileStream);  
                }  
                writer.WriteLine(DateTime.Now + ": " + info);
                errCount++;
  
            }  
            finally  
            {  
                if (writer != null)  
                {  
                    writer.Close();  
                    writer.Dispose();  
                    fileStream.Close();  
                    fileStream.Dispose();  
                }  
            }  
        }  
  
        public void CreateDirectory(string infoPath)  
        {  
            DirectoryInfo directoryInfo = Directory.GetParent(infoPath);  
            if (!directoryInfo.Exists)  
            {  
                directoryInfo.Create();  
            }  
        }  
    }
}
