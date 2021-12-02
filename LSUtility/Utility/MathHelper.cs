using System;

namespace RCIS.Utility
{
    public class MathHelper
    {
        /// <summary>
        /// 数字经纬度和度分秒经纬度转换 (Digital degree of latitude and longitude and vehicle to latitude and longitude conversion)
        /// </summary>
        /// <param name="digitalDegree">数字经纬度</param>
        /// <return>度分秒经纬度</return>
        public static string ConvertDigitalToDegrees(double digitalDegree, int len)
        {
            const double num = 60;
            int degree = (int)digitalDegree;
            double tmp = (digitalDegree - degree) * num;
            int minute = (int)tmp;
            double second = (tmp - minute) * num;
            string degrees = "" + degree + "°" + minute + "′" + Math.Round(second, len) + "″";
            return degrees;
        }
        /// <summary>
        /// 数字经纬度和度分秒经纬度转换 (Digital degree of latitude and longitude and vehicle to latitude and longitude conversion)
        /// </summary>
        /// <param name="digitalDegree">数字经纬度</param>
        /// <return>度分秒经纬度</return>
        public static string ConvertDigitalToDegrees(double digitalDegree)
        {
            const double num = 60;
            int degree = (int)digitalDegree;
            double tmp = (digitalDegree - degree) * num;
            int minute = (int)tmp;
            double second = (tmp - minute) * num;
            string degrees = "" + degree + "°" + minute.ToString().PadLeft(2, '0') + "′" + ((int)second).ToString().PadLeft(2, '0') + "″";
            return degrees;
        }
        /// <summary>
        /// 度分秒经纬度(必须含有'°')和数字经纬度转换
        /// </summary>
        /// <param name="digitalDegree">度分秒经纬度</param>
        /// <return>数字经纬度</return>
        public static double ConvertDegreesToDigital(string degrees)
        {
            const double num = 60;
            double digitalDegree = 0.0;
            int d = degrees.IndexOf('°');           //度的符号对应的 Unicode 代码为：00B0[1]（六十进制），显示为°。
            if (d < 0)
            {
                return digitalDegree;
            }
            string degree = degrees.Substring(0, d);
            digitalDegree += Convert.ToDouble(degree);

            int m = degrees.IndexOf('′');           //分的符号对应的 Unicode 代码为：2032[1]（六十进制），显示为′。
            if (m < 0)
            {
                return digitalDegree;
            }
            string minute = degrees.Substring(d + 1, m - d - 1);
            digitalDegree += ((Convert.ToDouble(minute)) / num);

            int s = degrees.IndexOf('″');           //秒的符号对应的 Unicode 代码为：2033[1]（六十进制），显示为″。
            if (s < 0)
            {
                return digitalDegree;
            }
            string second = degrees.Substring(m + 1, s - m - 1);
            digitalDegree += (Convert.ToDouble(second) / (num * num));

            return digitalDegree;
        }
        /// <summary>
        /// 判断是否正整数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNumeric(string str)
        {
            System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(@"^[0-9]\d*$");
            return reg1.IsMatch(str);
        } 
        /// <summary>
        /// 是否是偶数
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static bool IsOdd(int n)
        {
            return Convert.ToBoolean(n % 2);
        } 

        /// <summary>
        /// 保留位数[Format]方法
        /// </summary>
        /// <param name="ss">输入数字字符串</param>
        /// <param name="n">需要保留小数位数</param>
        /// <returns></returns>
        public static double Round(string ss, int n)
        {
            double s = ConvertHelper.ObjectToDouble(ss);
            return Round(s, n);
        }
        /// <summary>
        /// 保留位数[Format]方法
        /// </summary>
        /// <param name="ss">输入数字</param>
        /// <param name="n">需要保留小数位数</param>
        /// <returns></returns>
        public static double Round(double s, int n)
        {
            decimal d = decimal.Round((decimal)s, n, MidpointRounding.AwayFromZero);
            return (double)d;
        }

        /// <summary>
        ///     //获得百分比
        /// </summary>
        /// <param name="minValue">最小值</param>
        /// <param name="maxValue">最大值</param>
        /// <param name="curValue">递增值</param>
        /// <returns></returns>
        public static int Precent(double minValue, double maxValue, double curValue)
        {
            double diff = maxValue - minValue;
            diff = Math.Abs(diff);
            if (diff == 0.0) return 0;
            double result = curValue / diff;
            int intResult = (int)(result * 100);
            return intResult;
        }
        public static double GetMJC(double a)
        {
            double a1 = Round(a / 2, 2);
            double a3 = Round(a1 + a1, 2);
            return a3 - a;
        }


        public static double Hailun(double pa, double pb, double pc)
        {
            double circle = (pa + pb + pc) / 2;
            double area = circle * (circle - pa) * (circle - pb)
            * (circle - pc);
            if (area < 0)
                return double.NaN;
            area = Math.Sqrt(area);
            return area;
        }


        /// <summary>
        /// 实现数据的四舍五入法
        /// </summary>
        /// <param name="v">要进行处理的数据</param>
        /// <param name="x">保留的小数位数</param>
        /// <returns>四舍五入后的结果</returns>
        public static double RoundEx(double v, int x)
        {
            bool isNegative = false;
            //如果是负数
            if (v < 0)
            {
                isNegative = true;
                v = -v;
            }

            int IValue = 1;
            for (int i = 1; i <= x; i++)
            {
                IValue = IValue * 10;
            }
            double big = v * IValue;
            long lbig = (long)(v * IValue);

            double ws = big - lbig;
            double Int = 0;
            if (ws < 0.5)
                Int = Math.Round(v * IValue, 0);
            else
                Int = Math.Round(v * IValue + 0.5, 0);


            v = Int / IValue;

            if (isNegative)
            {
                v = -v;
            }

            return v;
        }

    }
}
