using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

using DevExpress.XtraBars;

using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;

using RCIS.Style.StyleEditor;
using RCIS.Helper;
using RCIS.Style;

namespace LsEditor
{
    public partial class frmMain : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public frmMain()
        {
            

            InitializeComponent();
        }


        public ISymbol pCurrentSym = null;

        private void RefreshData(string  tabname )
        {
            try
            {
                DataSet ds = DataOperateHelper.getDataSet("select * from [" + tabname + "] ", tabname);
                DataTable dt = ds.Tables[0];

                this.gridControl1.DataSource = ds;
                this.gridControl1.DataMember = dt.TableName;

                if (dt.Rows.Count > 0)
                {
                    pCurrentSym = getSymbol(dt.Rows[0]["Object"]);
                    Bitmap bitmap = PreviewItem(pCurrentSym, 180, 180);
                    this.pictureEdit1.Image = bitmap;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void UpdateStatus(string txt)
        {
            this.brStatus.Caption = txt;
            this.brCurrFile.Caption = GlobalObj.currStyleFile;
        }

        /// <summary>
        /// 实现将Symbol转换为Bitmap
        /// </summary>
        /// <param name="Symbol">符号对象</param>
        /// <param name="Width">宽度值</param>
        /// <param name="Height">高度值</param>
        /// <returns>图像对象</returns>
        public Bitmap PreviewItem(ISymbol Symbol, int Width, int Height)
        {

            Bitmap bitmap = new Bitmap(Width, Height);
            Graphics mGraphics = Graphics.FromImage(bitmap);
            double dpi = mGraphics.DpiY;
            IEnvelope pEnvelope = new EnvelopeClass();
            pEnvelope.PutCoords(0, 0, bitmap.Width, bitmap.Height);
            tagRECT myRect = new tagRECT();
            myRect.bottom = bitmap.Height;
            myRect.left = 0;
            myRect.right = bitmap.Width;
            myRect.top = 0;

            IDisplayTransformation pDisplayTransformation = new DisplayTransformationClass();
            pDisplayTransformation.VisibleBounds = pEnvelope;
            pDisplayTransformation.Bounds = pEnvelope;
            pDisplayTransformation.set_DeviceFrame(ref myRect);
            pDisplayTransformation.Resolution = dpi;
            IntPtr pIntPtr = mGraphics.GetHdc();
            int hDC = pIntPtr.ToInt32();
            Symbol.SetupDC(hDC, pDisplayTransformation);
            IGeometry pGeometry = GetSymbolGeometry(Symbol, pEnvelope);
            Symbol.Draw(pGeometry);
            Symbol.ResetDC();
            mGraphics.ReleaseHdc(pIntPtr);

            mGraphics.Dispose();

            return bitmap;

        }

        /// <summary>
        /// 根据符号类型得到相应的几何形体
        /// </summary>
        /// <param name="Symbol">符号对象</param>
        /// <param name="Envelop">矩形对象</param>
        /// <returns>几何形体对象</returns>
        private IGeometry GetSymbolGeometry(ISymbol Symbol, IEnvelope Envelop)
        {

            IGeometry pGeometry = null;

            if (Symbol is IMarkerSymbol)
            {

                IArea pArea = Envelop as IArea;
                pGeometry = pArea.Centroid;

            }
            else if (Symbol is ILineSymbol)
            {

                IPolyline pPolyline = new PolylineClass();
                IPoint pFromPoint = new PointClass();
                pFromPoint.PutCoords(Envelop.XMin, (Envelop.YMax + Envelop.YMin) / 2);
                IPoint pToPoint = new PointClass();
                pToPoint.PutCoords(Envelop.XMax, (Envelop.YMax + Envelop.YMin) / 2);
                pPolyline.FromPoint = pFromPoint;
                pPolyline.ToPoint = pToPoint;
                pGeometry = pPolyline;
            }
            else if (Symbol is IFillSymbol)
            {
                pGeometry = Envelop;
            }
            else if (Symbol is ITextSymbol)
            {

                IArea pArea = Envelop as IArea;
                pGeometry = pArea.Centroid;

            }
            return pGeometry;

        }



        //打开符号
        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "符号文件(*.style)|*.style";
            if (dlg.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            if (GlobalObj.currStyleFile == dlg.FileName)
            {
                MessageBox.Show("当前文件已经打开！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                return;
            }
            GlobalObj.currStyleFile = dlg.FileName;
            DataOperateHelper.Connstring = "Provider=Microsoft.JET.OLEDB.4.0;data source=" + dlg.FileName;
            RefreshData("Marker Symbols");

            UpdateStatus("当前打开：点状符号");
        }

        /// <summary>
        /// 从数据库字段获取符号
        /// </summary>
        /// <param name="StyleObject"></param>
        /// <returns></returns>
        private ISymbol getSymbol(Object StyleObject)
        {

            //object StyleObject = dt.Rows[0]["Object"];
            System.Array pArray = (System.Array)StyleObject;
            byte[] pGuidByte = new byte[16];
            System.Array.Copy(pArray, 0, pGuidByte, 0, 16);
            System.Guid pGuid = new Guid(pGuidByte);
            System.Type objectCustom = Type.GetTypeFromCLSID(pGuid);

            IPersistStream pPersistStream = (IPersistStream)Activator.CreateInstance(objectCustom);
            byte[] StyleData = new byte[pArray.Length - 16];
            System.Array.Copy(pArray, 16, StyleData, 0, pArray.Length - 16);


            //ISymbol pSymbol = new SimpleFillSymbolClass();
            //pSymbol = DecodeGeometry(pSymbol, StyleData);

            IMemoryBlobStream pStream = new MemoryBlobStreamClass();
            ((IMemoryBlobStreamVariant)pStream).ImportFromVariant(StyleData);
            pPersistStream.Load((IStream)pStream);

            ISymbol pSym = pPersistStream as ISymbol;
            return pSym;
        }


        private void SaveSymbol(ISymbol pSymbol,string tabName,string sName,string sCategory)
        {
            System.Guid pGuid = pSymbol.GetType().GUID;
            byte[] pGuidArr = pGuid.ToByteArray();
            System.Type objectCustom = Type.GetTypeFromCLSID(pGuid);


            IPersistStream pPersistStream = pSymbol as IPersistStream;
            XMLStreamClass aStream = new XMLStreamClass();
            pPersistStream.Save(aStream, 0);
            System.Array pArray = aStream.SaveToBytes();


            byte[] pGuidByte = new byte[pArray.Length + 16];
            System.Array.Copy(pGuidArr, 0, pGuidByte, 0, 16);
            System.Array.Copy(pArray, 0, pGuidByte, 16, pArray.Length);

            
            OleDbConnection myConnection = new OleDbConnection(DataOperateHelper.Connstring);
            OleDbCommand command = new OleDbCommand("INSERT INTO ["+tabName+"] (name,Category,[Object])" +
            "VALUES (@cname,@ccate,@obj)", myConnection);
            System.Data.OleDb.OleDbParameter paramPersonName = new OleDbParameter("@cname", System.Data.OleDb.OleDbType.VarChar, 255);
            paramPersonName.Value = sName;
            command.Parameters.Add(paramPersonName);
            System.Data.OleDb.OleDbParameter paramPersonEmail = new OleDbParameter("@ccate", System.Data.OleDb.OleDbType.VarChar, 255);
            paramPersonEmail.Value = sCategory;
            command.Parameters.Add(paramPersonEmail);
            System.Data.OleDb.OleDbParameter paramPersonImageType = new OleDbParameter("@obj", System.Data.OleDb.OleDbType.Binary);
            paramPersonImageType.Value = pGuidByte;
            command.Parameters.Add(paramPersonImageType);
            //打开连接，执行查询
            myConnection.Open();
            command.ExecuteNonQuery();
            myConnection.Close();
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            DataRow dr= this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
            if (dr == null)
                return;
            object StyleObject = dr["Object"];
            pCurrentSym = getSymbol(StyleObject);

            Bitmap bitmap = PreviewItem(pCurrentSym, 180, 180);
            this.pictureEdit1.Image = bitmap;

        }

        private void navBarLine_LinkPressed(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (GlobalObj.currStyleFile == "")
                return;
            if (this.gridControl1.DataMember.ToUpper()=="Line Symbols".ToUpper())
                return;

            RefreshData("Line Symbols");
            UpdateStatus("当前打开：线状符号");

        }

        private void navBarPolygon_LinkPressed(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (GlobalObj.currStyleFile == "")
                return;
            if (this.gridControl1.DataMember.ToUpper() == "Fill Symbols".ToUpper())
                return;

            RefreshData("Fill Symbols");
            UpdateStatus("当前打开：面状符号");
        }

        private void navBarPoint_LinkPressed(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (GlobalObj.currStyleFile == "")
                return;
            if (this.gridControl1.DataMember.ToUpper() == "Marker Symbols".ToUpper())
                return;

            RefreshData("Marker Symbols");
            UpdateStatus("当前打开：点状符号");
        }

        
        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            //新建符号
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "符号文件(*.style)|*.style";

            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            string sFile=dlg.FileName;
            if (System.IO.File.Exists(sFile))
            {
                if (MessageBox.Show("当前已存在该文件，是否覆盖?","提示",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.No)
                    return;
                System.IO.File.Delete(sFile);
            }
            System.IO.File.Copy(Application.StartupPath + @"\style\blank.Style", sFile);
            GlobalObj.currStyleFile =sFile ;
            DataOperateHelper.Connstring = "Provider=Microsoft.JET.OLEDB.4.0;data source=" + sFile;
            RefreshData("Marker Symbols");



        }

        //删除当前选中符号
        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            DataRow dr = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
            if (dr == null)
                return;
            string id = dr["ID"].ToString();
            if (MessageBox.Show("您确定要删除编号为" + id + "的符号么？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            try
            {
                dr.Delete();
                DataOperateHelper.getcmd("delete from [" + this.gridControl1.DataMember + "] where id=" + id);
                this.gridControl1.Refresh();
                MessageBox.Show("删除成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        //新增符号
        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {

            if (this.gridControl1.DataMember.Trim() == "")
                return;



            string tabName = this.gridControl1.DataMember.Trim().ToUpper();

            StyleNameDlg dlg = new StyleNameDlg();
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            string sName = dlg.StyleName;
            string sCategory = dlg.StyleCategory;

            ISymbol pSym = null;

            switch (tabName.ToUpper())
            {
                case "MARKER SYMBOLS":
                    pSym = new SimpleMarkerSymbolClass();
                    break;
                case "LINE SYMBOLS":
                    pSym = new SimpleLineSymbolClass();
                    break;
                case "FILL SYMBOLS":
                    pSym = new SimpleFillSymbolClass();
                    break;
            }
            ServerStyleEditor aEditor = new ServerStyleEditor();
            aEditor.EditedStyle = pSym;
            if (aEditor.ShowDialog() == DialogResult.Cancel)
                return;
            ISymbol aNewSym = aEditor.EditedStyle;
            SaveSymbol(aNewSym, tabName, sName, sCategory);
            RefreshData(tabName);


        }

        //编辑符号
        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {

            if (pCurrentSym == null)
                return;
            DataRow pDr = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
            string id = pDr["ID"].ToString();
            string name = pDr["Name"].ToString();
            string Category = pDr["Category"].ToString();

            try
            {
                

                ServerStyleEditor aEditor = new ServerStyleEditor();
                aEditor.EditedStyle = pCurrentSym;

                DialogResult dr = aEditor.ShowDialog(this);
                if (dr == DialogResult.OK)
                {
                    ISymbol aNewSym = aEditor.EditedStyle;

                    string tabName=pDr.Table.TableName ;


                    //重命名 名称和种类
                    StyleNameDlg dlg = new StyleNameDlg();
                    dlg.StyleName = name;
                    dlg.StyleCategory = Category;
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        name = dlg.StyleName;
                        Category = dlg.StyleCategory;
                    }


                    //删除原来的，添加新的
                    DataOperateHelper.getcmd("delete from [" +tabName + "] where Id=" + id);

                    SaveSymbol(aNewSym, tabName, name, Category);
                    RefreshData(tabName);

                }
            }
            catch { }
            
        }

        private void frmMain_Load(object sender, EventArgs e)
        {

        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            Close();
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            string exeFile = @"C:\Program Files (x86)\ArcGIS\Desktop10.2\bin\MakeServerStyleSet.exe";
            if (!System.IO.File.Exists(exeFile))
            {
                if (MessageBox.Show("没找到MakeServerStyleSet.exe，\r\n是否指定文件位置。", "询问",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    OpenFileDialog dlg = new OpenFileDialog();
                    dlg.Filter = "可执行文件|*.exe";
                    if (dlg.ShowDialog() == DialogResult.Cancel)
                        return;
                    string destfile = dlg.FileName;
                    System.Diagnostics.Process.Start(dlg.FileName);

                }
            }
            else
            {
                System.Diagnostics.Process.Start(exeFile);
            }
        }
    }
}