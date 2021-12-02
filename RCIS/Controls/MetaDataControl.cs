using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Data.OleDb;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;
using System.IO;
using ESRI.ArcGIS.DataSourcesRaster;
using System.Xml;
using DevExpress.XtraTreeList.Nodes;

using RCIS.GISCommon;
using RCIS.Utility;

namespace RCIS.Controls
{
    public partial class MetaDataControl : UserControl
    {
        IWorkspace m_WS;
        AxMapControl m_MapControl;
        private string sql = string.Empty;
        private string stable = string.Empty;
        DataRow dr = null;
        DataSet ds = null;
        string sMetaDataFile = Application.StartupPath + @"\template\metadata.xml";
        public IWorkspace Workspace
        {
            get
            {
                return m_WS;
            }
            set
            { m_WS = value; }
            
        }
        public AxMapControl MapControl
        {
            get
            {
                if (m_MapControl == null)
                {
                    m_MapControl = new AxMapControl();
                }
                return m_MapControl;
            }
            set
            { m_MapControl = value; }

        }
        public MetaDataControl()
        {
            
            InitializeComponent();
         

            this.tbSR.ReadOnly = true;
            this.toolStripComboBox1.SelectedIndex = 0;
            this.toolStripComboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
       
            FillHashTab();
            
        }
        #region 显示
        /// <summary>
        /// 显示数据
        /// </summary>
        private void Display(string sFileName)
        {
            try
            {

                this.Cursor = Cursors.WaitCursor;
                XmlDocument doc = new XmlDocument();

                doc.Load(sFileName);
                DisplayMetaData(doc);
               // LoadMetaTree(doc);
                //加载投影信息
                LoadPrjectionInfo();
                //加载图形图层
                LoadMapLayerInfo();
               
                //坐标转换为经纬度
                XY2JYD();
            
                this.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            { this.Cursor = Cursors.Arrow; }
        }
        #endregion
        #region 加载数据集名称
        private string GetDatasetName(IWorkspace pWS)
        {
            if (pWS == null) return "";

            IFeatureWorkspace pFWS = pWS as IFeatureWorkspace;

            IDataset pDateset = pFWS.OpenFeatureDataset(RCIS.Global.AppParameters.DATASET_DEFAULT_NAME);

            IFeatureDataset pFD = (IFeatureDataset)pDateset;

            return pFD.Name.ToString();
        }
        #endregion


        #region 加载元数据树型结构
        private string sFlag = "：";
        private void LoadMetaTree(XmlDocument doc)
        {
           // this.treeView1.Nodes.Clear();
         
            //#region  标识信息
            //TreeNode pBSNode = new TreeNode("标识信息");
            //this.treeView1.Nodes.Add(pBSNode);
            //TreeNode pMDBSNode = new TreeNode("MD_标识");
            //pBSNode.Nodes.Add(pMDBSNode);
            //pMDBSNode.Nodes.Add("数据集引用", "数据集引用");
            //pMDBSNode.Nodes["数据集引用"].Nodes.Add("CI_引用", "CI_引用");

            //pMDBSNode.Nodes["数据集引用"].Nodes["CI_引用"].Nodes.Add("名称", "名称" + sFlag + XMLHelper.XmlReadValue(doc, "标识信息", "数据集引用", "名称"));
            //pMDBSNode.Nodes["数据集引用"].Nodes["CI_引用"].Nodes.Add("日期", "日期" + sFlag + XMLHelper.XmlReadValue(doc, "标识信息", "数据集引用", "日期"));
            ////  pMDBSNode.Nodes["数据集引用"].Nodes["CI_引用"].Nodes.Add("版本", "版本" + sFlag + XMLHelper.XmlReadValue(doc, "标识信息", "数据集引用", "版本"));

            //pMDBSNode.Nodes.Add("语种", "语种" + sFlag + XMLHelper.XmlReadValue(doc, "标识信息", "语种"));
            //pMDBSNode.Nodes.Add("摘要", "摘要" + sFlag + XMLHelper.XmlReadValue(doc, "标识信息", "摘要"));
            //pMDBSNode.Nodes.Add("现状", "现状" + sFlag + XMLHelper.XmlReadValue(doc, "标识信息", "现状"));

            //pMDBSNode.Nodes.Add("地理范围", "地理范围");
            //pMDBSNode.Nodes["地理范围"].Nodes.Add("EX_地理坐标范围", "EX_地理坐标范围");
            //pMDBSNode.Nodes["地理范围"].Nodes["EX_地理坐标范围"].Nodes.Add("西边经度", "西边经度" + sFlag + XMLHelper.XmlReadValue(doc, "标识信息", "地理范围", "西边经度"));
            //pMDBSNode.Nodes["地理范围"].Nodes["EX_地理坐标范围"].Nodes.Add("东边经度", "东边经度" + sFlag + XMLHelper.XmlReadValue(doc, "标识信息", "地理范围", "东边经度"));
            //pMDBSNode.Nodes["地理范围"].Nodes["EX_地理坐标范围"].Nodes.Add("南边纬度", "南边纬度" + sFlag + XMLHelper.XmlReadValue(doc, "标识信息", "地理范围", "南边纬度"));
            //pMDBSNode.Nodes["地理范围"].Nodes["EX_地理坐标范围"].Nodes.Add("北边纬度", "北边纬度" + sFlag + XMLHelper.XmlReadValue(doc, "标识信息", "地理范围", "北边纬度"));

            //pMDBSNode.Nodes.Add("地理描述", "地理描述");
            //pMDBSNode.Nodes["地理描述"].Nodes.Add("SI_地理描述", "SI_地理描述");
            //pMDBSNode.Nodes["地理描述"].Nodes["SI_地理描述"].Nodes.Add("地理标识符", "地理标识符" + sFlag + XMLHelper.XmlReadValue(doc, "标识信息", "地理描述", "地理标识符"));

            //pMDBSNode.Nodes.Add("时间范围", "时间范围");
            //pMDBSNode.Nodes["时间范围"].Nodes.Add("EX_时间范围", "EX_时间范围");
            //pMDBSNode.Nodes["时间范围"].Nodes["EX_时间范围"].Nodes.Add("范围", "范围");
            //pMDBSNode.Nodes["时间范围"].Nodes["EX_时间范围"].Nodes["范围"].Nodes.Add("TM_时间段", "TM_时间段");
            //pMDBSNode.Nodes["时间范围"].Nodes["EX_时间范围"].Nodes["范围"].Nodes["TM_时间段"].Nodes.Add("起始时间", "起始时间" + sFlag + XMLHelper.XmlReadValue(doc, "标识信息", "时间范围", "范围", "起始时间"));
            //pMDBSNode.Nodes["时间范围"].Nodes["EX_时间范围"].Nodes["范围"].Nodes["TM_时间段"].Nodes.Add("终止时间", "终止时间" + sFlag + XMLHelper.XmlReadValue(doc, "标识信息", "时间范围", "范围", "终止时间"));

            //pMDBSNode.Nodes.Add("垂向范围", "垂向范围");
            //pMDBSNode.Nodes["垂向范围"].Nodes.Add("EX_垂向范围", "EX_垂向范围");
            //pMDBSNode.Nodes["垂向范围"].Nodes["EX_垂向范围"].Nodes.Add("最小垂向坐标值", "最小垂向坐标值" + sFlag + XMLHelper.XmlReadValue(doc, "标识信息", "垂向范围", "最小垂向坐标值"));
            //pMDBSNode.Nodes["垂向范围"].Nodes["EX_垂向范围"].Nodes.Add("最大垂向坐标值", "最大垂向坐标值" + sFlag + XMLHelper.XmlReadValue(doc, "标识信息", "垂向范围", "最大垂向坐标"));
            //pMDBSNode.Nodes["垂向范围"].Nodes["EX_垂向范围"].Nodes.Add("计量单位", "计量单位" + sFlag + XMLHelper.XmlReadValue(doc, "标识信息", "垂向范围", "计量单位"));


            //pMDBSNode.Nodes.Add("表示方式", "表示方式" + sFlag + XMLHelper.XmlReadValue(doc, "标识信息", "表示方式"));
            //pMDBSNode.Nodes.Add("数据格式名称", "数据格式名称" + sFlag + XMLHelper.XmlReadValue(doc, "标识信息", "数据格式名称"));
            //pMDBSNode.Nodes.Add("类别", "类别" + sFlag + XMLHelper.XmlReadValue(doc, "标识信息", "类别"));
            //pMDBSNode.Nodes.Add("可交换数据格式名称", "可交换数据格式名称" + sFlag + XMLHelper.XmlReadValue(doc, "标识信息", "可交换数据格式名称"));
            //pMDBSNode.Nodes.Add("调查比例尺", "调查比例尺" + sFlag + XMLHelper.XmlReadValue(doc, "标识信息", "调查比例尺"));

            //pMDBSNode.Nodes.Add("数据集联系信息", "数据集联系信息");
            //pMDBSNode.Nodes["数据集联系信息"].Nodes.Add("CI_负责单位", "CI_负责单位");
            //pMDBSNode.Nodes["数据集联系信息"].Nodes["CI_负责单位"].Nodes.Add("负责单位名称", "负责单位名称" + sFlag + XMLHelper.XmlReadValue(doc, "标识信息", "数据集联系信息", "负责单位名称"));
            //pMDBSNode.Nodes["数据集联系信息"].Nodes["CI_负责单位"].Nodes.Add("联系人", "联系人" + sFlag + XMLHelper.XmlReadValue(doc, "标识信息", "数据集联系信息", "联系人"));
            //pMDBSNode.Nodes["数据集联系信息"].Nodes["CI_负责单位"].Nodes.Add("职责", "职责" + sFlag + XMLHelper.XmlReadValue(doc, "标识信息", "数据集联系信息", "职责"));
            //pMDBSNode.Nodes["数据集联系信息"].Nodes["CI_负责单位"].Nodes.Add("联系信息", "联系信息");
            //pMDBSNode.Nodes["数据集联系信息"].Nodes["CI_负责单位"].Nodes["联系信息"].Nodes.Add("CI_联系", "CI_联系");
            //pMDBSNode.Nodes["数据集联系信息"].Nodes["CI_负责单位"].Nodes["联系信息"].Nodes["CI_联系"].Nodes.Add("电话", "电话" + sFlag + XMLHelper.XmlReadValue(doc, "标识信息", "数据集联系信息", "联系信息", "电话"));
            //pMDBSNode.Nodes["数据集联系信息"].Nodes["CI_负责单位"].Nodes["联系信息"].Nodes["CI_联系"].Nodes.Add("传真", "传真" + sFlag + XMLHelper.XmlReadValue(doc, "标识信息", "数据集联系信息", "联系信息", "传真"));
            //pMDBSNode.Nodes["数据集联系信息"].Nodes["CI_负责单位"].Nodes["联系信息"].Nodes["CI_联系"].Nodes.Add("通信地址", "通信地址" + sFlag + XMLHelper.XmlReadValue(doc, "标识信息", "数据集联系信息", "联系信息", "通信地址"));
            //pMDBSNode.Nodes["数据集联系信息"].Nodes["CI_负责单位"].Nodes["联系信息"].Nodes["CI_联系"].Nodes.Add("邮政编码", "邮政编码" + sFlag + XMLHelper.XmlReadValue(doc, "标识信息", "数据集联系信息", "联系信息", "邮政编码"));
            //pMDBSNode.Nodes["数据集联系信息"].Nodes["CI_负责单位"].Nodes["联系信息"].Nodes["CI_联系"].Nodes.Add("电子信箱地址", "电子信箱地址" + sFlag + XMLHelper.XmlReadValue(doc, "标识信息", "数据集联系信息", "联系信息", "电子信箱地址"));
            //pMDBSNode.Nodes["数据集联系信息"].Nodes["CI_负责单位"].Nodes["联系信息"].Nodes["CI_联系"].Nodes.Add("网址", "网址" + sFlag + XMLHelper.XmlReadValue(doc, "标识信息", "数据集联系信息", "联系信息", "网址"));
            //pMDBSNode.Nodes.Add("静态浏览图信息", "静态浏览图信息");
            //pMDBSNode.Nodes["静态浏览图信息"].Nodes.Add("MD_浏览图", "MD_浏览图");
            //pMDBSNode.Nodes["静态浏览图信息"].Nodes["MD_浏览图"].Nodes.Add("文件名称", "文件名称" + sFlag + XMLHelper.XmlReadValue(doc, "标识信息", "静态浏览图信息", "文件名称"));

            //pMDBSNode.Nodes.Add("数据集限制", "数据集限制");
            //pMDBSNode.Nodes["数据集限制"].Nodes.Add("MD_数据集限制", "MD_数据集限制");
            //pMDBSNode.Nodes["数据集限制"].Nodes["MD_数据集限制"].Nodes.Add("MD_法律限制", "MD_法律限制");
            //pMDBSNode.Nodes["数据集限制"].Nodes["MD_数据集限制"].Nodes["MD_法律限制"].Nodes.Add("使用限制代码", "使用限制代码" + sFlag + XMLHelper.XmlReadValue(doc, "标识信息", "数据集限制", "MD_法律限制", "使用限制代码"));
            //pMDBSNode.Nodes["数据集限制"].Nodes["MD_数据集限制"].Nodes.Add("MD_安全限制", "MD_安全限制");
            //pMDBSNode.Nodes["数据集限制"].Nodes["MD_数据集限制"].Nodes["MD_安全限制"].Nodes.Add("安全等级代码", "安全等级代码" + sFlag + XMLHelper.XmlReadValue(doc, "标识信息", "数据集限制", "MD_安全限制", "安全等级代码"));

            //pMDBSNode.Nodes.Add("数据集格式", "数据集格式");
            //pMDBSNode.Nodes["数据集格式"].Nodes.Add("MD_格式", "MD_格式");
            //pMDBSNode.Nodes["数据集格式"].Nodes["MD_格式"].Nodes.Add("名称", "名称" + sFlag + XMLHelper.XmlReadValue(doc, "标识信息", "数据集格式", "名称"));
            //pMDBSNode.Nodes["数据集格式"].Nodes["MD_格式"].Nodes.Add("版本", "版本" + sFlag + XMLHelper.XmlReadValue(doc, "标识信息", "数据集格式", "版本"));
            //#endregion
            //#region  数据质量信息

            //TreeNode pSJZLNode = new TreeNode("数据质量信息");
            //this.treeView1.Nodes.Add(pSJZLNode);

            //pSJZLNode.Nodes.Add("概述", "概述");
            //pSJZLNode.Nodes["概述"].Nodes.Add("完整性", "完整性" + sFlag + XMLHelper.XmlReadValue(doc, "数据质量信息", "概述", "完整性"));
            //pSJZLNode.Nodes["概述"].Nodes.Add("规范性", "规范性" + sFlag + XMLHelper.XmlReadValue(doc, "数据质量信息", "概述", "规范性"));
            //pSJZLNode.Nodes["概述"].Nodes.Add("准确性", "准确性" + sFlag + XMLHelper.XmlReadValue(doc, "数据质量信息", "概述", "准确性"));
            //pSJZLNode.Nodes["概述"].Nodes.Add("检查验收", "检查验收" + sFlag + XMLHelper.XmlReadValue(doc, "数据质量信息", "概述", "检查验收"));

            //pSJZLNode.Nodes.Add("数据志", "数据志");
            //pSJZLNode.Nodes["数据志"].Nodes.Add("数据志", "航摄和纸质正射影像图调绘" + sFlag + XMLHelper.XmlReadValue(doc, "数据质量信息", "数据志", "航摄和纸质正射影像图调绘"));
            //pSJZLNode.Nodes["数据志"].Nodes.Add("数据志", "航天遥感和纸质正射影像图调绘" + sFlag + XMLHelper.XmlReadValue(doc, "数据质量信息", "数据志", "航天遥感和纸质正射影像图调绘"));
            //pSJZLNode.Nodes["数据志"].Nodes.Add("数据志", "放大航片调绘" + sFlag + XMLHelper.XmlReadValue(doc, "数据质量信息", "数据志", "放大航片调绘"));
            //pSJZLNode.Nodes["数据志"].Nodes.Add("数据志", "利用经加工详查数据库" + sFlag + XMLHelper.XmlReadValue(doc, "数据质量信息", "数据志", "利用经加工详查数据库"));
            //#endregion
            //#region 空间参照系统信息
            //TreeNode pKJCZXTNode = new TreeNode("空间参照系统信息");
            //this.treeView1.Nodes.Add(pKJCZXTNode);
            //pKJCZXTNode.Nodes.Add("SI_基于地理标识的空间参照系统", "SI_基于地理标识的空间参照系统");
            //pKJCZXTNode.Nodes["SI_基于地理标识的空间参照系统"].Nodes.Add("名称", "名称" + sFlag + XMLHelper.XmlReadValue(doc, "空间参照系统信息", "SI_基于地理标识的空间参照系统", "名称"));

            //pKJCZXTNode.Nodes.Add("SC_基于坐标的空间参照系统", "SC_基于坐标的空间参照系统");
            //pKJCZXTNode.Nodes["SC_基于坐标的空间参照系统"].Nodes.Add("SC_大地坐标参照系统", "SC_大地坐标参照系统");
            //pKJCZXTNode.Nodes["SC_基于坐标的空间参照系统"].Nodes["SC_大地坐标参照系统"].Nodes.Add("大地坐标参照系统名称", "大地坐标参照系统名称" + sFlag + XMLHelper.XmlReadValue(doc, "空间参照系统信息", "SC_基于坐标的空间参照系统", "SC_大地坐标参照系统", "大地坐标参照系统名称"));
            //pKJCZXTNode.Nodes["SC_基于坐标的空间参照系统"].Nodes["SC_大地坐标参照系统"].Nodes.Add("SC_坐标系统", "SC_坐标系统");
            //pKJCZXTNode.Nodes["SC_基于坐标的空间参照系统"].Nodes["SC_大地坐标参照系统"].Nodes["SC_坐标系统"].Nodes.Add("坐标系统类型", "坐标系统类型" + sFlag + XMLHelper.XmlReadValue(doc, "空间参照系统信息", "SC_基于坐标的空间参照系统", "SC_大地坐标参照系统", "SC_坐标系统", "坐标系统类型"));
            //pKJCZXTNode.Nodes["SC_基于坐标的空间参照系统"].Nodes["SC_大地坐标参照系统"].Nodes["SC_坐标系统"].Nodes.Add("坐标系统名称", "坐标系统名称" + sFlag + XMLHelper.XmlReadValue(doc, "空间参照系统信息", "SC_基于坐标的空间参照系统", "SC_大地坐标参照系统", "SC_坐标系统", "坐标系统名称"));
            //pKJCZXTNode.Nodes["SC_基于坐标的空间参照系统"].Nodes["SC_大地坐标参照系统"].Nodes["SC_坐标系统"].Nodes.Add("投影坐标系统参数", "投影坐标系统参数" + sFlag + XMLHelper.XmlReadValue(doc, "空间参照系统信息", "SC_基于坐标的空间参照系统", "SC_大地坐标参照系统", "SC_坐标系统", "投影坐标系统参数"));
            //pKJCZXTNode.Nodes["SC_基于坐标的空间参照系统"].Nodes.Add("SC_垂向坐标参照系统", "SC_垂向坐标参照系统");
            //pKJCZXTNode.Nodes["SC_基于坐标的空间参照系统"].Nodes["SC_垂向坐标参照系统"].Nodes.Add("垂向坐标参照系统名称", "垂向坐标参照系统名称" + sFlag + XMLHelper.XmlReadValue(doc, "空间参照系统信息", "SC_基于坐标的空间参照系统", "SC_垂向坐标参照系统", "垂向坐标参照系统名称"));
            //#endregion
            //#region 内容信息
            //TreeNode pNRNode = new TreeNode("内容信息");
            //this.treeView1.Nodes.Add(pNRNode);

            //TreeNode pNode = new TreeNode("MD_内容描述");
            //pNRNode.Nodes.Add(pNode);

            //List<string> GetFeatureClassList = DatabaseHelper.QueryFeatureClassName(this.m_WS);
            //string sLayerName = "";
            //for (int i = 0; i < GetFeatureClassList.Count; i++)
            //{
            //    sLayerName = GetFeatureClassList[i];
               
            //        AddLayerName2Metadata(pNode, sLayerName);
               
            //}

            //#endregion
            //#region 分发信息
            //TreeNode pFFZLNode = new TreeNode("分发信息");
            //this.treeView1.Nodes.Add(pFFZLNode);
            //pFFZLNode.Nodes.Add("数字传输选项", "数字传输选项");
            //pFFZLNode.Nodes["数字传输选项"].Nodes.Add("在线连接", "在线连接" + sFlag + XMLHelper.XmlReadValue(doc, "分发信息", "数字传输选项", "在线连接"));
            //pFFZLNode.Nodes.Add("分发者", "分发者");
            //pFFZLNode.Nodes["分发者"].Nodes.Add("分发者联系信息", "分发者联系信息");
            //pFFZLNode.Nodes["分发者"].Nodes["分发者联系信息"].Nodes.Add("负责单位名称", "负责单位名称" + sFlag + XMLHelper.XmlReadValue(doc, "分发信息", "分发者", "分发者联系信息", "负责单位名称"));
            //pFFZLNode.Nodes["分发者"].Nodes["分发者联系信息"].Nodes.Add("联系人", "联系人" + sFlag + XMLHelper.XmlReadValue(doc, "分发信息", "分发者", "分发者联系信息", "联系人"));
            //pFFZLNode.Nodes["分发者"].Nodes["分发者联系信息"].Nodes.Add("职责", "职责" + sFlag + XMLHelper.XmlReadValue(doc, "分发信息", "分发者", "分发者联系信息", "职责"));
            //pFFZLNode.Nodes["分发者"].Nodes["分发者联系信息"].Nodes.Add("联系信息", "联系信息");
            //pFFZLNode.Nodes["分发者"].Nodes["分发者联系信息"].Nodes["联系信息"].Nodes.Add("电话", "电话" + sFlag + XMLHelper.XmlReadValue(doc, "分发信息", "分发者", "分发者联系信息", "联系信息", "电话"));
            //pFFZLNode.Nodes["分发者"].Nodes["分发者联系信息"].Nodes["联系信息"].Nodes.Add("传真", "传真" + sFlag + XMLHelper.XmlReadValue(doc, "分发信息", "分发者", "分发者联系信息", "联系信息", "传真"));
            //pFFZLNode.Nodes["分发者"].Nodes["分发者联系信息"].Nodes["联系信息"].Nodes.Add("通信地址", "通信地址" + sFlag + XMLHelper.XmlReadValue(doc, "分发信息", "分发者", "分发者联系信息", "联系信息", "通信地址"));
            //pFFZLNode.Nodes["分发者"].Nodes["分发者联系信息"].Nodes["联系信息"].Nodes.Add("邮政编码", "邮政编码" + sFlag + XMLHelper.XmlReadValue(doc, "分发信息", "分发者", "分发者联系信息", "联系信息", "邮政编码"));
            //pFFZLNode.Nodes["分发者"].Nodes["分发者联系信息"].Nodes["联系信息"].Nodes.Add("电子信箱地址", "电子信箱地址" + sFlag + XMLHelper.XmlReadValue(doc, "分发信息", "分发者", "分发者联系信息", "联系信息", "电子信箱地址"));
            //pFFZLNode.Nodes["分发者"].Nodes["分发者联系信息"].Nodes["联系信息"].Nodes.Add("网址", "网址" + sFlag + XMLHelper.XmlReadValue(doc, "分发信息", "分发者", "分发者联系信息", "联系信息", "网址"));
            //#endregion
            //#region 元数据联系信息
            //TreeNode pMDYSJ = new TreeNode("MD_元数据");
            //this.treeView1.Nodes.Add(pMDYSJ);
            //pMDYSJ.Nodes.Add("日期", "日期" + sFlag + XMLHelper.XmlReadValue(doc, "MD_元数据", "日期"));
            //pMDYSJ.Nodes.Add("联系", "联系");
            //pMDYSJ.Nodes["联系"].Nodes.Add("负责单位名称", "负责单位名称" + sFlag + XMLHelper.XmlReadValue(doc, "MD_元数据", "联系", "负责单位名称"));
            //pMDYSJ.Nodes["联系"].Nodes.Add("联系人", "联系人" + sFlag + XMLHelper.XmlReadValue(doc, "MD_元数据", "联系", "联系人"));
            //pMDYSJ.Nodes["联系"].Nodes.Add("职责", "职责" + sFlag + XMLHelper.XmlReadValue(doc, "MD_元数据", "联系", "职责"));
            //pMDYSJ.Nodes["联系"].Nodes.Add("联系信息", "联系信息");
            //pMDYSJ.Nodes["联系"].Nodes["联系信息"].Nodes.Add("电话", "电话" + sFlag + XMLHelper.XmlReadValue(doc, "MD_元数据", "联系", "联系信息", "电话"));
            //pMDYSJ.Nodes["联系"].Nodes["联系信息"].Nodes.Add("传真", "传真" + sFlag + XMLHelper.XmlReadValue(doc, "MD_元数据", "联系", "联系信息", "传真"));
            //pMDYSJ.Nodes["联系"].Nodes["联系信息"].Nodes.Add("通信地址", "通信地址" + sFlag + XMLHelper.XmlReadValue(doc, "MD_元数据", "联系", "联系信息", "通信地址"));
            //pMDYSJ.Nodes["联系"].Nodes["联系信息"].Nodes.Add("邮政编码", "邮政编码" + sFlag + XMLHelper.XmlReadValue(doc, "MD_元数据", "联系", "联系信息", "邮政编码"));
            //pMDYSJ.Nodes["联系"].Nodes["联系信息"].Nodes.Add("电子信箱地址", "电子信箱地址" + sFlag + XMLHelper.XmlReadValue(doc, "MD_元数据", "联系", "联系信息", "电子信箱地址"));
            //pMDYSJ.Nodes["联系"].Nodes["联系信息"].Nodes.Add("网址", "网址" + sFlag + XMLHelper.XmlReadValue(doc, "MD_元数据", "联系", "联系信息", "网址"));
            //#endregion

        }
        #endregion

        private void AddLayerName2Metadata(TreeNode pNode, string sLayerName)
        {
            IFeatureClass pFc = (this.m_WS as IFeatureWorkspace).OpenFeatureClass(sLayerName);
            if (pFc != null)
            {

                pNode.Nodes.Add("图层名称", "图层名称" + sFlag + pFc.AliasName);
                pNode.Nodes.Add("要素实体名称", "要素实体名称" + sFlag + pFc.FeatureType.ToString());

                string sFieldName = "";
                string sFields = "";
                for (int i = 0; i < pFc.Fields.FieldCount; i++)
                {
                    sFieldName = pFc.Fields.get_Field(i).AliasName;
                    sFields = sFields + sFieldName + "、";
                }
                sFields = sFields.Remove(sFields.Length - 1, 1);
                pNode.Nodes.Add("属性列表", "属性列表" + sFlag + sFields);
            }
        }

        #region 显示元数据信息

        private void DisplayMetaData(XmlDocument doc)
        {

            #region 标识信息
            this.txtSJJ_MC.Text = XMLHelper.XmlReadValue(doc, "标识信息","MD_标识", "数据集引用", "CI_引用","名称");
            this.dESJJ_RQ.Text = XMLHelper.XmlReadValue(doc, "标识信息", "MD_标识", "数据集引用", "CI_引用", "日期");

            this.txtSJJ_YZ.Text = XMLHelper.XmlReadValue(doc, "标识信息", "MD_标识", "语种");
            this.txtSJJ_ZY.Text = XMLHelper.XmlReadValue(doc, "标识信息", "MD_标识", "摘要");
            this.cboSJJ_XZ.Text = XMLHelper.XmlReadValue(doc, "标识信息", "MD_标识", "现状");
            this.textBZ_version.Text=XMLHelper.XmlReadValue(doc,"标识信息", "MD_标识", "版本");

            this.txtDLFW_BW.Text = XMLHelper.XmlReadValue(doc, "标识信息", "MD_标识", "地理范围", "EX_地理坐标范围", "北边纬度");
            this.txtDLFW_DJ.Text = XMLHelper.XmlReadValue(doc, "标识信息", "MD_标识", "地理范围", "EX_地理坐标范围", "东边经度");
            this.txtDLFW_NW.Text = XMLHelper.XmlReadValue(doc, "标识信息", "MD_标识", "地理范围", "EX_地理坐标范围", "南边纬度");
            this.txtDLFW_XJ.Text = XMLHelper.XmlReadValue(doc, "标识信息", "MD_标识", "地理范围", "EX_地理坐标范围", "西边经度");

            this.txtDLMS_DLBSF.Text = XMLHelper.XmlReadValue(doc, "标识信息", "MD_标识", "地理描述", "SI_地理描述", "地理标识符");

            this.dESJFW_QS.Text = XMLHelper.XmlReadValue(doc, "标识信息", "MD_标识", "时间范围", "EX_时间范围", "范围", "TM_时间段", "起始时间");
            this.dESJFW_ZZ.Text = XMLHelper.XmlReadValue(doc, "标识信息", "MD_标识", "时间范围", "EX_时间范围", "范围", "TM_时间段", "终止时间");

            this.txtCXFW_ZD.Text = XMLHelper.XmlReadValue(doc, "标识信息", "MD_标识", "垂向范围", "EX_垂向范围", "最大垂向坐标值");
            this.txtCXFW_ZX.Text = XMLHelper.XmlReadValue(doc, "标识信息", "MD_标识", "垂向范围", "EX_垂向范围", "最小垂向坐标值");
            this.txtCXFW_JLDW.Text = XMLHelper.XmlReadValue(doc, "标识信息", "MD_标识", "垂向范围", "EX_垂向范围", "计量单位");

            this.cboBSFS.Text = XMLHelper.XmlReadValue(doc, "标识信息", "MD_标识", "表示方式");
            this.txtSJGSMC.Text = XMLHelper.XmlReadValue(doc, "标识信息", "MD_标识", "数据格式名称");
            this.txtKJHSJGSMC.Text = XMLHelper.XmlReadValue(doc, "标识信息", "MD_标识", "可交换数据格式名称");
            this.txtDCBLC.Text = XMLHelper.XmlReadValue(doc, "标识信息", "MD_标识", "调查比例尺");
            this.cboLB.Text = XMLHelper.XmlReadValue(doc, "标识信息", "MD_标识", "类别");

            this.txtSJJLX_MC.Text = XMLHelper.XmlReadValue(doc, "标识信息", "MD_标识", "数据集联系信息", "CI_负责单位", "负责单位名称");
            this.txtSJJLX_LXR.Text = XMLHelper.XmlReadValue(doc, "标识信息", "MD_标识", "数据集联系信息", "CI_负责单位", "联系人");
            this.txtSJJLX_ZZ.Text = XMLHelper.XmlReadValue(doc, "标识信息", "MD_标识", "数据集联系信息", "CI_负责单位", "职责");
            this.txtSJJLX_DH.Text = XMLHelper.XmlReadValue(doc, "标识信息", "MD_标识", "数据集联系信息", "CI_负责单位", "联系信息", "CI_联系", "电话");
            this.txtSJJLX_CZ.Text = XMLHelper.XmlReadValue(doc, "标识信息", "MD_标识", "数据集联系信息", "CI_负责单位", "联系信息", "CI_联系", "传真");
            this.txtSJJLX_EMAIL.Text = XMLHelper.XmlReadValue(doc, "标识信息", "MD_标识", "数据集联系信息", "CI_负责单位", "联系信息", "CI_联系", "电子信箱地址");
            this.txtSJJLX_YB.Text = XMLHelper.XmlReadValue(doc, "标识信息", "MD_标识", "数据集联系信息", "CI_负责单位", "联系信息", "CI_联系", "邮政编码");
            this.txtSJJLX_TXDZ.Text = XMLHelper.XmlReadValue(doc, "标识信息", "MD_标识", "数据集联系信息", "CI_负责单位", "联系信息", "CI_联系", "通信地址");
            this.txtSJJLX_WZ.Text = XMLHelper.XmlReadValue(doc, "标识信息", "MD_标识", "数据集联系信息", "CI_负责单位", "联系信息", "CI_联系", "网址");

            this.txtJTLLT_WJMC.Text = XMLHelper.XmlReadValue(doc, "标识信息", "MD_标识", "静态浏览图信息", "MD_浏览图", "文件名称");

            this.cboSJJXZ_SYXZ.Text = XMLHelper.XmlReadValue(doc, "标识信息", "MD_标识", "数据集限制", "MD_数据集限制", "MD_法律限制", "使用限制代码");
            this.cboSJJXZ_AQDJ.Text = XMLHelper.XmlReadValue(doc, "标识信息","MD_标识", "数据集限制", "MD_数据集限制", "MD_安全限制", "安全等级代码");

            this.txtSJJGS_MC.Text = XMLHelper.XmlReadValue(doc, "标识信息", "MD_标识", "数据集格式", "MD_格式", "名称");
            this.txtSJJGS_BB.Text = XMLHelper.XmlReadValue(doc, "标识信息", "MD_标识", "数据集格式", "MD_格式", "版本");

            #endregion
            #region 数据质量信息
          
            this.txtGS_Gaishu.Text = XMLHelper.XmlReadValue(doc, "数据质量信息","DQ_数据质量", "概述");
            this.txtShujuzhi.Text = XMLHelper.XmlReadValue(doc, "数据质量信息", "DQ_数据质量", "数据志");
         
            #endregion

            #region 空间参照系统信息
            this.txtJYDLBS.Text = XMLHelper.XmlReadValue(doc, "空间参照系统信息","RS_参照系统", "SI_基于地理标识的空间参照系统", "名称");
            this.cboDDZB.Text = XMLHelper.XmlReadValue(doc, "空间参照系统信息", "RS_参照系统", "SC_基于坐标的空间参照系统", "SC_大地坐标参照系统", "大地坐标参照系统名称");
            this.cboZBXTLX.Text = XMLHelper.XmlReadValue(doc, "空间参照系统信息", "RS_参照系统", "SC_基于坐标的空间参照系统", "SC_大地坐标参照系统", "SC_坐标系统", "坐标系统类型");
            this.txtZBXTMC.Text = XMLHelper.XmlReadValue(doc, "空间参照系统信息", "RS_参照系统", "SC_基于坐标的空间参照系统", "SC_大地坐标参照系统", "SC_坐标系统", "坐标系统名称");
            this.txtZYJX.Text = XMLHelper.XmlReadValue(doc, "空间参照系统信息", "RS_参照系统", "SC_基于坐标的空间参照系统", "SC_大地坐标参照系统", "SC_坐标系统", "投影坐标系统参数");
            this.cboCXZB.Text = XMLHelper.XmlReadValue(doc, "空间参照系统信息", "RS_参照系统", "SC_基于坐标的空间参照系统", "SC_垂向坐标参照系统", "垂向坐标参照系统名称");

            #endregion

            #region 分发信息
            this.txtFF_ZXLJ.Text = XMLHelper.XmlReadValue(doc, "分发信息","MD_分发", "数字传输选项","MD_数字传输选项", "在线连接");
            this.txtFF_MC.Text = XMLHelper.XmlReadValue(doc, "分发信息", "MD_分发", "分发者","MD_分发者", "分发者联系信息","CI_负责单位", "负责单位名称");
            this.txtFF_LXR.Text = XMLHelper.XmlReadValue(doc, "分发信息", "MD_分发", "分发者", "MD_分发者", "分发者联系信息", "CI_负责单位", "联系人");
            this.cboFF_ZZ.Text = XMLHelper.XmlReadValue(doc, "分发信息", "MD_分发", "分发者", "MD_分发者", "分发者联系信息", "CI_负责单位", "职责");

            this.txtFF_DH.Text = XMLHelper.XmlReadValue(doc, "分发信息", "MD_分发", "分发者", "MD_分发者", "分发者联系信息", "CI_负责单位", "联系信息","CI_联系", "电话");
            this.txtFF_CZ.Text = XMLHelper.XmlReadValue(doc, "分发信息", "MD_分发", "分发者", "MD_分发者", "分发者联系信息", "CI_负责单位", "联系信息", "CI_联系", "传真");
            this.txtFF_TXDZ.Text = XMLHelper.XmlReadValue(doc, "分发信息", "MD_分发", "分发者", "MD_分发者", "分发者联系信息", "CI_负责单位", "联系信息", "CI_联系", "通信地址");
            this.txtFF_YB.Text = XMLHelper.XmlReadValue(doc, "分发信息", "MD_分发", "分发者", "MD_分发者", "分发者联系信息", "CI_负责单位", "联系信息", "CI_联系", "邮政编码");
            this.txtFF_EMAIL.Text = XMLHelper.XmlReadValue(doc, "分发信息", "MD_分发", "分发者", "MD_分发者", "分发者联系信息", "CI_负责单位", "联系信息", "CI_联系", "电子信箱地址");
            this.txtFF_WZ.Text = XMLHelper.XmlReadValue(doc, "分发信息", "MD_分发", "分发者", "MD_分发者", "分发者联系信息", "CI_负责单位", "联系信息", "CI_联系", "网址");
            
            #endregion

            #region MD_元数据
            this.dEYSJ_RQ.Text = XMLHelper.XmlReadValue(doc, "MD_元数据", "日期");
            this.txtYSJ_MC.Text = XMLHelper.XmlReadValue(doc, "MD_元数据", "联系", "CI_负责单位", "负责单位名称");
            this.txtYSJ_LXR.Text = XMLHelper.XmlReadValue(doc, "MD_元数据", "联系", "CI_负责单位", "联系人");
            this.cboYSJ_ZZ.Text = XMLHelper.XmlReadValue(doc, "MD_元数据", "联系", "CI_负责单位", "职责");
            this.txtYSJ_DH.Text = XMLHelper.XmlReadValue(doc, "MD_元数据", "联系", "CI_负责单位", "联系信息","CI_联系", "电话");
            this.txtYSJ_CZ.Text = XMLHelper.XmlReadValue(doc, "MD_元数据", "联系", "CI_负责单位", "联系信息", "CI_联系", "传真");
            this.txtYSJ_TXDZ.Text = XMLHelper.XmlReadValue(doc, "MD_元数据", "联系", "CI_负责单位", "联系信息", "CI_联系", "通信地址");
            this.txtYSJ_YB.Text = XMLHelper.XmlReadValue(doc, "MD_元数据", "联系", "CI_负责单位", "联系信息", "CI_联系", "邮政编码");
            this.txtYSJ_EMAIL.Text = XMLHelper.XmlReadValue(doc, "MD_元数据", "联系", "CI_负责单位", "联系信息", "CI_联系", "电子信箱地址");
            this.txtYSJ_WZ.Text = XMLHelper.XmlReadValue(doc, "MD_元数据", "联系", "CI_负责单位", "联系信息", "CI_联系", "网址");
            #endregion
        }
        #endregion
     
        #region 加载投影数据信息
        private void LoadPrjectionInfo()
        {
            IMap pMap = this.m_MapControl.ActiveView.FocusMap;

          
            ISpatialReference sr = pMap.SpatialReference;
            if (sr != null)
            {               
                string aLine = SpatialRefHelper.FormatSR(sr);
                this.tbSR.Text = aLine;
                if(sr is UnknownCoordinateSystemClass)
                {
                    this.cboDDZB.Text = "未知坐标系";
                    this.txtZBXTMC.Text = "";
                    //this.txtZBXTTYCS.Text = "";
                }
                else if (sr is IProjectedCoordinateSystem)
                {
                    this.cboZBXTLX.Text = "投影坐标系";
                    IProjectedCoordinateSystem prjSR = sr as IProjectedCoordinateSystem;
                    IGeographicCoordinateSystem gcs = prjSR.GeographicCoordinateSystem;
                    if (gcs.Datum.Spheroid.Name == "Xian_1980")
                    {
                        this.cboDDZB.Text = "1980年国家大地坐标系";
                    }
                    else if (gcs.Datum.Spheroid.Name == "Beijing_1954")
                    {
                        this.cboDDZB.Text = "1954年北京坐标系";
                    }
                    else if (gcs.Datum.Spheroid.Name == "CGCS2000")
                    {
                        this.cboDDZB.Text = "国家2000";
                    }
                    if (prjSR.Projection.Name == "Gauss_Kruger")
                    {
                        this.txtZBXTMC.Text = "高斯-克吕格（3度带）";
                    }
                    else
                    {
                        this.txtZBXTMC.Text = prjSR.Projection.Name;
                    }
                    string ss = sr.Name;

                    try
                    {
                        ss = ss.Substring(ss.IndexOf("_") + 1, ss.Length - ss.IndexOf("_") - 1).Trim();
                        ss = ss.Substring(ss.IndexOf("_") + 1, ss.Length - ss.IndexOf("_") - 1).Trim();
                        ss = ss.Substring(0, ss.IndexOf("_")).Trim();
                        //this.txtZBXTTYCS.Text = ss+"度带";
                    }
                    catch { }
                }
                else if (sr is IGeographicCoordinateSystem)
                {
                    this.cboDDZB.Text = "";
                    this.txtZBXTMC.Text = "";
                    //this.txtZBXTTYCS.Text = "";
                }
                else
                {
                    this.cboDDZB.Text = "";
                    this.txtZBXTMC.Text = "";
                   // this.txtZBXTTYCS.Text = "";
                }
            }
        }
        #endregion
       
  
       

        #region 加载图层信息
        private void LoadMapLayerInfo()
        {
           
            this.listView1.Items.Clear();
            if (this.m_WS == null) return;
            try
            {
                IWorkspace m_space = this.m_WS;

                IFeatureWorkspace pFWS = m_space as IFeatureWorkspace;

                IDataset pDateset = pFWS.OpenFeatureDataset(RCIS.Global.AppParameters.DATASET_DEFAULT_NAME);

                if (pDateset != null)
                {
                    IFeatureClassContainer pFeatureClsConner = (IFeatureClassContainer)pDateset;
                    
                    for (int i = 0; i < pFeatureClsConner.ClassCount; i++)
                    {
                        IFeatureClass pFeauteClass = pFeatureClsConner.get_Class(i);
                        ListViewItem lv = new ListViewItem();
                        lv.SubItems.Add(pFeauteClass.AliasName);
                        lv.SubItems.Add(this.GetFeatureLayerTableName(pFeauteClass));
                        lv.SubItems.Add(pFeauteClass.ShapeType.ToString());
                        lv.SubItems.Add(pFeauteClass.FeatureType.ToString());
                        lv.SubItems.Add(pFeauteClass.FeatureCount(null).ToString());

                        ITopologyClass pTopClass = (ITopologyClass)pFeauteClass;
                        bool bb = pTopClass.IsInTopology;
                        lv.SubItems.Add(bb.ToString());
                        if (bb == true)
                        {
                            lv.SubItems.Add(pTopClass.XYRank.ToString());
                            lv.SubItems.Add(pTopClass.ZRank.ToString());
                            lv.SubItems.Add(pTopClass.Weight.ToString());

                        }
                        else
                        {
                            lv.SubItems.Add("null");
                            lv.SubItems.Add("null");
                            lv.SubItems.Add("null");

                        }
                        lv.SubItems.Add(GetYSDM(pFeauteClass));
                        lv.SubItems.Add(GetYSMC(GetYSDM(pFeauteClass)));

                        if (i % 2 == 0)
                        {
                            lv.BackColor = Color.Wheat;
                        }
                        this.listView1.Items.Add(lv);
                    }

                }
            }
            catch (Exception ex) { }
        }
        //获得要素代码
        private string GetYSDM(IFeatureClass pFC)
        {
            if (pFC == null) return "无数据";
            //获取所有要素代码
            Dictionary<string, string> dicClassYsdm = new Dictionary<string, string>();
            try
            {
                DataTable dt = RCIS.Database.LS_SetupMDBHelper.GetDataTable("select * from SYS_YSDM ", "ysdm");
                foreach (DataRow dr in dt.Rows)
                {
                    dicClassYsdm.Add(dr["CLASSNAME"].ToString(), dr["YSDM"].ToString());
                }
                
                IDataset pDS = pFC as IDataset;
                if (dicClassYsdm.ContainsKey(pDS.Name.ToUpper()))
                {
                    return dicClassYsdm[pDS.Name.ToUpper()];
                }
                else
                {
                    return "2000000000";
                }
            }
            catch { return ""; }
        }
        //获得要素名称
        private string GetYSMC(string sYSDM)
        {
            Dictionary<string, string> dicClassYsdm = new Dictionary<string, string>();
            try{
                
                DataTable dt = RCIS.Database.LS_SetupMDBHelper.GetDataTable("select * from SYS_YSDM ", "ysdm");
                foreach (DataRow dr in dt.Rows)
                {
                    dicClassYsdm.Add(dr["YSDM"].ToString(), dr["CLASSNAME"].ToString());
                }
                if (dicClassYsdm.ContainsKey(sYSDM))
                {
                    return dicClassYsdm[sYSDM];
                }
                else return "";
            }
            catch (Exception ex)
            {
                return "";
            }
            

        }
        #endregion
        

        #region 根据传给的FeatureLayer获得该英文表名
        /// <summary>
        /// 根据地类图斑－－－获得“DLTB”
        /// </summary>
        /// <param name="featLayer"></param>
        /// <returns></returns>
        private string GetFeatureLayerTableName(IFeatureClass fc)
        {
            try
            {

                string shortName = "";
                shortName = (fc as IDataset).Name;
                int index = shortName.LastIndexOf(".");
                if (index >= 0)
                {
                    shortName = shortName.Substring(index + 1);
                }
                return shortName;
            }
            catch (Exception ee)
            {

                MessageBox.Show("" + ee.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return "";
            }
        }

        #endregion
        #region 坐标转换为经纬度
        private void XY2JYD()
        {
            //try
            //{
               
            //    IEnvelope pEnv = Helper.OtherHelper.pLayerObject.ESRILayerObject.AreaOfInterest;

               
            //    double xMin = pEnv.XMin;
            //    double xMax = pEnv.XMax;
            //    double yMin = pEnv.YMin;
            //    double yMax = pEnv.YMax;
              
            //    IPoint pMinPoint = new PointClass();
            //    pMinPoint.PutCoords(xMin, yMin);

            //    IPoint jwPt = GeometryHelper.XY2JYD(this.m_MapControl.Map
            //             , pMinPoint);
            //    if (jwPt != null && !jwPt.IsEmpty)
            //    {
            //        this.txtDLFW_XJ.Text = GeometryHelper.FormatDFM(jwPt.X);
            //        this.txtDLFW_NW.Text = GeometryHelper.FormatDFM(jwPt.Y);

            //    }
            //    else
            //    {
            //        this.txtDLFW_XJ.Text = "未知坐标系";
            //        this.txtDLFW_NW.Text = "未知坐标系";

            //    }

            //    IPoint pMaxPoint = new PointClass();
            //    pMaxPoint.PutCoords(xMax, yMax);
            //    jwPt = GeometryHelper.XY2JYD(this.m_MapControl.Map
            //            , pMaxPoint);
            //    if (jwPt != null && !jwPt.IsEmpty)
            //    {
            //        this.txtDLFW_DJ.Text = GeometryHelper.FormatDFM(jwPt.X);
            //        this.txtDLFW_BW.Text = GeometryHelper.FormatDFM(jwPt.Y);

            //    }
            //    else
            //    {
            //        this.txtDLFW_DJ.Text = "未知坐标系";
            //        this.txtDLFW_BW.Text = "未知坐标系";

            //    }


            //    if ( pEnv.ZMax.ToString()=="非数字")
            //    {
            //        this.txtCXFW_ZD.Text = "无垂向信息";
            //    }
            //    else
            //    {
            //        this.txtCXFW_ZD.Text = pEnv.ZMax.ToString();
            //    }
            //    if (pEnv.ZMin.ToString() == "非数字")
            //    {
            //        this.txtCXFW_ZX.Text = "无垂向信息";
            //    }
            //    else
            //    {
            //        this.txtCXFW_ZX.Text = pEnv.ZMin.ToString();
            //    }
               
            //}
            //catch (Exception ex)
            //{ }
        }
        #endregion

        //显示
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            
            
            string sfile = sMetaDataFile;
            this.Display(sfile);
        }
        //导出
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (this.txtSJJ_MC.Text.Trim() == "") return;
            SaveFileDialog ofd = new SaveFileDialog();
            string aFilterStr = "元数据模式文件(*.xml)|*.xml";

            ofd.Filter = aFilterStr;
        
            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                try
                {

                    File.Copy(Application.StartupPath + @"\template\metadata_tysd.xml", ofd.FileName);
                    //XmlDocument doc = new XmlDocument();
                    //string sXMLPath = ofd.FileName;
                    //doc.Load(sXMLPath);
                    MessageBox.Show("导出成功","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
                catch (Exception ex)
                { }
            }
        }
        //更新
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (this.txtSJJ_MC.Text.Trim() == "") return;
            DialogResult dr = MessageBox.Show("是否需要进行元数据信息修改更新，\n该功能只更新元数据属性信息，\n图形信息不能修改。"
            , "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (DialogResult.Yes == dr)
            {
                //DataOperateHelper.Connstring = "Provider=Microsoft.JET.OLEDB.4.0;data source=" + Application.StartupPath + "\\data\\MetaData.mdb";

                XmlDocument doc = new XmlDocument();
                doc.Load(sMetaDataFile);

                string sXMLPath = sMetaDataFile;
                #region 信息保存
                XMLHelper.XmlWriteValue(doc, sXMLPath, "标识信息","MD_标识", "数据集引用", "CI_引用", "名称", this.txtSJJ_MC.Text.Trim());
                XMLHelper.XmlWriteValue(doc, sXMLPath, "标识信息", "MD_标识", "数据集引用", "CI_引用", "日期", this.dESJJ_RQ.Text.Trim());
                XMLHelper.XmlWriteValue(doc, sXMLPath, "标识信息", "MD_标识", "数据集引用", "CI_引用", "版本", this.textBZ_version.Text.Trim());

                XMLHelper.XmlWriteValue(doc, sXMLPath, "标识信息", "MD_标识", "语种", this.txtSJJ_YZ.Text.Trim());
                XMLHelper.XmlWriteValue(doc, sXMLPath, "标识信息", "MD_标识", "摘要", this.txtSJJ_ZY.Text.Trim());
                XMLHelper.XmlWriteValue(doc, sXMLPath, "标识信息", "MD_标识", "现状", this.cboSJJ_XZ.Text.Trim());

                XMLHelper.XmlWriteValue(doc, sXMLPath, "标识信息", "MD_标识", "地理范围", "EX_地理坐标范围", "西边经度", this.txtDLFW_XJ.Text.Trim());
                XMLHelper.XmlWriteValue(doc, sXMLPath, "标识信息", "MD_标识", "地理范围", "EX_地理坐标范围", "东边经度", this.txtDLFW_DJ.Text.Trim());
                XMLHelper.XmlWriteValue(doc, sXMLPath, "标识信息", "MD_标识", "地理范围", "EX_地理坐标范围", "南边纬度", this.txtDLFW_NW.Text.Trim());
                XMLHelper.XmlWriteValue(doc, sXMLPath, "标识信息", "MD_标识", "地理范围", "EX_地理坐标范围", "北边纬度", this.txtDLFW_BW.Text.Trim());

                XMLHelper.XmlWriteValue(doc, sXMLPath, "标识信息", "MD_标识", "地理描述", "SI_地理描述", "地理标识符", this.txtDLMS_DLBSF.Text.Trim());

                XMLHelper.XmlWriteValue(doc, sXMLPath, "标识信息", "MD_标识", "时间范围", "EX_时间范围", "范围", "TM_时间段", "起始时间", this.dESJFW_QS.Text.Trim());
                XMLHelper.XmlWriteValue(doc, sXMLPath, "标识信息", "MD_标识", "时间范围", "EX_时间范围", "范围", "TM_时间段", "终止时间", this.dESJFW_ZZ.Text.Trim());

                XMLHelper.XmlWriteValue(doc, sXMLPath, "标识信息", "MD_标识", "垂向范围", "EX_垂向范围", "最小垂向坐标值", this.txtCXFW_ZX.Text.Trim());
                XMLHelper.XmlWriteValue(doc, sXMLPath, "标识信息", "MD_标识", "垂向范围", "EX_垂向范围", "最大垂向坐标值", this.txtCXFW_ZD.Text.Trim());
                XMLHelper.XmlWriteValue(doc, sXMLPath, "标识信息", "MD_标识", "垂向范围", "EX_垂向范围", "计量单位", this.txtCXFW_JLDW.Text.Trim());



                XMLHelper.XmlWriteValue(doc, sXMLPath, "标识信息", "MD_标识", "表示方式", this.cboBSFS.Text.Trim());
                XMLHelper.XmlWriteValue(doc, sXMLPath, "标识信息", "MD_标识", "空间分辨率", "2");
                XMLHelper.XmlWriteValue(doc, sXMLPath, "标识信息", "MD_标识", "数据格式名称", this.txtSJGSMC.Text.Trim());
                XMLHelper.XmlWriteValue(doc, sXMLPath, "标识信息", "MD_标识", "可交换数据格式名称", this.txtKJHSJGSMC.Text.Trim());
                XMLHelper.XmlWriteValue(doc, sXMLPath, "标识信息", "MD_标识", "调查比例尺", this.txtDCBLC.Text.Trim());
                XMLHelper.XmlWriteValue(doc, sXMLPath, "标识信息", "MD_标识", "类别", this.cboLB.Text.Trim());
                XMLHelper.XmlWriteValue(doc, sXMLPath, "标识信息", "MD_标识", "卫星轨道标识", "北斗");

                XMLHelper.XmlWriteValue(doc, sXMLPath, "标识信息", "MD_标识", "数据集联系信息", "CI_负责单位", "负责单位名称", this.txtSJJGS_MC.Text.Trim());
                XMLHelper.XmlWriteValue(doc, sXMLPath, "标识信息", "MD_标识", "数据集联系信息", "CI_负责单位", "联系人", this.txtSJJLX_LXR.Text.Trim());
                XMLHelper.XmlWriteValue(doc, sXMLPath, "标识信息", "MD_标识", "数据集联系信息", "CI_负责单位", "职责", this.txtSJJLX_ZZ.Text.Trim());


                XMLHelper.XmlWriteValue(doc, sXMLPath, "标识信息", "MD_标识", "数据集联系信息", "CI_负责单位", "联系信息", "CI_联系", "电话", this.txtSJJLX_DH.Text.Trim());
                XMLHelper.XmlWriteValue(doc, sXMLPath, "标识信息", "MD_标识", "数据集联系信息", "CI_负责单位", "联系信息", "CI_联系", "传真", this.txtSJJLX_CZ.Text.Trim());
                XMLHelper.XmlWriteValue(doc, sXMLPath, "标识信息", "MD_标识", "数据集联系信息", "CI_负责单位", "联系信息", "CI_联系", "通信地址", this.txtSJJLX_TXDZ.Text.Trim());
                XMLHelper.XmlWriteValue(doc, sXMLPath, "标识信息", "MD_标识", "数据集联系信息", "CI_负责单位", "联系信息", "CI_联系", "邮政编码", this.txtSJJLX_YB.Text.Trim());
                XMLHelper.XmlWriteValue(doc, sXMLPath, "标识信息", "MD_标识", "数据集联系信息", "CI_负责单位", "联系信息", "CI_联系", "电子信箱地址", this.txtSJJLX_EMAIL.Text.Trim());
                XMLHelper.XmlWriteValue(doc, sXMLPath, "标识信息", "MD_标识", "数据集联系信息", "CI_负责单位", "联系信息", "CI_联系", "网址", this.txtSJJLX_WZ.Text.Trim());


                XMLHelper.XmlWriteValue(doc, sXMLPath, "标识信息", "MD_标识", "静态浏览图信息", "MD_浏览图", "文件名称", this.txtJTLLT_WJMC.Text.Trim());
                XMLHelper.XmlWriteValue(doc, sXMLPath, "标识信息", "MD_标识", "数据集限制", "MD_数据集限制", "MD_法律限制", "使用限制代码", this.cboSJJXZ_SYXZ.Text.Trim());
                XMLHelper.XmlWriteValue(doc, sXMLPath, "标识信息", "MD_标识", "数据集限制", "MD_数据集限制", "MD_安全限制", "安全等级代码", this.cboSJJXZ_AQDJ.Text.Trim());
                XMLHelper.XmlWriteValue(doc, sXMLPath, "标识信息", "MD_标识", "数据集格式", "MD_格式", "名称", this.txtSJJGS_MC.Text.Trim());
                XMLHelper.XmlWriteValue(doc, sXMLPath, "标识信息", "MD_标识", "数据集格式", "MD_格式", "版本", this.txtSJJGS_BB.Text.Trim());

                XMLHelper.XmlWriteValue(doc, sXMLPath, "数据质量信息", "DQ_数据质量", "概述",  this.txtGS_Gaishu.Text.Trim());

                XMLHelper.XmlWriteValue(doc, sXMLPath, "数据质量信息", "DQ_数据质量", "数据志", this.txtShujuzhi.Text.Trim());
                
                XMLHelper.XmlWriteValue(doc, sXMLPath, "空间参照系统信息","RS_参照系统", "SI_基于地理标识的空间参照系统", "名称", this.txtJYDLBS.Text.Trim());
                XMLHelper.XmlWriteValue(doc, sXMLPath, "空间参照系统信息","RS_参照系统", "SC_基于坐标的空间参照系统", "SC_大地坐标参照系统", "大地坐标参照系统名称", this.cboDDZB.Text.Trim());
                XMLHelper.XmlWriteValue(doc, sXMLPath, "空间参照系统信息", "RS_参照系统", "SC_基于坐标的空间参照系统", "SC_大地坐标参照系统", "SC_坐标系统", "坐标系统类型", this.cboZBXTLX.Text.Trim());
                XMLHelper.XmlWriteValue(doc, sXMLPath, "空间参照系统信息", "RS_参照系统", "SC_基于坐标的空间参照系统", "SC_大地坐标参照系统", "SC_坐标系统", "坐标系统名称", this.txtZBXTMC.Text.Trim());
                XMLHelper.XmlWriteValue(doc, sXMLPath, "空间参照系统信息", "RS_参照系统", "SC_基于坐标的空间参照系统", "SC_大地坐标参照系统", "SC_坐标系统", "投影坐标系统参数", this.txtZYJX.Text.Trim());
                XMLHelper.XmlWriteValue(doc, sXMLPath, "空间参照系统信息", "RS_参照系统", "SC_垂向坐标参照系统", "垂向坐标参照系统名称", this.cboCXZB.Text.Trim());


                XMLHelper.XmlWriteValue(doc, sXMLPath, "分发信息","MD_分发", "数字传输选项","MD_数字传输选项", "在线连接", this.txtFF_ZXLJ.Text.Trim());

                XMLHelper.XmlWriteValue(doc, sXMLPath, "分发信息","MD_分发" ,"分发者","MD_分发者", "分发者联系信息","CI_负责单位", "负责单位名称", this.txtFF_MC.Text.Trim());
                XMLHelper.XmlWriteValue(doc, sXMLPath, "分发信息", "MD_分发", "分发者", "MD_分发者", "分发者联系信息", "CI_负责单位", "联系人", this.txtFF_LXR.Text.Trim());
                XMLHelper.XmlWriteValue(doc, sXMLPath, "分发信息", "MD_分发", "分发者", "MD_分发者", "分发者联系信息", "CI_负责单位", "职责", this.cboFF_ZZ.Text.Trim());


                XMLHelper.XmlWriteValue(doc, sXMLPath, "分发信息", "MD_分发", "分发者", "MD_分发者", "分发者联系信息", "CI_负责单位", "联系信息","CI_联系", "电话", this.txtFF_DH.Text.Trim());
                XMLHelper.XmlWriteValue(doc, sXMLPath, "分发信息", "MD_分发", "分发者", "MD_分发者", "分发者联系信息", "CI_负责单位", "联系信息", "CI_联系", "传真", this.txtFF_CZ.Text.Trim());
                XMLHelper.XmlWriteValue(doc, sXMLPath, "分发信息", "MD_分发", "分发者", "MD_分发者", "分发者联系信息", "CI_负责单位", "联系信息", "CI_联系", "通信地址", this.txtFF_TXDZ.Text.Trim());
                XMLHelper.XmlWriteValue(doc, sXMLPath, "分发信息", "MD_分发", "分发者", "MD_分发者", "分发者联系信息", "CI_负责单位", "联系信息", "CI_联系", "邮政编码", this.txtFF_YB.Text.Trim());
                XMLHelper.XmlWriteValue(doc, sXMLPath, "分发信息", "MD_分发", "分发者", "MD_分发者", "分发者联系信息", "CI_负责单位", "联系信息", "CI_联系", "电子信箱地址", this.txtFF_EMAIL.Text.Trim());
                XMLHelper.XmlWriteValue(doc, sXMLPath, "分发信息", "MD_分发", "分发者", "MD_分发者", "分发者联系信息", "CI_负责单位", "联系信息", "CI_联系", "网址", this.txtFF_WZ.Text.Trim());

                XMLHelper.XmlWriteValue(doc, sXMLPath, "MD_元数据", "日期", this.dEYSJ_RQ.Text);
                XMLHelper.XmlWriteValue(doc, sXMLPath, "MD_元数据", "联系","CI_负责单位", "负责单位名称", this.txtYSJ_MC.Text.Trim());
                XMLHelper.XmlWriteValue(doc, sXMLPath, "MD_元数据", "联系", "CI_负责单位", "联系人", this.txtYSJ_LXR.Text.Trim());
                XMLHelper.XmlWriteValue(doc, sXMLPath, "MD_元数据", "联系", "CI_负责单位", "职责", this.cboYSJ_ZZ.Text.Trim());

                XMLHelper.XmlWriteValue(doc, sXMLPath, "MD_元数据", "联系", "CI_负责单位", "联系信息", "CI_联系", "电话", this.txtYSJ_DH.Text.Trim());
                XMLHelper.XmlWriteValue(doc, sXMLPath, "MD_元数据", "联系", "CI_负责单位", "联系信息", "CI_联系", "传真", this.txtYSJ_CZ.Text.Trim());
                XMLHelper.XmlWriteValue(doc, sXMLPath, "MD_元数据", "联系", "CI_负责单位", "联系信息", "CI_联系", "通信地址", this.txtYSJ_TXDZ.Text.Trim());
                XMLHelper.XmlWriteValue(doc, sXMLPath, "MD_元数据", "联系", "CI_负责单位", "联系信息", "CI_联系", "邮政编码", this.txtYSJ_YB.Text.Trim());
                XMLHelper.XmlWriteValue(doc, sXMLPath, "MD_元数据", "联系", "CI_负责单位", "联系信息", "CI_联系", "电子信箱地址", this.txtYSJ_EMAIL.Text.Trim());
                XMLHelper.XmlWriteValue(doc, sXMLPath, "MD_元数据", "联系", "CI_负责单位", "联系信息", "CI_联系", "网址", this.txtYSJ_WZ.Text.Trim());
                #endregion

                MessageBox.Show("更新成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);          

            }

        }
        //private string currentTab="";
        public string strConn = "";
        public string strConfFile = Application.StartupPath + @"\data\allTables.xml";

        protected Hashtable fieldTypes = new Hashtable();
        protected void FillHashTab()  //设置字典，类型与中文类型名成的对应
        {

            fieldTypes.Add("VARCHAR2", "文本型");
            fieldTypes.Add("NVARCHAR2", "文本型");
            fieldTypes.Add("NUMBER", "数值型");
            fieldTypes.Add("FLOAT", "浮点型");
            fieldTypes.Add("LONG", "整型");
            fieldTypes.Add("DATETIME", "日期型");
            fieldTypes.Add("DATE", "日期型");

        }
      
        //加载数据
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "元数据文件(*.xml)|*.xml";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Clear();
                string sFile = ofd.FileName;
                this.Display(sFile);
            }

        }
        private void Clear()
        {
            this.listView1.Items.Clear();
            this.tbSR.Text = "";
            //this.memoEdit1.Text = "";
            //this.treeView1.Nodes.Clear();
            #region 标识信息
            this.txtSJJ_MC.Text = "";
            this.dESJJ_RQ.Text = "";
            this.txtSJJ_YZ.Text = "";
            this.txtSJJ_ZY.Text = "";
            this.cboSJJ_XZ.Text = "";

            this.txtDLFW_BW.Text = "";
            this.txtDLFW_DJ.Text = "";
            this.txtDLFW_NW.Text = "";
            this.txtDLFW_XJ.Text = "";

            this.txtDLMS_DLBSF.Text = "";

            this.dESJFW_QS.Text = "";
            this.dESJFW_ZZ.Text = "";

            this.txtCXFW_ZD.Text = "";
            this.txtCXFW_ZX.Text = "";
            this.txtCXFW_JLDW.Text = "";

            this.cboBSFS.Text = "";
            this.txtSJGSMC.Text = "";
            this.txtKJHSJGSMC.Text = "";
            this.txtDCBLC.Text = "";
            this.cboLB.Text = "";

            this.txtSJJLX_MC.Text = "";
            this.txtSJJLX_LXR.Text = "";
            this.txtSJJLX_ZZ.Text = "";
            this.txtSJJLX_DH.Text = "";
            this.txtSJJLX_CZ.Text = "";
            this.txtSJJLX_EMAIL.Text = "";
            this.txtSJJLX_YB.Text = "";
            this.txtSJJLX_TXDZ.Text = "";
            this.txtSJJLX_WZ.Text = "";

            this.txtJTLLT_WJMC.Text = "";

            this.cboSJJXZ_SYXZ.Text = "";
            this.cboSJJXZ_AQDJ.Text = "";

            this.txtSJJGS_MC.Text = "";
            this.txtSJJGS_BB.Text = "";
            #endregion
            #region 数据质量信息
           
            this.txtGS_Gaishu.Text = "";
            this.txtShujuzhi.Text = "";
          
            #endregion

            #region 空间参照系统信息
            this.txtJYDLBS.Text = "";
            this.cboDDZB.Text = "";
            this.cboZBXTLX.Text = "";
            this.txtZBXTMC.Text = "";
            this.txtZYJX.Text = "";
            this.cboCXZB.Text = "";

            #endregion

            #region 分发信息
            this.txtFF_ZXLJ.Text = "";
            this.txtFF_MC.Text = "";
            this.txtFF_LXR.Text = "";
            this.cboFF_ZZ.Text = "";
            this.txtFF_CZ.Text = "";
            this.txtFF_TXDZ.Text = "";
            this.txtFF_YB.Text = "";
            this.txtFF_EMAIL.Text = "";
            this.txtFF_WZ.Text = "";
            this.txtFF_DH.Text = "";
            #endregion

            #region MD_元数据
            this.dEYSJ_RQ.Text = "";
            this.txtYSJ_MC.Text = "";
            this.txtYSJ_LXR.Text = "";
            this.cboYSJ_ZZ.Text = "";
            this.txtYSJ_DH.Text = "";
            this.txtYSJ_CZ.Text = "";
            this.txtYSJ_TXDZ.Text = "";
            this.txtYSJ_YB.Text = "";
            this.txtYSJ_EMAIL.Text = "";
            this.txtYSJ_WZ.Text = "";
            #endregion
        }

