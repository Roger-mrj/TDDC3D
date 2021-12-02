using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using RCIS.Global;
using RCIS.Utility;

namespace RCIS.Controls
{
    public partial class SplashForm : Form
    {
        public SplashForm()
        {
            InitializeComponent();
        }
        public string Caption
        {
            get
            {
                return this.lbDesc.Text;
            }
            set
            {
                if (value == null) value = "";
                this.lbDesc.Text = "第三次全国国土调查数据库系统:" + value;
                Application.DoEvents();
            }
        }
        public string ImagePath
        {
            set
            {
                this.m_imagePath=value;
            }
        }
        
        private string m_imagePath;
        private void SplashForm_Load(object sender, EventArgs e)
        {
            Image aImage = ImageHelper.LoadImage(Global.AppParameters.ImgPath+ "\\启动背景new.bmp");
            
            if (aImage != null)
            {
                this.BackgroundImage = aImage;
            }
        }
    }
}