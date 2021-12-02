using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
namespace RCIS.Utility
{
    public class Utilities
    {

        /// 将一个object对象序列化，返回一个byte[]
        /// </summary>
        /// <param name="obj">能序列化的对象</param>
        /// <returns></returns>
        public static byte[] ObjectToBytes(object obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                return ms.GetBuffer();
            }
        }

        /**/
        /// <summary>
        /// 将一个序列化后的byte[]数组还原
        /// </summary>
        /// <param name="Bytes"></param>
        /// <returns></returns>
        public static object BytesToObject(byte[] Bytes)
        {
            using (MemoryStream ms = new MemoryStream(Bytes))
            {
                IFormatter formatter = new BinaryFormatter();
                return formatter.Deserialize(ms);
            }
        }


        public static int Precent(double minValue, double maxValue, double curValue)
        {
            double diff = maxValue - minValue;
            diff = Math.Abs(diff);
            if (diff == 0.0) return 0;
            double result = curValue / diff;
            int intResult = (int)(result * 100);
            return intResult;
        }
    }
}
