using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace RCIS.Utility
{
    /// <summary>
    /// 图片相关的 一些函数，比如加载图像
    /// </summary>
    public class ImageHelper
    {
        public static Image LoadImage(string sPath)
        {
            if (!(System.IO.File.Exists(sPath)))
                return null;
            try
            {
                Image img = Image.FromFile(sPath);
                return img;
            }
            catch { return null; }
        }
    }
}
