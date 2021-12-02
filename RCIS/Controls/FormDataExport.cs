using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Collections;
using System.IO;

using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Carto;

namespace RCIS.Controls
{
    public partial class FormDataExport : Form
    {
        string prefix = "";

        public FormDataExport()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Shapefile路径
        /// </summary>
        public string ShapefilePath
        {
            get
            {
                return this.textBoxShapefilePath.Text;
            }
            set
            {
                this.textBoxShapefilePath.Text = value;
            }
        }

        public int ExportFeature
        {
            get
            {
                return this.GetExportFeaturClass(this.comboBoxExportFeature.Text);
            }
        }

        /// <summary>
        /// 关闭数据输出窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 选择输出shapefile名称路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "请选择导出路径与文件名";
            sfd.Filter = "Shapefile|*.shp";
            sfd.InitialDirectory = Application.StartupPath;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                this.textBoxShapefilePath.Text = sfd.FileName;
            }
        }

        /// <summary>
        /// 确认输出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOkay_Click(object sender, EventArgs e)
        {

            string folderPath = this.textBoxShapefilePath.Text.Substring(0, this.textBoxShapefilePath.Text.LastIndexOf("\\"));
            if (!Directory.Exists(folderPath))
            {
                if (MessageBox.Show("您所输入的路径不存在，是否创建？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Directory.CreateDirectory(folderPath);
                }
                else
                {
                    this.textBoxShapefilePath.Text = "";
                }
            }

            if (this.textBoxShapefilePath.Text == "")
            {
                if (MessageBox.Show("您尚未制定导出文件路径，无法导出文件。\n\n是否继续？\"是\"不进行导出操作，\"否\"重新制定路径。"
                    , "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    //this.DialogResult = DialogResult.Cancel;
                    this.Close();
                }
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public void SetExportFeatures(bool selectedFeature)
        {
            if (selectedFeature == true)
            {
                this.comboBoxExportFeature.Items.Add("选定的要素");
                this.comboBoxExportFeature.Items.Add("未选定的要素");
            }
            this.comboBoxExportFeature.Items.Add("全部要素");
            this.comboBoxExportFeature.Items.Add("当前视图范围内的要素");
        }

        /// <summary>
        /// 获取输出图层的要素类型
        /// </summary>
        /// <param name="sFeatureClass">输入选择的类型</param>
        /// <returns>
        /// 0,未选定任何类型
        /// 1,选定的要素
        /// 2,未选定的要素
        /// 3,全部要素
        /// 4,当前视图范围内的要素
        /// </returns>
        private int GetExportFeaturClass(string sFeatureClass)
        {
            if (sFeatureClass == "选定的要素")
            {
                return 1;
            }
            if (sFeatureClass == "未选定的要素")
            {
                return 2;
            }
            if (sFeatureClass == "全部要素")
            {
                return 3;
            }
            if (sFeatureClass == "当前视图范围内的要素")
            {
                return 4;
            }
            return 0;
        }

        private void FormDataExport_Load(object sender, EventArgs e)
        {
            this.comboBoxExportFeature.SelectedIndex = 0;
        }
    }
}
