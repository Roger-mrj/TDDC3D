using System.Text;
using System.Runtime.InteropServices;

namespace RCIS.Utility
{
    /// <summary>
    /// 对ＩＮＩ的读写操作
    /// </summary>
    public class INIHelper
    {
        //文件INI名称 
        public string Path;

        ////声明读写INI文件的API函数 
        [DllImport("kernel32")]

        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);


        [DllImport("kernel32")]

        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);


        //类的构造函数，传递INI文件名 
        public INIHelper(string inipath)
        {

            // 
            // TODO: Add constructor logic here 
            // 
            Path = inipath;
        }

        //写INI文件 
        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.Path);

        }

        //读取INI文件指定 
        public string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, this.Path);
            return temp.ToString();

        }


    }
}
