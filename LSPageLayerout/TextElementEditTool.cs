using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using System.Windows.Forms;


using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;

using RCIS.GISCommon;


namespace RCIS.MapTool
{
    /// <summary>
    /// Summary description for TextElementEditTool.
    /// </summary>
    [Guid("2586bbb5-4f25-4a8b-9467-7e6377e4a1a6")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("RCIS.MapTool.TextElementEditTool")]
    public sealed class TextElementEditTool : BaseTool
    {
        #region COM Registration Function(s)
        [ComRegisterFunction()]
        [ComVisible(false)]
        static void RegisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryRegistration(registerType);

            //
            // TODO: Add any COM registration code here
            //
        }

        [ComUnregisterFunction()]
        [ComVisible(false)]
        static void UnregisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryUnregistration(registerType);

            //
            // TODO: Add any COM unregistration code here
            //
        }

        #region ArcGIS Component Category Registrar generated code
        /// <summary>
        /// Required method for ArcGIS Component Category registration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryRegistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            ControlsCommands.Register(regKey);

        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            ControlsCommands.Unregister(regKey);

        }

        #endregion
        #endregion

        private IHookHelper m_hookHelper;

        private AxPageLayoutControl m_pageLayeroutCtl = null;

        public TextElementEditTool( AxPageLayoutControl control)
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = ""; //localizable text 
            base.m_caption = "";  //localizable text 
            base.m_message = "";  //localizable text
            base.m_toolTip = "";  //localizable text
            base.m_name = "";   //unique id, non-localizable (e.g. "MyCategory_MyTool")
            try
            {
                this.m_lineSymbol = new SimpleLineSymbolClass();
                this.m_lineSymbol.Color = ColorHelper.CreateColor(0, 0, 255);
                this.m_lineSymbol.Style = esriSimpleLineStyle.esriSLSDash;
                this.m_lineSymbol.ROP2 = esriRasterOpCode.esriROPNotXOrPen;
                m_pageLayeroutCtl = control;

                CreateContextMenu();

                //
                // TODO: change resource name if necessary
                //
                string bitmapResourceName = GetType().Name + ".bmp";
                base.m_bitmap = new Bitmap(GetType(), bitmapResourceName);
                //base.m_cursor = new System.Windows.Forms.Cursor(GetType(), GetType().Name + ".cur");
                base.m_cursor = new System.Windows.Forms.Cursor(Application.StartupPath + @"\cur\mouse.cur");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
            }
        }


        private int m_toolState = 0;//表示空闲,1表示移动	
        private int ToolStateIdle = 0;
        private int ToolStateMove = 1;
        private SimpleLineSymbolClass m_lineSymbol;
        private PolygonClass m_lastGeometry;
        private IPoint m_originPoint;
        private IPoint m_lastPoint;
        private IElement m_curElement;
        protected MenuItem m_miAddNew;
        protected MenuItem m_miAttribute;
        protected MenuItem m_miDelete;

        protected ContextMenu m_ctxMenu = null;

        protected  void CreateContextMenu()
        {
            this.m_ctxMenu = new ContextMenu();
            this.m_miAddNew = new MenuItem("添加标注");
            this.m_miAddNew.Click += new EventHandler(m_miAddNew_Click);
            this.m_miAttribute = new MenuItem("修改属性");
            this.m_miAttribute.Click += new EventHandler(OnEditAttribute);
            this.m_miDelete = new MenuItem("删除标注");
            this.m_miDelete.Click += new EventHandler(OnDeleteElement);
            this.m_ctxMenu.MenuItems.Add(this.m_miAddNew);
            this.m_ctxMenu.MenuItems.Add(this.m_miAttribute);
            this.m_ctxMenu.MenuItems.Add(this.m_miDelete);

            
        }

        void m_miAddNew_Click(object sender, EventArgs e)
        {

            IPoint aPt = null;
            if (this.m_lastPoint != null && !this.m_lastPoint.IsEmpty)
            {
                aPt = (this.m_lastPoint as IClone).Clone() as IPoint;
            }
            else
            {
                IEnvelope aEnv = this.m_hookHelper.ActiveView.Extent;
                IPoint aCt = (aEnv as IArea).Centroid;
                aPt = (aCt as IClone).Clone() as IPoint;
            }
            ITextElement aTe = new TextElementClass();
            aTe.Text = "新建文本标注";
            aTe.Symbol = new TextSymbolClass();
            (aTe as IElement).Geometry = aPt;
            this.m_hookHelper.ActiveView.GraphicsContainer.AddElement(aTe as IElement, 0);
            TextElementPropertyForm attrForm = new TextElementPropertyForm();
            attrForm.TargetElement = aTe;
            attrForm.ActiveView = this.m_hookHelper.ActiveView;
            if (attrForm.ShowDialog() == DialogResult.Cancel)
            {
                this.m_hookHelper.ActiveView.GraphicsContainer.DeleteElement(aTe as IElement);
            }
        }

        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
           

            if (null == m_ctxMenu) return;

            if (X < 0 || Y < 0) return;

            System.Drawing.Point pt = new System.Drawing.Point(X, Y);

            if (Button == 1)
            {
                IPoint mapPt = this.m_hookHelper.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);
                IEnumElement aEnumEle = this.m_hookHelper.ActiveView.GraphicsContainer.LocateElements(mapPt, 0.1);
                if (aEnumEle != null)
                {
                    IElement aEle = aEnumEle.Next();
                    if (!(aEle is IMapFrame))
                    {
                        this.m_lastPoint = mapPt;
                        this.m_originPoint = mapPt;
                        this.m_curElement = aEle;
                        IEnvelope aEnv = new EnvelopeClass();
                        aEle.QueryBounds(this.m_hookHelper.ActiveView.ScreenDisplay, aEnv);
                        this.m_lastGeometry = new PolygonClass();
                        this.m_lastGeometry.SetRectangle(aEnv);
                        this.DrawElementLine();
                        this.m_toolState = this.ToolStateMove;
                    }
                }
            }
            else if (Button == 2)
            {
                try
                {
                    pt.Offset(m_pageLayeroutCtl.Left, m_pageLayeroutCtl.Top);
                    this.m_ctxMenu.Show(m_pageLayeroutCtl.Parent, pt);
                }
                catch (Exception ex)
                {
                }
            }
             
        }
        private void DrawElementLine()
        {
            if (this.m_lastGeometry != null && !this.m_lastGeometry.IsEmpty)
            {
                IGeometry boundary = (this.m_lastGeometry as ITopologicalOperator).Boundary;
                if (boundary != null && !boundary.IsEmpty)
                {
                    
                    DisplayHelper.DrawGeometry(this.m_hookHelper.ActiveView.ScreenDisplay, boundary, this.m_lineSymbol);
                }
            }
        }
        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {
            IPoint mapPt = this.m_hookHelper.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);
            if (this.m_toolState == this.ToolStateIdle)
            {//空闲的时候
                this.m_cursor = System.Windows.Forms.Cursors.Default;
                IEnumElement aEnumEle = this.m_hookHelper.ActiveView.GraphicsContainer.LocateElements(mapPt, 0.1);
                if (aEnumEle != null)
                {
                    IElement aEle = aEnumEle.Next();
                    if (!(aEle is IMapFrame))
                    {
                        this.m_cursor = System.Windows.Forms.Cursors.SizeAll;
                    }
                }
            }
            else if (this.m_toolState == this.ToolStateMove)
            {//移动的时候
                this.DrawElementLine();
                double deltaX = mapPt.X - this.m_lastPoint.X;
                double deltaY = mapPt.Y - this.m_lastPoint.Y;
                (this.m_lastGeometry as ITransform2D).Move(deltaX, deltaY);
                this.DrawElementLine();
            }
            this.m_lastPoint = mapPt;
        }
        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {
            if (this.m_toolState == this.ToolStateMove)
            {
                IPoint mapPt = this.m_hookHelper.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);
                this.DrawElementLine();
                double deltaX = mapPt.X - this.m_originPoint.X;
                double deltaY = mapPt.Y - this.m_originPoint.Y;
                (this.m_curElement as ITransform2D).Move(deltaX, deltaY);
                this.m_hookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics
                    , this.m_curElement, this.m_hookHelper.ActiveView.Extent);

                #region 回复状态
                this.m_toolState = this.ToolStateIdle;
                this.m_curElement = null;
                this.m_lastGeometry = null;
                this.m_originPoint = null;
                //this.m_lastPoint =null;
                #endregion
            }
            base.OnMouseUp(Button, Shift, X, Y);
        }

        private void OnEditAttribute(object sender, EventArgs e)
        {
            if (this.m_lastPoint != null)
            {
                IEnumElement aEnumElement = this.m_hookHelper.ActiveView.GraphicsContainer.LocateElements(this.m_lastPoint, 0.1);
                IElement aEle = aEnumElement.Next();
                if (aEle is ITextElement)
                {
                    TextElementPropertyForm attrForm = new TextElementPropertyForm();
                    attrForm.TargetElement = aEle as ITextElement;
                    attrForm.ActiveView = this.m_hookHelper.ActiveView;
                    attrForm.ShowDialog();
                }
            }
        }

        private void OnDeleteElement(object sender, EventArgs e)
        {
            if (this.m_lastPoint != null)
            {
                IEnumElement aEnumElement = this.m_hookHelper.ActiveView.GraphicsContainer.LocateElements(this.m_lastPoint, 0.1);
                IElement aEle = aEnumElement.Next();
                if (!(aEle is IMapFrame))
                {

                    string aText = "确定要删除这个图形元素吗?";
                    if (aEle is ITextElement)
                    {
                        aText += "\n" + (aEle as ITextElement).Text;
                    }
                    DialogResult dr = MessageBox.Show(aText, "删除对象", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (DialogResult.OK == dr)
                    {
                        this.m_hookHelper.ActiveView.GraphicsContainer.DeleteElement(aEle);
                        this.m_hookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, aEle, this.m_hookHelper.ActiveView.Extent);
                    }
                }
            }
        }



        #region Overriden Class Methods

        /// <summary>
        /// Occurs when this tool is created
        /// </summary>
        /// <param name="hook">Instance of the application</param>
        public override void OnCreate(object hook)
        {
            if (m_hookHelper == null)
                m_hookHelper = new HookHelperClass();

            m_hookHelper.Hook = hook;

            // TODO:  Add TextElementEditTool.OnCreate implementation
            MessageBox.Show("请用右键选择菜单创建标注!", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion 
    }
}
