using System.Text;
using System.Runtime.InteropServices;

namespace RCIS.Utility
{
    /// <summary>
    /// �ԣɣΣɵĶ�д����
    /// </summary>
    public class INIHelper
    {
        //�ļ�INI���� 
        public string Path;

        ////������дINI�ļ���API���� 
        [DllImport("kernel32")]

        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);


        [DllImport("kernel32")]

        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);


        //��Ĺ��캯��������INI�ļ��� 
        public INIHelper(string inipath)
        {

            // 
            // TODO: Add constructor logic here 
            // 
            Path = inipath;
        }

        //дINI�ļ� 
        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.Path);

        }

        //��ȡINI�ļ�ָ�� 
        public string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, this.Path);
            return temp.ToString();

        }


    }
}
