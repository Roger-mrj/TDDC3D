using System;
using System.IO;
using System.Windows.Forms;
namespace RCIS.Utility
{
    public class LS_LogHelper
    {
        private static TextBox m_TextBoxLog = null;

        public static string m_LogFileName = "";

        public static string m_LogFileDirectory = Application.StartupPath + "\\Log";

        public static TextBox TextBoxLog
        {
            get
            {
                return LS_LogHelper.m_TextBoxLog;
            }
            set
            {
                LS_LogHelper.m_TextBoxLog = value;
            }
        }

        private static DevExpress.XtraBars.BarStaticItem m_StatusItem = null;

        public static DevExpress.XtraBars.BarStaticItem StatusItem
        {
            get
            {
                return m_StatusItem;
            }
            set
            {
                m_StatusItem = value;
            }
        }

        public static void Logging(string sLog)
        {
            LS_LogHelper.TextBoxLog.AppendText("\r\n[" + DateTime.Now.ToString() + "]" + sLog + "\r\n");
            WriteToLogFile("[" + DateTime.Now.ToString() + "]" + sLog + "");
            Application.DoEvents();
        }

        public static void Logging(string sLog, Exception ex)
        {
            LS_LogHelper.TextBoxLog.AppendText("\r\n[" + DateTime.Now.ToString() + "]" + sLog + "\r\n" + ex.ToString() + "\r\n");
            WriteToLogFile("[" + DateTime.Now.ToString() + "]" + sLog + "\r\n" + ex.ToString() + "");
            Application.DoEvents();
        }

        public static void StatusLog(string sStatus)
        {
            StatusItem.Caption = sStatus;
            Application.DoEvents();
        }

        public static void WriteToLogFile(string log)
        {
            if (!LogFileExist())
            {
                return;
            }
            FileStream fs = new FileStream(m_LogFileDirectory + "\\" + m_LogFileName, FileMode.Append, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            try
            {
                sw.WriteLine(log);
            }
            catch (Exception ex)
            {
            }
            finally
            {
                sw.Close();
                fs.Close();
            }
        }

        private static bool LogFileExist()
        {
            try
            {
                if (!System.IO.File.Exists(m_LogFileDirectory + "\\" + m_LogFileName))
                {
                    if (!System.IO.Directory.Exists(m_LogFileDirectory))
                    {
                        System.IO.Directory.CreateDirectory(m_LogFileDirectory);
                    }
                    FileStream fs = new FileStream(m_LogFileDirectory + "\\" + m_LogFileName,
                        FileMode.Create, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.WriteLine(m_LogFileName);
                    sw.Close();
                    fs.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false; ;
            }
        }
    }
}
