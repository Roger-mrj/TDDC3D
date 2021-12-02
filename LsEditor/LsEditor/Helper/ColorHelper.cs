using System;
using System.Collections .Generic ;
using System.Drawing ;
using ESRI.ArcGIS.Display;
namespace RCIS
{
	/// <summary>
	/// ColorHelper 的摘要说明。
	/// </summary>
	public class ColorHelper
	{
        private static Random m_random=new Random();
        
        public static IColor CreateColor(byte alpha,int red,int green,int blue)
        {
            RgbColorClass rgbClr=new RgbColorClass ();
            rgbClr.Red =red;
            rgbClr.Green =green;
            rgbClr.Blue =blue;
            rgbClr.Transparency =alpha;
            return rgbClr as IColor;
        }
        public static IColor CreateColor(System.Drawing .Color msColor)
        {
            return ColorHelper.CreateColor (msColor.R ,msColor.G ,msColor.B );
        }
        public static IColor CreateColor(int red, int green, int blue)
        {
            RgbColorClass rgbClr = new RgbColorClass();
            rgbClr.Red = red;
            rgbClr.Green = green;
            rgbClr.Blue = blue;
            return rgbClr as IColor;
        }
        public static System.Drawing .Color CreateColor(IColor esriColor)
        {
        
            System.Drawing .Color msClr=Color.Black ;
            if (esriColor == null) return msClr;
            if(esriColor is IRgbColor )
            {
                IRgbColor rgbColor=esriColor as IRgbColor ;
                msClr=Color.FromArgb (rgbColor.Red ,rgbColor.Green ,rgbColor.Blue );
            }
            else 
            {
                int b = esriColor.RGB % 256;
                int g = (esriColor.RGB / 256) % 256;
                int r = (esriColor.RGB / 256/256) % 256;
                msClr=System.Drawing .Color.FromArgb (b,g,r );
            }
            return msClr;
        }
        public static IColor CreateRandomColor(byte alpha)
        {
            //创建随机的颜色
            int r=ColorHelper.m_random .Next (255);
            int g=ColorHelper.m_random .Next (255);
            int b=ColorHelper.m_random .Next (255);
            return ColorHelper.CreateColor (alpha,r,g,b);
        }
        public static IColor CreateRandomColor()
        {
            return CreateRandomColor(100);
        }
        public static Color CreateRandomMSColor()
        {
            int r = ColorHelper.m_random.Next(255);
            int g = ColorHelper.m_random.Next(255);
            int b = ColorHelper.m_random.Next(255);
            return Color.FromArgb(r, g, b);
        }
        public static List<IColor> CreateRandomColorList(int pCount)
        {
            List<Color> msList = new List<Color>();
            List<IColor> rList = new List<IColor>();
            while (msList.Count < pCount)
            {
                Color aClr= ColorHelper.CreateRandomMSColor();
                if (!msList.Contains(aClr))
                {
                    msList.Add(aClr);
                }
            }
            foreach (Color msColor in msList)
            {
rList.Add (ColorHelper .CreateColor (msColor ));
            }
            return rList;
        }
	}
}
