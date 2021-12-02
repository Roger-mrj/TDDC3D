using System;
using System.Text;
using System.Globalization;
namespace RCIS.Utility
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
        /// GB2312转换成UTF8
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string gb2312_utf8(string text)
        {
            //声明字符集   
            System.Text.Encoding utf8, gb2312;
            //gb2312   
            gb2312 = System.Text.Encoding.GetEncoding("gb2312");
            //utf8   
            utf8 = System.Text.Encoding.GetEncoding("utf-8");
            byte[] gb;
            gb = gb2312.GetBytes(text);
            gb = System.Text.Encoding.Convert(gb2312, utf8, gb);
            //返回转换后的字符   
            return utf8.GetString(gb);
        }

        /// <summary>
        /// UTF8转换成GB2312
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string utf8_gb2312(string text)
        {
            //声明字符集   
            System.Text.Encoding utf8, gb2312;
            //utf8   
            utf8 = System.Text.Encoding.GetEncoding("utf-8");
            //gb2312   
            gb2312 = System.Text.Encoding.GetEncoding("gb2312");
            byte[] utf;
            utf = utf8.GetBytes(text);
            utf = System.Text.Encoding.Convert(utf8, gb2312, utf);
            //返回转换后的字符   
            return gb2312.GetString(utf);
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
    }
}
