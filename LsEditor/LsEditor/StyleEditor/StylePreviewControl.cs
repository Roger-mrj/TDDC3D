using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Display;
namespace RCIS.Style.StyleEditor
{
    public partial class StylePreviewControl : UserControl
    {
        public StylePreviewControl()
        {
            InitializeComponent();
        }
        private ISymbol m_curStyle;
        public ISymbol PreviewedStyle
        {
            set
            {
                this.m_curStyle = value;
                if (this.m_curStyle is ILineSymbol)
                    this.cbDogleg.Enabled = true;
                else this.cbDogleg.Enabled = false;
                this.ShowStyle();
            }
            get
            {
                return this.m_curStyle;
            }
        }
        private void ShowStyle()
        {
            if (this.PreviewedStyle == null)
            {
                this.pbStyle.Image = null;
            }
            else
            {
                int aWidth = this.pbStyle.Width;
                int aHeight = this.pbStyle.Height;
                
                Image aImg = SymbolHelper.StyleToImage(this.PreviewedStyle, aWidth, aHeight);
                if (this.cbAxies.Checked)
                {
                    Pen aPen=new Pen (Color.Blue );
                    aPen.DashStyle=DashStyle.Dash;
                    Graphics gc = Graphics.FromImage(aImg);
                    gc.DrawLine(aPen, new PointF(0, (float)aHeight / 2), new PointF(aWidth, (float)aHeight / 2));
                    gc.DrawLine(aPen, new PointF((float)aWidth / 2, 0), new PointF((float)aWidth / 2, aHeight));
                }
                this.pbStyle.Image = aImg;
            }
        }

        private void cbAxies_CheckedChanged(object sender, EventArgs e)
        {
            this.ShowStyle();
        }

        private void pbStyle_SizeChanged(object sender, EventArgs e)
        {
            this.ShowStyle();
        }

        private void cbScale_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.PreviewedStyle == null)
            {
                this.pbStyle.Image = null;
            }
            else
            {
                //int aWidth = this.pbStyle.Width;
                //int aHeight = this.pbStyle.Height;

                //Image aImg = SymbolHelper.StyleToImage(this.PreviewedStyle, aWidth, aHeight);
  
                
                //int newWidth; 
                //int newHeight; 
                ////你自定义图的长宽如 
                //newWidth = aImg.Width * 10;
                //newHeight = aImg.Height * 10; 
                //Bitmap newImg = new Bitmap(newWidth, newHeight);
                //Graphics g = Graphics.FromImage(aImg);
                //g.DrawImage(aImg, 0, 0, newWidth, newHeight);

                //pbStyle.Image = newImg; 
                this.pbStyle.SizeMode = PictureBoxSizeMode.StretchImage;

                Rectangle oldrct;
                Bitmap bmp;
                bmp = (Bitmap)this.pbStyle.Image;

                if (bmp == null) return;
                oldrct = new Rectangle(0, 0, bmp.Width, bmp.Height);
                this.pbStyle.Image = bmp;
                Bitmap tmpbmp = null;
                int i = 3;
                if (i > 0)
                {
                    tmpbmp = new Bitmap(bmp.Width * 2, bmp.Height * 2);

                }
                else
                {
                    tmpbmp = new Bitmap(bmp.Width / 2, bmp.Height / 2);
                }
                Graphics g = Graphics.FromImage(tmpbmp);
                Rectangle newrct = new Rectangle(0, 0, tmpbmp.Width, tmpbmp.Height);

                g.DrawImage(bmp, newrct, oldrct, GraphicsUnit.Pixel);//newrct是你的目标矩形位置，oldrct是你原始图片的起始矩形位置 
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                //oldrct = oldrct;
                pbStyle.Image = tmpbmp;
                g.Dispose();
                pbStyle.Update();

                this.ShowStyle();




            }

        }
    }
}
