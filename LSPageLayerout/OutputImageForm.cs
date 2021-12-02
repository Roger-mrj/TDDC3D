using ESRI.ArcGIS.Carto;
using Microsoft.Win32;
using sycCommonLib;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace RCIS.Controls
{
    public partial class OutputImageForm : Form
    {
        public OutputImageForm()
        {
            InitializeComponent();
        }

        private int m_TimerSecond;

        public ESRI.ArcGIS.Controls.AxMapControl m_MapControl;
        public ESRI.ArcGIS.Controls.AxPageLayoutControl m_PageControl;
        public DevExpress.XtraTab.XtraTabControl   m_myTab;


        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                string sImagePath = folderBrowserDialog1.SelectedPath;
                textBox1.Text = sImagePath;

                RegistryKey keyWrite = Registry.CurrentUser;
                keyWrite = keyWrite.CreateSubKey("Software\\landstar");
                keyWrite.SetValue("ImagePath", sImagePath);
            }
        }

        private void OutputImageForm_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;	//BMP

            //检查上次的图像的存放路径:
            RegistryKey keyRead = Registry.CurrentUser;
            keyRead = keyRead.OpenSubKey("Software\\landstar");
            if (keyRead == null) ;
            else
            {
                object oPath = keyRead.GetValue("ImagePath");
                if (oPath == null) ;
                else textBox1.Text = oPath.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //检查下基本的参数:
            string sPath = textBox1.Text.Trim();
            if (Directory.Exists(sPath) == false)
            {
                MessageBox.Show("指定的路径不存在!");
                return;
            }
            string sType = comboBox1.SelectedItem.ToString().Trim();
            double dDPI = 96.0;
            try
            {
                dDPI = (double)numericUpDown1.Value;
            }
            catch (Exception E)
            {
                MessageBox.Show("DPI错误: " + E.Message + " !");
                return;
            }
            string sName = txtFileName.Text.Trim();
            sName = sName + "." + sType;
            string sAllFileName = "";
            int nLen = sPath.Length;
            if (sPath[nLen - 1] == '\\')
                sAllFileName = sPath + sName;
            else
                sAllFileName = sPath + "\\" + sName;
            IActiveView act = null;
            if (m_myTab != null)
            {
                if (m_myTab.SelectedTabPageIndex == 0)
                    act = m_MapControl.ActiveView.FocusMap as IActiveView;
                else if (m_myTab.SelectedTabPageIndex == 1)
                {
                    if (m_MapControl != null)
                        m_PageControl.ActiveView.FocusMap.ReferenceScale = m_MapControl.ReferenceScale;
                    act = m_PageControl.ActiveView;
                }
            }
            else
            {
                if (m_MapControl == null)
                {
                    if (m_MapControl != null)
                        m_PageControl.ActiveView.FocusMap.ReferenceScale = m_MapControl.ReferenceScale;
                    act = m_PageControl.ActiveView;
                }
            }
            if (act==null)
            {
                MessageBox.Show("无法确定当前视图类型[MapControl or PageLayout] !");
                return;
            }

            //... ...tagRECT
            string sRetErrorInfo = "";
            sycCommonFuns CommonClass = new sycCommonLib.sycCommonFuns();
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            long lDPI = (long)dDPI;
            bool bRet = CommonClass.ExportActiveViewParameterized(act, lDPI, 1, false , sAllFileName, ref sRetErrorInfo);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            CommonClass.Dispose();
            if (bRet == true)
            {
                MessageBox.Show("产生的图像: " + sAllFileName + " !");
                this.DialogResult = DialogResult.OK;

                Process myProcess = new Process();
                myProcess.StartInfo.FileName = sAllFileName;
                myProcess.Start();
            }
            else
                MessageBox.Show("Error:" + sRetErrorInfo + " !");
        }


        private void timer1_Tick(object sender, System.EventArgs e)
        {
            m_TimerSecond++;
            string sTips = m_TimerSecond.ToString() + "秒";
            button2.Text = sTips;
            Application.DoEvents();

        }
    }
}