        //private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        //{
        //    try
        //    {
        //        this.memoEdit1.Text = this.treeView1.SelectedNode.Text;
        //    }
        //    catch { }
        //}

        //private void ccmdExpand_Click(object sender, EventArgs e)
        //{
        //    this.treeView1.ExpandAll();
        //}

        //private void cmdCollapse_Click(object sender, EventArgs e)
        //{
        //    this.treeView1.CollapseAll();
        //}

        

        private void tvAttrExt_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            if (this.txtSJJ_MC.Text.Trim() == "") return;
            SaveFileDialog ofd = new SaveFileDialog();
            string aFilterStr = "元数据模式文件(*.xml)|*.xml";

            ofd.Filter = aFilterStr;

            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                try
                {

                    File.Copy(sMetaDataFile, ofd.FileName);
                    //XmlDocument doc = new XmlDocument();
                    //string sXMLPath = ofd.FileName;
                    //doc.Load(sXMLPath);
                    MessageBox.Show("导出成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                { }
            }
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (toolStripComboBox1.SelectedIndex)
            {
                case 0:
                    sMetaDataFile = Application.StartupPath + @"\template\metadata.xml";
                    break;
                case 1:
                    sMetaDataFile = Application.StartupPath + @"\template\metadata_tysd.xml";
                    break;
                default:
                    sMetaDataFile = Application.StartupPath + @"\template\metadata.xml";
                    break;
            }
        }

        
    }
}
