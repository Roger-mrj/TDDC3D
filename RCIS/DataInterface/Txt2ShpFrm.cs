using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Geodatabase;

using RCIS.GISCommon;
using RCIS.Utility;

namespace RCIS.DataInterface
{
    public partial class Txt2ShpFrm : Form
    {
        public Txt2ShpFrm()
        {
            InitializeComponent();
        }

        private esriGeometryType mGT;
        //private IGeometry m_geometry;

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void beDestFolder_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "SHP文件|*.shp";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                this.beDestFolder.Text = dlg.FileName;               
            }

        }


        IWorkspace  pDestWorkspace=null;
        private IFeatureClass CreateShpFile( IFeatureWorkspace pFWS,string   outfileNamePath,ISpatialReference pSR)
        {           
           
            //创建结构
            IFields pFields = new FieldsClass();
            IFieldsEdit pFieldsEdit;
            pFieldsEdit = (IFieldsEdit)pFields;

            IField pField = new FieldClass();
            IFieldEdit pFieldEdit = (IFieldEdit)pField;
            pFieldEdit.Name_2 = "Shape";
            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;
            IGeometryDef pGeometryDef = new GeometryDefClass();
            IGeometryDefEdit pGDefEdit = (IGeometryDefEdit)pGeometryDef;
            pGDefEdit.GeometryType_2 = mGT;  //类型
            pGDefEdit.SpatialReference_2 = pSR;

            pFieldEdit.GeometryDef_2 = pGeometryDef;
            pFieldsEdit.AddField(pField);

            pField = new FieldClass();
            pFieldEdit = (IFieldEdit)pField;
            pFieldEdit.Length_2 = 50;
            pFieldEdit.Name_2 = "BH";
            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            pFieldsEdit.AddField(pField);

            outfileNamePath = System.IO.Path.GetFileName(outfileNamePath);
            IFeatureClass  pFeatureClass =
                    pFWS.CreateFeatureClass(outfileNamePath, pFields, null, null, esriFeatureType.esriFTSimple, "Shape", "");
            return pFeatureClass;
        }


        private IGeometry GetAGeom( List<IPoint> lstPoints)
        {
            object missing1 = Type.Missing;
            if (rbPt.Checked)
            {
                return lstPoints[0] as IGeometry ;//返回第一个点
            }
            else if (rbLine.Checked)
            {
                IPointCollection pGonCol1 = new Polyline();
                foreach (IPoint aPt in lstPoints)
                {
                    pGonCol1.AddPoint(aPt, ref missing1, ref missing1);
                }
                return pGonCol1 as IPolyline;
            }
            else if (rbPoly.Checked)
            {
                IPointCollection pGonCol1 = new PolygonClass(); //拓扑正确的图形   
                foreach (IPoint aPt in lstPoints)
                {
                    pGonCol1.AddPoint(aPt, ref missing1, ref missing1);
                }
                ITopologicalOperator pToper = pGonCol1 as ITopologicalOperator;
                pToper.Simplify();
                IPolygon pPolygon = pGonCol1 as IPolygon;
                return pPolygon as IGeometry;
            }
            return null;

        }

        private IPoint GetAPoint(string s)
        {
            string[] sXy = s.Split(',');
            if (sXy.Length < 2)
                return null;
            IPoint aPt = new PointClass();
            double zbx,zby = 0;
            double.TryParse(sXy[0], out zbx);
            double.TryParse(sXy[1], out zby);
            aPt.X = zbx;
            aPt.Y = zby;
            return aPt;
        }

        private Dictionary<string, IGeometry> ParseGeoTxt()
        {
            Dictionary<string, IGeometry> resultDic = new Dictionary<string, IGeometry>();
            string sTmp = "";
            for (int i = 0; i < this.txtCotents.Lines.Length; i++)
            {
                string s = this.txtCotents.Lines[i].Trim();
                if (s == "")
                {
                    //解析stmp
                    if (sTmp != "")
                    {
                        string[] aContent=sTmp.Split("\r\n".ToCharArray());
                        if (aContent.Length > 1)
                        {
                            string bh=aContent[0];
                            List<IPoint> lts = new List<IPoint>();
                            for (int j = 1; j < aContent.Length; j++)
                            {
                                IPoint aPt = GetAPoint(aContent[j]);
                                if (aPt != null)
                                {
                                    lts.Add(aPt);
                                }
                            }
                            IGeometry geo = GetAGeom(lts);

                            if (geo != null)
                            {
                                resultDic.Add(bh, geo);
                            }                            
                        }

                    }
                    sTmp = "";
                    continue;
                }
                sTmp += s+"\r\n";

            }
            return resultDic;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {            
              //创建 shp, 带一个编号，名称字段，
            #region 判断条件
            string folder = beDestFolder.Text;
            if (folder.Trim() == "")
                return;


            if (this.txtCotents.Text.Trim() == "")
                return;

            if (beSrcFile.Text.Trim() == "")
                return;

            if (rbPt.Checked)
            {
                mGT = esriGeometryType.esriGeometryPoint;
            }
            else if (rbLine.Checked)
            {
                mGT = esriGeometryType.esriGeometryPolyline;
            }
            else if (rbPoly.Checked)
            {
                mGT = esriGeometryType.esriGeometryPolygon;
            }

            #endregion 

            string fullNamePath = this.beDestFolder.Text;
            string outFileName = System.IO.Path.GetFileName(fullNamePath);
            Dictionary<string, IGeometry> dic = ParseGeoTxt();
            if (dic.Count == 0)
            {
                MessageBox.Show("当前文件内容为空！", "确定", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            IWorkspaceFactory pWSF = new ShapefileWorkspaceFactoryClass();
            pDestWorkspace = pWSF.OpenFromFile(System.IO.Path.GetDirectoryName(fullNamePath), 0);
                      

            IFeatureWorkspace pFWS = (IFeatureWorkspace)pDestWorkspace;
            //如果shapefile存在替换它
            if (System.IO.File.Exists(fullNamePath))
            {
                IFeatureClass featureClass = pFWS.OpenFeatureClass(outFileName);
                IDataset pDataset = (IDataset)featureClass;
                pDataset.Delete();

            }

            ISpatialReference pSR = SpatialRefHelper.ConstructCoordinateSystem(true, this.beSpatialTxt.Text);
            IFeatureClass destClass = CreateShpFile(pFWS, fullNamePath,pSR);            //读取解析 文件
            IWorkspaceEdit pWsEdit = pDestWorkspace as IWorkspaceEdit;
            pWsEdit.StartEditing(true);
            pWsEdit.StartEditOperation();
            try{
                
                int iCount=0;                
                foreach(KeyValuePair<string,IGeometry> aItem in dic)
                {
                    lblstatus.Text="正在写入第"+iCount+"个要素";
                    lblstatus.Update();

                    IFeature aFea=destClass.CreateFeature();
                    FeatureHelper.SetFeatureValue(aFea,"BH",aItem.Key.ToString() );
                    aFea.Shape = aItem.Value;
                    aFea.Store();

                }
                  
                lblstatus.Text="写入完成！";
                MessageBox.Show("写入完成","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                pWsEdit.StopEditOperation();
                pWsEdit.StopEditing(true);
            }

            

            //逐个要素生成

        }

        private void beSrcFile_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "点数据文件(*.*)|*.*";
            ofd.CheckFileExists = true;
            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                this.beSrcFile.Text = ofd.FileName;
                this.txtCotents.Text = "";
                using (StreamReader sr=new StreamReader(ofd.FileName))
                {

                    string line = sr.ReadLine();
                    while (line != null)
                    {
                        txtCotents.AppendText(line + "\r\n");
                        line = sr.ReadLine();
                    }

                }
            }



        }

        private void Txt2ShpFrm_Load(object sender, EventArgs e)
        {

        }

        private void beSpatialTxt_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = Application.StartupPath + @"\srprj\CGCS2000";
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            this.beSpatialTxt.Text = dlg.FileName;
            
            
        }



    }
}
