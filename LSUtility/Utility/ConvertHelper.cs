using System;

namespace RCIS.Utility
{
    public class ConvertHelper
    {
        /// <summary>
        /// 转换为整型
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static int ObjectToInt(string str)
        {
            try
            {
                if (str == null) return 0;
                if (str == "") return 0;
                int i = 0;
                Int32.TryParse(str, out i);
                return i;
            }
            catch { return 0; }
        }
        /// <summary>
        /// 转换为整型
        /// </summary>
        /// <param name="str">Object</param>
        /// <returns></returns>
        public static int ObjectToInt(object str)
        {
            try
            {
                if (str == null) return 0;
                int i = 0;
                Int32.TryParse(str.ToString(), out i);
                return i;
            }
            catch { return 0; }
        }
        /// <summary>
        /// 转换为双精度浮点型
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static double ObjectToDouble(string str)
        {
            try
            {
                if ((str == "") || (str == null))
                {
                    return 0.0;
                }

                double i = 0.00;
                double.TryParse(str, out i);
                return i;
            }
            catch { return 0.0; }
        }
        /// <summary>
        /// 转换为双精度浮点型
        /// </summary>
        /// <param name="str">Object</param>
        /// <returns></returns>
        public static double ObjectToDouble(object str)
        {
            try
            {
                if (str == null)
                {
                    return 0.0;
                }

                double i = 0.00;
                double.TryParse(str.ToString(), out i);
                return i;
            }
            catch { return 0.0; }
        }
        /// <summary>
        /// 转换为单精度浮点型
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static double ObjectToFloat(string str)
        {
            try
            {
                if ((str == "") || (str == null))
                {
                    return 0.0;
                }

                float i = 0.0f;
                float.TryParse(str, out i);
                return i;
            }
            catch { return 0.0; }
        }
        /// <summary>
        /// 转换为单精度浮点型
        /// </summary>
        /// <param name="str">Object</param>
        /// <returns></returns>
        public static double ObjectToFloat(object str)
        {
            try
            {
                if (str == null)
                {
                    return 0.0;
                }

                float i = 0.0f;
                float.TryParse(str.ToString(), out i);
                return i;
            }
            catch { return 0.0; }
        }
        /// <summary>
        /// 转换为字符串
        /// </summary>
        /// <param name="str">Object</param>
        /// <returns></returns>
        public static string ObjectToString(object str)
        {
            try
            {
                if (str == null)
                {
                    return "";
                }
                else
                {
                    return Convert.ToString(str);
                }
            }
            catch { return ""; }

        }
    }
}
