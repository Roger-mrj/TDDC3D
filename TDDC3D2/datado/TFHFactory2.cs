using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TDDC3D.datado
{
    public class TFHFactory2
    {
        protected double m_left;
        protected double m_bottom;
        protected double m_right;
        protected double m_top;
        protected int m_lNumber;
        protected int m_hNumber;
        protected double m_detx;
        protected double m_dety;
        private IList<TFHObject> m_list;
        protected TFHScale m_scale;
        public TFHFactory2() { }
        public IList<TFHObject> TFHList
        {
            get { return m_list; }
            set { m_list = value; }
        }
        /// <summary>
        /// 坐标初始化范围
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        public virtual void FromExtent2TFH(double x1, double y1, double x2, double y2, TFHScale scale)
        {
            m_scale = scale;
            m_detx = TFHObject.s_millionDetX / (int)scale;
            m_dety = TFHObject.s_millionDetY / (int)scale;
            m_left = ((int)(x1 / m_detx)) * m_detx;
            m_bottom = ((int)(y1 / m_dety)) * m_dety;
            m_top = ((int)(y2 / m_dety) + 1) * m_dety;
            m_right = ((int)(x2 / m_detx) + 1) * m_detx;
            m_lNumber = (int)((m_right - m_left) / m_detx);
            m_hNumber = (int)((m_top - m_bottom) / m_dety);
            ComputeTFHInfo();
        }

        public void FromPoint2TFH(double x, double y, TFHScale scale)
        {
            m_scale = scale;
            m_detx = TFHObject.s_millionDetX / (int)scale;
            m_dety = TFHObject.s_millionDetY / (int)scale;
            m_left = ((int)(x / m_detx)) * m_detx;
            m_bottom = ((int)(y / m_dety)) * m_dety;
            m_top = m_bottom + m_dety;
            m_right = m_left + m_detx;
            m_lNumber = 1;
            m_hNumber = 1;
            ComputeTFHInfo();
        }
        public void Parse(IList<String> tfhList)
        {
            m_list = new List<TFHObject>();
            foreach (String tfh in tfhList)
            {
                TFHObject tfhObject = new TFHObject();
                if (tfhObject.Parse(tfh))
                {
                    m_list.Add(tfhObject);
                }
            }
        }
        private void ComputeTFHInfo()
        {
            m_list = new List<TFHObject>();
            for (int x = 0; x < m_lNumber; x++)
            {
                for (int y = 0; y < m_hNumber; y++)
                {
                    TFHObject tfhObject = new TFHObject();
                    tfhObject.Scale = m_scale;
                    tfhObject.Left = m_left + m_detx * x;
                    tfhObject.Bottom = m_bottom + m_dety * y;
                    tfhObject.Right = m_left + m_detx * (x + 1);
                    tfhObject.Top = m_bottom + m_dety * (y + 1);
                    tfhObject.ComputeTFH();
                    m_list.Add(tfhObject);
                }
            }
        }

    }
}
