using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using DevExpress.XtraEditors;

namespace TDDC3D.raster
{
    
    public class RasterInfoClass
    {
        private string colsrows = "";
        private int pyramidLevel = 0;

         [CategoryAttribute("栅格信息"),
        DisplayNameAttribute("金字塔级别")]
        public int PyramidLevel
        {
            get { return pyramidLevel; }
            set { pyramidLevel = value; }
        }


          [CategoryAttribute("栅格信息"),
        DisplayNameAttribute("格式")]
        public string Format
        {
            get { return format; }
            set { format = value; }
        }

        private int numberBands = 0;

         [CategoryAttribute("栅格信息"),
        DisplayNameAttribute("波段数")]
        public int NumberBands
        {
            get { return numberBands; }
            set { numberBands = value; }
        }
         private string cellSize = "";

         [CategoryAttribute("栅格信息"),
        DisplayNameAttribute("像元大小")]
         public string CellSize
         {
             get { return cellSize; }
             set { cellSize = value; }
         }
        private string format = "";
        private string pixelType = "";

        [CategoryAttribute("栅格信息"),
        DisplayNameAttribute("像素类型")]
        public string PixelType
        {
            get { return pixelType; }
            set { pixelType = value; }
        }
        private double noDataValue;

        [CategoryAttribute("栅格信息"),
        DisplayNameAttribute("无数据值")]
        public double NoDataValue
        {
            get { return noDataValue; }
            set { noDataValue = value; }
        }
        private int pixelDepth;

        [CategoryAttribute("栅格信息"),
        DisplayNameAttribute("像素位深")]
        public int PixelDepth
        {
            get { return pixelDepth; }
            set { pixelDepth = value; }
        }
        private string pyramids;

        private string colormap;
        [CategoryAttribute("栅格信息"),
        DisplayNameAttribute("色带")]
        public string Colormap
        {
            get { return colormap; }
            set { colormap = value; }
        }

        [CategoryAttribute("栅格信息"),
        DisplayNameAttribute("行列数")]
        public string Colsrows
        {
            get { return colsrows; }
            set { colsrows = value; }
        }


        private double top, left, right, bottom;
        [CategoryAttribute("范围"),
        DisplayNameAttribute("上")]
        public double Top
        {
            get { return top; }
            set { top = value; }
        }
        [CategoryAttribute("范围"),
        DisplayNameAttribute("下")]
        public double Bottom
        {
            get { return bottom; }
            set { bottom = value; }
        }
        [CategoryAttribute("范围"),
         DisplayNameAttribute("左")]
        public double Left
        {
            get { return left; }
            set { left = value; }
        }
        [CategoryAttribute("范围"),
        DisplayNameAttribute("右")]
        public double Right
        {
            get { return right; }
            set { right = value; }
        }


        private string spatialRefName = "";
        [CategoryAttribute("空间参考"),
        DisplayNameAttribute("名称")]
        public string SpatialRefName
        {
            get { return spatialRefName; }
            set { spatialRefName = value; }
        }

        private string project;
        [CategoryAttribute("空间参考"),
        DisplayNameAttribute("投影")]
        public string Project
        {
            get { return project; }
            set { project = value; }
        }

        private string datum = "";
        [CategoryAttribute("空间参考"),
        DisplayNameAttribute("基准面")]
        public string Datum
        {
            get { return datum; }
            set { datum = value; }
        }

        private string  centerMeridian;
        [CategoryAttribute("空间参考"),
        DisplayNameAttribute("中央经线")]
        public string  CenterMeridian
        {
            get { return centerMeridian; }
            set { centerMeridian = value; }
        }

        private string linearUnit = "";
        [CategoryAttribute("空间参考"),
        DisplayNameAttribute("线性单位")]
        public string LinearUnit
        {
            get { return linearUnit; }
            set { linearUnit = value; }
        }

        private double semiMajorAxis;
        [CategoryAttribute("空间参考"),
        DisplayNameAttribute("长半轴")]
        public double SemiMajorAxis
        {
            get { return semiMajorAxis; }
            set { semiMajorAxis = value; }
        }
        private double semiMinorAxis;
        [CategoryAttribute("空间参考"),
        DisplayNameAttribute("短半轴")]
        public double SemiMinorAxis
        {
            get { return semiMinorAxis; }
            set { semiMinorAxis = value; }
        }

        private double falseEasting = 0;

        [CategoryAttribute("空间参考"),
        DisplayNameAttribute("东偏移")]
        public double FalseEasting
        {
            get { return falseEasting; }
            set { falseEasting = value; }
        }
        private double falseNorthing = 0;

        [CategoryAttribute("空间参考"),
        DisplayNameAttribute("北偏移")]
        public double FalseNorthing
        {
            get { return falseNorthing; }
            set { falseNorthing = value; }
        }
    }
}
