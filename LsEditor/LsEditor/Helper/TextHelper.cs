using System;
using System.Globalization;
using System.Text ;
using System.Text .RegularExpressions ;
namespace RCIS
{
    /// <summary>
    /// TextHelper 提供和字符串有关的一些通用功能的支持
    /// </summary>
    public class TextHelper
    {
        /// <summary>
        /// 将字符串按照小数点个数对齐
        /// </summary>
        /// <param name="pStr"></param>
        /// <param name="pLen"></param>
        /// <returns></returns>
        public static string AlignDouble(string pStr, int pLen)
        {//将字符当作Double类型的数据对齐小数位.
            if (pStr == null || pLen <= 0)
                return "";
            int index = pStr.LastIndexOf(".");
            if (index < 0) pStr += ".0";
            index = pStr.LastIndexOf(".");
            if (index >= 0)
            {
                int curLen = pStr.Length - index - 1;
                int diff = pLen - curLen;
                if (diff > 0)
                {
                    pStr = pStr.PadRight(pStr.Length + diff, '0');
                }
                else
                {
                    pStr = pStr.Substring(0, pStr.Length + diff);
                }
            }
            return pStr;
        }
        /// <summary>
        /// Compress将多个连续空格压缩成一个
        /// </summary>
        /// <param name="aLine"></param>
        /// <returns></returns>
        public static String Compress(String aLine)
        {
            aLine = aLine.Trim();
            StringBuilder fromBuilder = new StringBuilder(aLine);
            StringBuilder toBuilder = new StringBuilder();
            bool hasSpace = false;
            int cCount = fromBuilder.Length;
            for (int ci = 0; ci < cCount; ci++)
            {
                char aChar = fromBuilder[ci];
                if (aChar.Equals(' ')
                    || aChar.Equals('\t'))
                {
                    hasSpace = true;
                }
                else
                {
                    if (hasSpace)
                    {
                        toBuilder.Append(" ").Append(aChar);
                        hasSpace = false;
                    }
                    else
                    {
                        toBuilder.Append(aChar);
                    }
                }
            }
            String rLine = toBuilder.ToString();
            return rLine;
        }
        /// <summary>
        /// Compact将空格都去掉
        /// </summary>
        /// <param name="aLine"></param>
        /// <returns></returns>
        public static String Compact(String aLine)
        {
            aLine = aLine.Trim();
            StringBuilder fromBuilder = new StringBuilder(aLine);
            StringBuilder toBuilder = new StringBuilder();
            int cCount = fromBuilder.Length;
            for (int ci = 0; ci < cCount; ci++)
            {
                char aChar = fromBuilder[ci];
                if (aChar.Equals(' ')
                    || aChar.Equals('\t'))
                {
                    //什么也不做
                }
                else
                {
                    toBuilder.Append(aChar);
                }
            }
            String rLine = toBuilder.ToString();
            return rLine;
        }
        public static int ParseInt(String aLine, int pDefInt)
        {
            int result = pDefInt;
            try
            {
                if (aLine != null
                    && !aLine.Equals("")
                    && !aLine.Trim().Equals(""))
                {
                    aLine.TrimStart("0".ToCharArray());
                    if (aLine.Equals(""))
                    {
                        result = 0;
                    }
                    else
                    {
                        result = Convert.ToInt32(aLine);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return result;
        }
        /// <summary>
        /// 将字符串转化为数字
        /// </summary>
        /// <param name="pData"></param>
        /// <param name="pResult"></param>
        /// <returns></returns>
        public static bool TryParse(string pData, out double pResult)
        {
            bool result = false;
            pResult = Double.NaN;
            try
            {
                result = Double.TryParse(pData, NumberStyles.Any,
                    new NumberFormatInfo(), out pResult);
            }
            catch (Exception ex)
            {
            }
            return result;
        }
        public static double ParseDouble(string aLine, double pDefDouble)
        {
            double result = pDefDouble;
            if (!TryParse(aLine, out result))
            {
                result = pDefDouble;
            }
            return result;
        }
        public static string PadLeft(object aLine, char aPad, int aWidth)
        {
            if (aLine == null)
                return PadLeft(null, aPad, aWidth);
            else return PadLeft(aLine.ToString(), aPad, aWidth);
        }
        public static string PadLeft(string aLine, char aPad, int aWidth)
        {
            if (aLine == null)
                aLine = "";
            if (aLine.Length > aWidth)
                aLine = aLine.Substring(aLine.Length - aWidth);
            aLine = aLine.PadLeft(aWidth, aPad);
            return aLine;
        }
        public static string Replace(string aLine
            , int pStartIndex, int pCount, string pNewStr)
        {
            if (aLine == null) aLine = "";
            if (pNewStr == null) pNewStr = "";
            int aLen = pStartIndex + pCount;
            if (aLine.Length < aLen)
                aLine = aLine.PadLeft(aLen, '0');
            aLine = aLine.Remove(pStartIndex, pCount);
            aLine = aLine.Insert(pStartIndex, pNewStr);
            return aLine;
        }
        public static string ToString(char[] pCharAry)
        {
            string rStr = "";
            StringBuilder aBuilder = new StringBuilder();
            if (pCharAry != null)
            {
                int aCount = pCharAry.Length;
                for (int i = 0; i < aCount; i++)
                {
                    char aChar = pCharAry[i];
                    if (aChar.Equals('\0'))
                    {
                        break;
                    }
                    else
                    {
                        aBuilder.Append(aChar);
                    }
                }
                rStr = aBuilder.ToString();
            }
            return rStr;
        }
        public static string ToString(byte[] pByteAry)
        {
            string rStr = "";
            StringBuilder aBuilder = new StringBuilder();
            if (pByteAry != null)
            {
                int aCount = pByteAry.Length;
                for (int i = 0; i < aCount; i++)
                {
                    char aChar = (char)(pByteAry[i]);
                    if (aChar.Equals('\0'))
                    {
                        break;
                    }
                    else
                    {
                        aBuilder.Append(aChar);
                    }
                }
                rStr = aBuilder.ToString();
            }
            return rStr;
        }
        public static string FormatDate(DateTime pTime, string pLink)
        {
            if (pLink == null) pLink = "";
            string year = pTime.Year.ToString();
            string month = pTime.Month.ToString().PadLeft(2, '0');
            string day = pTime.Day.ToString().PadLeft(2, '0');
            return year + pLink + month + pLink + day;
        }
        public static string FormatTime(DateTime pTime, string pLink)
        {
            if (pLink == null) pLink = "";
            string hour = pTime.Hour.ToString().PadLeft(2, '0');
            string minute = pTime.Minute.ToString().PadLeft(2, '0');
            string second = pTime.Second.ToString().PadLeft(2, '0');
            return hour + pLink + minute + pLink + second;
        }
        public static string FormatDateTime(DateTime pTime, string pLink)
        {
            if (pLink == null) pLink = "";
            string year = pTime.Year.ToString();
            string month = pTime.Month.ToString().PadLeft(2, '0');
            string day = pTime.Day.ToString().PadLeft(2, '0');
            string hour = pTime.Hour.ToString().PadLeft(2, '0');
            string minute = pTime.Minute.ToString().PadLeft(2, '0');
            string second = pTime.Second.ToString().PadLeft(2, '0');
            return year + pLink + month + pLink + day + pLink + hour + pLink + minute + pLink + second;
        }

        /// <summary>
        /// 给定一个字符串，判断其是否只包含有汉字
        /// </summary>
        /// <param name="testStr"></param>
        /// <returns></returns>
        public bool IsOnlyContainsChinese(string testStr)
        {
            char[] words = testStr.ToCharArray();
            foreach (char word in words)
            {
                if (IsGBCode(word.ToString()) || IsGBKCode(word.ToString()))  // it is a GB2312 or GBK chinese word
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 判断一个word是否为GB2312编码的汉字
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        private bool IsGBCode(string word)
        {
            byte[] bytes = Encoding.GetEncoding("GB2312").GetBytes(word);
            if (bytes.Length <= 1)  // if there is only one byte, it is ASCII code or other code
            {
                return false;
            }
            else
            {
                byte byte1 = bytes[0];
                byte byte2 = bytes[1];
                if (byte1 >= 176 && byte1 <= 247 && byte2 >= 160 && byte2 <= 254)    //判断是否是GB2312
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// 判断一个word是否为GBK编码的汉字
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        private bool IsGBKCode(string word)
        {
            byte[] bytes = Encoding.GetEncoding("GBK").GetBytes(word.ToString());
            if (bytes.Length <= 1)  // if there is only one byte, it is ASCII code
            {
                return false;
            }
            else
            {
                byte byte1 = bytes[0];
                byte byte2 = bytes[1];
                if (byte1 >= 129 && byte1 <= 254 && byte2 >= 64 && byte2 <= 254)     //判断是否是GBK编码
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// 判断一个word是否为Big5编码的汉字
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        private bool IsBig5Code(string word)
        {
            byte[] bytes = Encoding.GetEncoding("Big5").GetBytes(word.ToString());
            if (bytes.Length <= 1)  // if there is only one byte, it is ASCII code
            {
                return false;
            }
            else
            {
                byte byte1 = bytes[0];
                byte byte2 = bytes[1];
                if ((byte1 >= 129 && byte1 <= 254) && ((byte2 >= 64 && byte2 <= 126) || (byte2 >= 161 && byte2 <= 254)))     //判断是否是Big5编码
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        #region IsInt32
        /// <summary>
        /// 验证字符串是否整数
        /// </summary>
        /// <param name="input">要验证的字符串</param>
        /// <returns></returns>
        public static bool IsInt32(string input)
        {
            string regexString = @"^-?\\d+$";

            return Regex.IsMatch(input, regexString);
        }
        #endregion

        #region IsDouble
        /// <summary>
        /// 验证字符串是否浮点数字
        /// </summary>
        /// <param name="v">要验证的字符串</param>
        /// <returns></returns>
        public static bool IsDouble(string input)
        {
            string regexString = @"^(-?\\d+)(\\.\\d+)?$";

            return Regex.IsMatch(input, regexString);
        }
        #endregion

        #region IsEmail
        /// <summary>
        /// 验证字符串Email地址
        /// </summary>
        /// <param name="input">要验证的字符串</param>
        /// <returns></returns>
        public static bool IsEmail(string input)
        {
            // Return true if strIn is in valid e-mail format.
            string regexString = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

            return Regex.IsMatch(input, regexString);
        }
        #endregion

        #region IsDate
        /// <summary>
        /// 验证字符串是否日期[2004-2-29|||2004-02-29 10:29:39 pm|||2004/12/31]
        /// </summary>
        /// <param name="v">要验证的字符串</param>
        /// <returns></returns>
        public static bool IsDate(string input)
        {
            string regexString = @"^((\d{2}(([02468][048])|([13579][26]))[\-\/\s]?((((0?[13578])|(1[02]))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[469])|(11))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(30)))|(0?2[\-\/\s]?((0?[1-9])|([1-2][0-9])))))|(\d{2}(([02468][1235679])|([13579][01345789]))[\-\/\s]?((((0?[13578])|(1[02]))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[469])|(11))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(30)))|(0?2[\-\/\s]?((0?[1-9])|(1[0-9])|(2[0-8]))))))(\s(((0?[1-9])|(1[0-2]))\:([0-5][0-9])((\s)|(\:([0-5][0-9])\s))([AM|PM|am|pm]{2,2})))?$";

            return Regex.IsMatch(input, regexString);
            #region Description
            /*
   Expression:  ^((\d{2}(([02468][048])|([13579][26]))[\-\/\s]?((((0?[13578]
       )|(1[02]))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[4
       69])|(11))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(30)))|(0?2[\-\/\
       s]?((0?[1-9])|([1-2][0-9])))))|(\d{2}(([02468][1235679])|([1
       3579][01345789]))[\-\/\s]?((((0?[13578])|(1[02]))[\-\/\s]?((
       0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[469])|(11))[\-\/\s]?((
       0?[1-9])|([1-2][0-9])|(30)))|(0?2[\-\/\s]?((0?[1-9])|(1[0-9]
       )|(2[0-8]))))))(\s(((0?[1-9])|(1[0-2]))\:([0-5][0-9])((\s)|(
       \:([0-5][0-9])\s))([AM|PM|am|pm]{2,2})))?$
 
   Author:  Sung Lee 
   Sample Matches:  
   2004-2-29|||2004-02-29 10:29:39 pm|||2004/12/31 
   Sample Non-Matches:  
   2003-2-29|||2003-13-02|||2003-2-2 10:72:30 am 
   Description:  Matches ANSI SQL date format YYYY-mm-dd hh:mi:ss am/pm. You can use / - or space for date delimiters, so 2004-12-31 works just as well as 2004/12/31. Checks leap year from 1901 to 2099. 
    */
            #endregion
        }
        #endregion

        #region IsAnsiSqlDate
        /// <summary>
        /// 验证字符串是否 ANSI SQL date format
        /// </summary>
        /// <param name="v">要验证的字符串</param>
        /// <returns></returns>
        public static bool IsAnsiSqlDate(string input)
        {
            string regexString = @"^((\d{2}(([02468][048])|([13579][26]))[\-\/\s]?((((0?[13578]
         )|(1[02]))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[4
         69])|(11))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(30)))|(0?2[\-\/\
         s]?((0?[1-9])|([1-2][0-9])))))|(\d{2}(([02468][1235679])|([1
         3579][01345789]))[\-\/\s]?((((0?[13578])|(1[02]))[\-\/\s]?((
         0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[469])|(11))[\-\/\s]?((
         0?[1-9])|([1-2][0-9])|(30)))|(0?2[\-\/\s]?((0?[1-9])|(1[0-9]
         )|(2[0-8]))))))(\s(((0?[1-9])|(1[0-2]))\:([0-5][0-9])((\s)|(
         \:([0-5][0-9])\s))([AM|PM|am|pm]{2,2})))?$";

            return Regex.IsMatch(input, regexString);
            #region Description
            /*
   Expression:  ^((\d{2}(([02468][048])|([13579][26]))[\-\/\s]?((((0?[13578]
       )|(1[02]))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[4
       69])|(11))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(30)))|(0?2[\-\/\
       s]?((0?[1-9])|([1-2][0-9])))))|(\d{2}(([02468][1235679])|([1
       3579][01345789]))[\-\/\s]?((((0?[13578])|(1[02]))[\-\/\s]?((
       0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[469])|(11))[\-\/\s]?((
       0?[1-9])|([1-2][0-9])|(30)))|(0?2[\-\/\s]?((0?[1-9])|(1[0-9]
       )|(2[0-8]))))))(\s(((0?[1-9])|(1[0-2]))\:([0-5][0-9])((\s)|(
       \:([0-5][0-9])\s))([AM|PM|am|pm]{2,2})))?$
        
       Author:  Sung Lee 
       Sample Matches:  
       2004-2-29|||2004-02-29 10:29:39 pm|||2004/12/31 
       Sample Non-Matches:  
       2003-2-29|||2003-13-02|||2003-2-2 10:72:30 am 
       Description:  Matches ANSI SQL date format YYYY-mm-dd hh:mi:ss am/pm. You can use / - or space for date delimiters, so 2004-12-31 works just as well as 2004/12/31. Checks leap year from 1901 to 2099. 

    */
            #endregion
        }
        #endregion

        #region IsTxtFileName
        /// <summary>
        /// 验证字符串是否TXT文件名(全名)
        /// </summary>
        /// <param name="input">要验证的字符串</param>
        /// <returns></returns>
        public static bool IsTxtFileName(string input)
        {
            string regexString = @"^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w ]*))+\.(txt|TXT)$";

            return Regex.IsMatch(input, regexString);
            #region Description
            /*
   
   Expression:  ^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w ]*))+\.(txt|TXT)$
 
   Author:  Michael Ash 
   Sample Matches:  
   c:\file.txt|||c:\folder\sub folder\file.txt|||\\network\folder\file.txt 
   Sample Non-Matches:  
   C:|||C:\file.xls|||folder.txt 
   Description:  This RE validates a path/file of type txt (text file) This RE can be used as a filter on certain file types, while insuring the entire string is a fully qualified path and file. The filter value can be changed or added to as you need 

    */
            #endregion



        }
        #endregion
    }
}
