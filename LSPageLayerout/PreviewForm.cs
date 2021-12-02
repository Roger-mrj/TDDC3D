using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Output;
using ESRI.ArcGIS.esriSystem;

namespace RCIS.Controls
{
    public partial class PreviewForm : Form
    {
        public PreviewForm()
        {
            InitializeComponent();
        }

        public ESRI.ArcGIS.Controls.AxPageLayoutControl m_PageControl;

        private double m_dPageW, m_dPageH;

        //输出到打印机
        private void button11_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            ESRI.ArcGIS.Controls.AxPageLayoutControl targetPLC = axPageLayoutControl1;
            IPrinter printer = targetPLC.Printer;
            if (printer.Paper.Orientation != targetPLC.Page.Orientation)
                printer.Paper.Orientation = targetPLC.Page.Orientation;
            targetPLC.PrintPageLayout(1, 1, 0);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            MessageBox.Show("输出OK!");
        }

        private void PreviewForm_Load(object sender, EventArgs e)
        {
            ESRI.ArcGIS.Controls.AxPageLayoutControl sourcePLC = m_PageControl;
            ESRI.ArcGIS.Controls.AxPageLayoutControl targetPLC = axPageLayoutControl1;

            //参数:
            IPage sourcePage = sourcePLC.Page;
            IPage targetPage = targetPLC.Page;
            targetPage.Units = sourcePage.Units;
            targetPage.PageToPrinterMapping = sourcePage.PageToPrinterMapping;
            targetPage.FormID = sourcePage.FormID;
            double dW = 0.0, dH = 0.0;
            sourcePage.QuerySize(out dW, out dH);
            targetPage.PutCustomSize(dW, dH);
            targetPage.BackgroundColor = sourcePage.BackgroundColor;
            sourcePage.QuerySize(out m_dPageW, out m_dPageH);

            //删除所有的Element:
            IGraphicsContainer pageCon = targetPLC.ActiveView.GraphicsContainer;
            pageCon.DeleteAllElements();
            IGraphicsContainer sourceCon = sourcePLC.ActiveView.GraphicsContainer;
            IGraphicsContainer targetCon = targetPLC.ActiveView.GraphicsContainer;
            sourceCon.Reset();
            IElement ele = sourceCon.Next();
            while (ele != null)
            {
                if (ele is IMapFrame)
                {
                    IMapFrame mapF = ele as IMapFrame;
                    IClone pClone = mapF as IClone;
                    IMapFrame mapFC = pClone.Clone() as IMapFrame;
                    targetCon.AddElement((IElement)mapFC, 0);
                }
                else targetCon.AddElement(ele, 0);
                ele = sourceCon.Next();
            }

            //信息:
            IPrinter printer = targetPLC.Printer;
            if (printer != null)
            {
                printerName.Text = printer.Paper.PrinterName;
                if (printer.Paper.Orientation == 1)
                    paperOrientation.Text = "纵向";
                else
                    paperOrientation.Text = "横向";
                double dWidth;
                double dheight;
                printer.Paper.QueryPaperSize(out dWidth, out dheight);
                paperSize.Text = dWidth.ToString("###.00") + "x" + dheight.ToString("###.00") + " Inches";
            }
            pagesize.Items.Add("Letter - 8.5in x 11in.");
            pagesize.Items.Add("Legal - 8.5in x 14in.");
            pagesize.Items.Add("Tabloid - 11in x 17in.");
            pagesize.Items.Add("C - 17in x 22in.");
            pagesize.Items.Add("D - 22in x 34in.");
            pagesize.Items.Add("E - 34in x 44in.");
            pagesize.Items.Add("A5 - 148mm x 210mm.");
            pagesize.Items.Add("A4 - 210mm x 297mm.");
            pagesize.Items.Add("A3 - 297mm x 420mm.");
            pagesize.Items.Add("A2 - 420mm x 594mm.");
            pagesize.Items.Add("A1 - 594mm x 841mm.");
            pagesize.Items.Add("A0 - 841mm x 1189mm.");
            pagesize.Items.Add("Custom Page Size.");
            pagesize.Items.Add("Same as Printer Form.");
            pagesize.SelectedIndex = 12;

            //...
            (sourcePLC.ActiveView.FocusMap as IActiveView).Refresh();
            sourcePLC.ActiveView.Refresh();

            (targetPLC.ActiveView.FocusMap as IActiveView).Refresh();
            targetPLC.ActiveView.Refresh();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            ESRI.ArcGIS.Controls.AxPageLayoutControl targetPLC = axPageLayoutControl1;
            if (radioButton1.Checked == true)
            {
                //Set the page orientation
                if (targetPLC.Page.FormID != esriPageFormID.esriPageFormSameAsPrinter)
                {
                    targetPLC.Page.Orientation = 1;
                    paperOrientation.Text = "纵向";
                }
            }


        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            ESRI.ArcGIS.Controls.AxPageLayoutControl targetPLC = axPageLayoutControl1;
            if (radioButton2.Checked == true)
            {
                //Set the page orientation
                if (targetPLC.Page.FormID != esriPageFormID.esriPageFormSameAsPrinter)
                {
                    targetPLC.Page.Orientation = 2;
                    paperOrientation.Text = "横向";
                }
            }
        }

        private void pagesize_SelectedIndexChanged(object sender, EventArgs e)
        {
            ESRI.ArcGIS.Controls.AxPageLayoutControl targetPLC = axPageLayoutControl1;
            //Set the page size
            targetPLC.Page.FormID = (esriPageFormID)pagesize.SelectedIndex;
            if (pagesize.SelectedIndex == 12)
                targetPLC.Page.PutCustomSize(m_dPageW, m_dPageH);
        }

        private void PreviewForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //ESRI.ArcGIS.Controls.AxPageLayoutControl sourcePLC = m_PageControl;
            //sourcePLC.ZoomToWholePage();
        }



    }
}