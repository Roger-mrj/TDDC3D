using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace TDDC3D.datado
{
    public class TFHObject
    {
        public static double s_millionDetY = 4.0;
        public static double s_millionDetX = 6.0;

        public static char[] s_HH;
        public static IDictionary<int, String> s_wuWanDict;
        public static IDictionary<String, int> s_wuWanDict2;
        public static char s_sep = '-';
        static TFHObject()
        {
            s_wuWanDict = new Dictionary<int, String>(8);
            s_wuWanDict.Add(1, "A");
            s_wuWanDict.Add(2, "B");
            s_wuWanDict.Add(3, "C");
            s_wuWanDict.Add(4, "D");

            s_wuWanDict.Add(5, "甲");
            s_wuWanDict.Add(6, "乙");
            s_wuWanDict.Add(7, "丙");
            s_wuWanDict.Add(8, "丁");
            s_wuWanDict2 = new Dictionary<String, int>(8);
            s_wuWanDict2.Add("A", 1);
            s_wuWanDict2.Add("B", 2);
            s_wuWanDict2.Add("C", 3);
            s_wuWanDict2.Add("D", 4);
            s_wuWanDict2.Add("甲", 1);
            s_wuWanDict2.Add("乙", 2);
            s_wuWanDict2.Add("丙", 3);
            s_wuWanDict2.Add("丁", 4);

            s_HH = new char[20];
            s_HH[0] = 'X';
            for (int i = 1; i < 20; i++)
            {
                s_HH[i] = (char)(i + 64);

            }
        }
        /// <summary>
        /// 只处图幅号
        /// </summary>
        /// <param name="newTfh"></param>
        /// <returns></returns>
        public static String FromOld2New(String oldTfh)
        {
            String newTfh = "";
            String[] tfhParts = oldTfh.Split(s_sep);
            for (int i = 0; i < tfhParts.Length; i++)
            {
                tfhParts[i] = tfhParts[i].Trim();
            }

            newTfh = tfhParts[0] + tfhParts[1];
            //得到10W编号
            int tenWanNumber = Convert.ToInt32(tfhParts[2]);
            int tenH = FromNumber2PositionH(tenWanNumber, 12);
            int tenL = FromNumber2PositionL(tenWanNumber, 12);
            //check the m_tfh one
            if (Char.IsDigit(tfhParts[3][0]) || tfhParts[3][0] == '(')
            {
                int oneWanNumber = 0;
                if (Char.IsDigit(tfhParts[3][0]))
                {
                    oneWanNumber = Convert.ToInt32(tfhParts[3]);
                }
                else if (tfhParts[3][0] == '(')
                {
                    tfhParts[3] = tfhParts[3].Replace("(", "");
                    tfhParts[3] = tfhParts[3].Replace(")", "");
                    oneWanNumber = Convert.ToInt32(tfhParts[3]);
                }
                int oneH = FromNumber2PositionH(oneWanNumber, 8);
                int oneL = FromNumber2PositionL(oneWanNumber, 8);
                oneH = (tenH - 1) * 8 + oneH;
                oneL = (tenL - 1) * 8 + oneL;
                newTfh += "G";
                newTfh += String.Format("{0:#000}", oneH);
                newTfh += String.Format("{0:#000}", oneL);

            }
            else
            {
                int fiveWanNumber = s_wuWanDict2[tfhParts[3]];
                int fiveH = FromNumber2PositionH(fiveWanNumber, 2);
                int fiveL = FromNumber2PositionL(fiveWanNumber, 2);

                fiveH = (tenH - 1) * 2 + fiveH;
                fiveL = (tenL - 1) * 2 + fiveL;
                newTfh += "E";
                newTfh += String.Format("{0:#000}", fiveH);
                newTfh += String.Format("{0:#000}", fiveL);
            }
            return newTfh;
        }

        /// <summary>
        /// 只处理5万和1万图幅号
        /// </summary>
        /// <param name="newTfh"></param>
        /// <returns></returns>
        public static String FromNew2Old(String newTfh)
        {
            String oldTfh = newTfh.Substring(0, 1) + "-";
            oldTfh += newTfh.Substring(1, 2) + "-";
            String scale = newTfh.Substring(3, 1);
            int temH = Convert.ToInt32(newTfh.Substring(4, 3));
            int temL = Convert.ToInt32(newTfh.Substring(7, 3));
            int tem10H = 0;
            int tem10L = 0;
            int temLastH = 0;
            int temLastL = 0;
            switch (scale)
            {
                //1:5W
                case "E":
                    //求10万位置和编号
                    tem10H = FromLow2High(temH, 2);
                    tem10L = FromLow2High(temL, 2);
                    int ten1 = FromPosition2Number(tem10H, tem10L, 12);
                    oldTfh += ten1.ToString() + "-";
                    //求5万位置和编号
                    temLastH = FromSameLevel(temH, 2);
                    temLastL = FromSameLevel(temL, 2);
                    int five = FromPosition2Number(temLastH, temLastL, 2);
                    oldTfh += s_wuWanDict[five];
                    break;
                //1:1W
                case "G":
                    tem10H = FromLow2High(temH, 8);
                    tem10L = FromLow2High(temL, 8);
                    int ten2 = FromPosition2Number(tem10H, tem10L, 12);
                    oldTfh += ten2.ToString() + "-";
                    //求1万位置和编号
                    temLastH = FromSameLevel(temH, 8);
                    temLastL = FromSameLevel(temL, 8);
                    int one = FromPosition2Number(temLastH, temLastL, 8);
                    oldTfh += one.ToString();
                    break;
            }
            return oldTfh;
        }
        public static int FromSameLevel(int pos, int times)
        {
            int tem = pos % times;
            if (0 == tem)
            {
                tem = times;
            }
            return tem;
        }
        public static int FromLow2High(int low, int times)
        {
            return (low - 1) / times + 1;
        }
        public static int FromPosition2Number(int h, int l, int times)
        {
            return (h - 1) * times + l;
        }
        public static int FromNumber2PositionH(int pos, int times)
        {
            return (pos - 1) / times + 1;
        }
        public static int FromNumber2PositionL(int pos, int times)
        {
            return (pos - 1) % times + 1;
        }
        private String m_oldTFH;
        private String m_newTFH;
        private double m_left;
        private double m_bottom;
        private double m_top;
        private double m_right;
        private TFHScale m_scale;
        private double m_detx;
        private double m_dety;
        public TFHObject()
        {
        }

        public TFHScale Scale
        {
            get { return m_scale; }
            set
            {
                m_scale = value;
                m_detx = TFHObject.s_millionDetX / (int)m_scale;
                m_dety = TFHObject.s_millionDetY / (int)m_scale;
            }
        }
        public String OldTFH
        {
            get { return m_oldTFH; }
            set
            {
                m_oldTFH = value;
                m_newTFH = TFHObject.FromOld2New(value);
            }
        }
        public String NewTFH
        {
            get { return m_newTFH; }
            set
            {
                m_newTFH = value;
                m_oldTFH = TFHObject.FromNew2Old(value);
            }
        }
        public double Left
        {
            get { return m_left; }
            set { m_left = value; }
        }
        public double Bottom
        {
            get { return m_bottom; }
            set { m_bottom = value; }
        }
        public double Top
        {
            get { return m_top; }
            set { m_top = value; }
        }
        public double Right
        {
            get { return m_right; }
            set { m_right = value; }
        }
        
        /// <summary>
        /// 得到上下左右四个点，分别是左下角，左上角，右上角，右下角
        /// </summary>
        /// <returns></returns>
        public IList<TFHPoint> ComputePtList()
        {
            IList<TFHPoint> lst = new List<TFHPoint>(4);

            TFHPoint pt = new TFHPoint();
            pt.m_x = m_left;
            pt.m_y = m_bottom;
            lst.Add(pt);

            pt = new TFHPoint();
            pt.m_x = m_left;
            pt.m_y = m_top;
            lst.Add(pt);

            pt = new TFHPoint();
            pt.m_x = m_right;
            pt.m_y = m_top;
            lst.Add(pt);

            pt = new TFHPoint();
            pt.m_x = m_right;
            pt.m_y = m_bottom;
            lst.Add(pt);

            pt = new TFHPoint();
            pt.m_x = m_left;
            pt.m_y = m_bottom;
            lst.Add(pt);
            return lst;
        }
        
        
        public void ComputeTFH()
        {
            NewTFH = GetTFH((m_left + m_right) / 2, (m_bottom + m_top) / 2);
        }

        /// <summary>
        /// 根据坐标计算新图幅号
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        protected String GetTFH(double x, double y)
        {
            int h = (int)(y / TFHObject.s_millionDetY) + 1;
            int l = (int)(x / TFHObject.s_millionDetX) + 31;
            int millonH = h;
            int millongL = l;

            double temy = y - ((int)(y / TFHObject.s_millionDetY)) * TFHObject.s_millionDetY;
            double temx = x - ((int)(x / TFHObject.s_millionDetX)) * TFHObject.s_millionDetX;
            int minH = (int)(Math.Ceiling(TFHObject.s_millionDetY / m_dety) - Math.Ceiling(temy / m_dety));
            int minL = (int)Math.Ceiling(temx / m_detx);

            int innerH = minH + 1;
            int nnerL = minL;
            String temStr = TFHObject.s_HH[millonH].ToString();
            temStr += String.Format("{0:#00}", millongL);
            temStr += Scale;
            temStr += String.Format("{0:#000}", innerH);
            temStr += String.Format("{0:#000}", nnerL);
            return temStr;
        }

        public bool Parse(String tfh)
        {
            String[] sp = tfh.Split('-');
            //old tfh
            if (sp.Length == 4)
            {
                OldTFH = tfh;
            }
            //new tfh
            else if (sp.Length < 2)
            {
                NewTFH = tfh;
            }
            else
            {
                MessageBox.Show("analysis failor:" + tfh);
                return false;
            }
            //计算坐标
            String[] tfhPart = new string[6];
            tfhPart[0] = m_newTFH.Substring(0, 1);
            tfhPart[1] = m_newTFH.Substring(1, 2);
            tfhPart[3] = m_newTFH.Substring(3, 1);
            tfhPart[4] = m_newTFH.Substring(4, 3);
            tfhPart[5] = m_newTFH.Substring(7, 3);

            double millionTop = ((int)tfhPart[0][0] - 64) * 4;
            int millionLNumber = Convert.ToInt32(tfhPart[1]) - 31;
            double millionX = millionLNumber * 6;
            int temH = Convert.ToInt32(tfhPart[4]);
            int temL = Convert.ToInt32(tfhPart[5]);
            switch (tfhPart[3])
            {
                //十万
                case "D":
                    m_scale = TFHScale.D;
                    break;
                //五万
                case "E":
                    m_scale = TFHScale.E;
                    break;
                case "F":
                    m_scale=TFHScale.F;
                    break;
                //一万
                case "G":
                    m_scale = TFHScale.G;
                    break;
                case "H":
                    m_scale = TFHScale.H;
                    break;
                case "I":
                    m_scale = TFHScale.I;
                    break;
                case "J":
                    m_scale = TFHScale.J;
                    break;
            }
            m_left = millionX + (temL - 1) * (TFHObject.s_millionDetX / (int)m_scale);
            m_top = millionTop - (temH - 1) * (TFHObject.s_millionDetY / (int)m_scale);
            m_right = m_left + (TFHObject.s_millionDetX / (int)m_scale); ;
            m_bottom = m_top - (TFHObject.s_millionDetY / (int)m_scale); ;
            return true;
        }
    }
    /// <summary>
    /// J: 1000, I:2000 H  5000 ;G 10000   A :100 0000
    /// </summary>
    public enum TFHScale
    {
        A = 1,
        B = 2,
        C = 4,
        D = 12,
        E = 24,
        F = 48,
        G = 96,
        H = 192,
        I = 576,
        J = 1152,
        K=2304
    }
    public class TFHPoint
    {
        public double m_x;
        public double m_y;
    }
}
