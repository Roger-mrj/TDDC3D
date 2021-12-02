using System;
using System.Windows.Forms;
using System.Drawing;
using Excel=Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace RCIS.Utility
{
    public class OtherHelper
    {
        /// <summary>
        /// ���� �б�� �е��� ѡ��״̬ ��ȡ �ַ���
        /// </summary>
        /// <param name="chkLstCtl"></param>
        /// <returns></returns>
        public static string GetStrByChkListBox(DevExpress.XtraEditors.CheckedListBoxControl chkLstCtl)
        {
            string lcxl = "";
            for (int j = 0; j < chkLstCtl.Items.Count; j++)
            {
                if (chkLstCtl.Items[j].CheckState == System.Windows.Forms.CheckState.Checked)
                {
                    lcxl += OtherHelper.GetLeftName(chkLstCtl.Items[j].Value.ToString().Trim()) + ",";
                }
            }
            if (lcxl.Length > 0)
            {
                lcxl = lcxl.Substring(0, lcxl.Length - 1);
            }
            return lcxl;
        }


        /// <summary>
        /// �����ַ����б�����  �б���е����Ƿ�ѡ��
        /// </summary>
        /// <param name="lstSrc"></param>
        /// <param name="chkLstCtl"></param>
        public static void SetCheckedListByStr(List<string> lstSrc, DevExpress.XtraEditors.CheckedListBoxControl chkLstCtl)
        {

            #region �����б�

            for (int i = 0; i < chkLstCtl.Items.Count; i++)
            {
                string aFunId = chkLstCtl.Items[i].Value.ToString();
                if (aFunId.IndexOf('|') > -1)
                {
                    aFunId = aFunId.Substring(0, aFunId.IndexOf('|'));

                }
                if (lstSrc.Contains(aFunId))
                {
                    chkLstCtl.Items[i].CheckState = CheckState.Checked;
                }
                else
                {
                    chkLstCtl.Items[i].CheckState = CheckState.Unchecked;
                }
            }
            #endregion
        }


        public static string ChangeDW2(string str)
        {
            try
            {
                if (str == "") return "0.00";
                if ((str == "0") || (str == "0.00") || (str == "0.0")) return "0.00";
                double ss = 0.00;

                double.TryParse(str, out ss);

                decimal d = decimal.Round((decimal)ss, 2, MidpointRounding.AwayFromZero);

                return d.ToString("F2");
            }
            catch { return ""; }
        }
        


        /// <summary>
        /// ���  DM|MC  ���
        /// </summary>
        /// <param name="sName"></param>
        /// <returns></returns>
        public static string GetLeftName(string sName)
        {
            string ss;
            if (sName == "")
            {
                return "";
            }
            if (sName.Contains("|"))
            {
                ss = sName.Substring(0, sName.IndexOf("|")).Trim();
            }
            else
            {
                return sName;
            }
            return ss;
        }
        /// <summary>
        /// ���  DM|MC  ���
        /// </summary>
        /// <param name="sName"></param>
        /// <returns></returns>
        public static string GetLeftName(string sName, string sflag)
        {
            try
            {
                string ss;
                if (sName == "")
                {
                    return "";
                }
                if (sName.Contains(sflag))
                {
                    ss = sName.Substring(0, sName.IndexOf(sflag)).Trim();
                }
                else
                {
                    return sName;
                }
                return ss;
            }
            catch { return ""; }
        }
        /// <summary>
        /// ���  DM|MC  �ұ�
        /// </summary>
        /// <param name="sName"></param>
        /// <returns></returns>
        public static string GetRightName(string sName)
        {
            string ss;
            if (sName == "")
            {
                return "";
            }
            if (sName.Contains("|"))
            {
                ss = sName.Substring(sName.IndexOf("|") + 1, sName.Length - sName.IndexOf("|") - 1).Trim();

            }
            else { return sName; }
            return ss;
        }
        /// <summary>
        /// ���  DM|MC  �ұ�
        /// </summary>
        /// <param name="sName"></param>
        /// <returns></returns>
        public static string GetRightName(string sName, string sflag)
        {
            try
            {
                string ss;
                if (sName == "")
                {
                    return "";
                }
                if (sName.Contains(sflag))
                {
                    ss = sName.Substring(sName.IndexOf(sflag) + 1, sName.Length - sName.IndexOf(sflag) - 1).Trim();
                }
                else
                {
                    return sName;
                }
                return ss;
            }
            catch { return ""; }
        }
        #region//ת����ֵΪ0
        public static int ChangeNullToZero(string str)
        {

            if ((str == "") || (str == null))
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(str);
            }
        }
        public static int ChangeNullToZero(object str)
        {

            if ((str == "") || (str == null))
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(str);
            }
        }
        #endregion
        #region//ת����ֵΪ0.0
        public static double ChangeNullToDoubleZero(string str)
        {
            try
            {
                if ((str == "") || (str == null))
                {
                    return 0.0;
                }
                else
                {
                    return Convert.ToDouble(str);
                }
            }
            catch { return 0.0; }
        }
        public static double ChangeNullToDoubleZero(object str)
        {
            try
            {
                if ((str == "") || (str == null))
                {
                    return 0.0;
                }
                else
                {
                    return Convert.ToDouble(str);
                }
            }
            catch (Exception ex)
            { return 0.0; }
        }
        #endregion
        #region//ת����ֵΪ""
        public static string ChangeNullToString(object str)
        {

            if (str == null)
            {
                return "";
            }
            else
            {
                return Convert.ToString(str);
            }

        }
        #endregion


        #region ��ѯListView�����ڵ���
        public static int QueryColumnIndex(ListView paramLV, int paramX)
        {
            int columnIndex = -1, columnWidth = 0;
            for (int ci = 0; ci < paramLV.Columns.Count; ci++)
            {
                columnWidth += paramLV.Columns[ci].Width;
                if (columnWidth > paramX)
                {
                    columnIndex = ci;
                    break;
                }
            }
            return columnIndex;
        }
        public static int QueryColumnLocation(ListView paramLV, int paramColumnIndex)
        {
            int columnWidth = 0;
            if (paramColumnIndex <= paramLV.Columns.Count)
            {
                for (int ci = 0; ci < paramColumnIndex; ci++)
                {
                    columnWidth += paramLV.Columns[ci].Width;
                }
            }
            return columnWidth;
        }    
        #endregion 

        /// <summary>
        /// �ͷ�COM����
        /// </summary>
        /// <param name="o"></param>
        public static void ReleaseComObject(object o)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(o);
            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(o);
        }

        #region ����ͼƬ
        public static Image LoadImage(string imageName)
        {
            
            Image resultImg = null;
            try
            {
                if (System.IO.File.Exists(imageName))
                {
                    resultImg = Image.FromFile(imageName);
                }
            }
            catch (Exception ex)
            {
            }
            return resultImg;
        }
        #endregion




        /// <summary>
        /// ������������ǰ��λ
        /// </summary>
        /// <param name="xzdm"></param>
        /// <returns></returns>
        public static string listcount(string xzdm)
        {
            if (xzdm.Length < 12)
            {
                xzdm = xzdm.PadRight(12, '0');
            }
            string result = "";
            if (xzdm == "000000000000")
                result = "";
            else
                if (xzdm.Substring(2, 10) == "0000000000")
                    result = xzdm.Substring(0, 2);
                else if (xzdm.Substring(4, 8) == "00000000")
                    result = xzdm.Substring(0, 4);
                else if (xzdm.Substring(6, 6) == "000000")
                    result = xzdm.Substring(0, 6);
                else if (xzdm.Substring(9, 3) == "000")
                    result = xzdm.Substring(0, 9);
                else 
                    result = xzdm.Substring(0, 12);
            return result;
        }



        /// <summary>
        /// ��Listview�еĽ��������Excel��
        /// </summary>
        /// <param name="caption">����</param>
        /// <param name="date">����</param>
        /// <param name="listview"></param>
        public static void ExportListViewToExcel(string caption, string date, ListView listview)
        {
            if (listview.Items.Count == 0) return;
            //DataGridView�ɼ�����
            int visiblecolumncount = listview.Columns.Count;


            try
            {
                //��ǰ�����е�����
                int currentcolumnindex = 1;
                //��ǰ�����е�����

                Excel.ApplicationClass Mylxls = new Excel.ApplicationClass();
                Mylxls.Application.Workbooks.Add(true);
                //Mylxls.Cells.Font.Size = 10.5;   //����Ĭ�������С
                //���ñ�ͷ
                Mylxls.Caption = caption;
                //��ʾ��ͷ
                Mylxls.Cells[1, 1] = caption;
                //��ʾʱ��
                Mylxls.Cells[2, 1] = date;
                //�����ͷ
                for (int i = 0; i < listview.Columns.Count; i++)
                {

                    Mylxls.Cells[3, currentcolumnindex] = listview.Columns[i].Text;
                    Mylxls.get_Range(Mylxls.Cells[3, currentcolumnindex], Mylxls.Cells[3, currentcolumnindex]).Cells.Borders.LineStyle = 1; //���ñ߿�
                    Mylxls.get_Range(Mylxls.Cells[3, currentcolumnindex], Mylxls.Cells[3, currentcolumnindex]).ColumnWidth = listview.Columns[i].Width / 8;

                    currentcolumnindex++;

                }
                Mylxls.get_Range(Mylxls.Cells[1, 1], Mylxls.Cells[1, visiblecolumncount]).MergeCells = true; //�ϲ���Ԫ��

                Mylxls.get_Range(Mylxls.Cells[1, 1], Mylxls.Cells[1, 1]).RowHeight = 30;   //�и�

                Mylxls.get_Range(Mylxls.Cells[1, 1], Mylxls.Cells[1, visiblecolumncount]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; //������ʾ
                Mylxls.get_Range(Mylxls.Cells[2, 1], Mylxls.Cells[2, 2]).MergeCells = true; //�ϲ�
                Mylxls.get_Range(Mylxls.Cells[2, 1], Mylxls.Cells[2, 2]).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft; //�����ʾ


                object[,] dataArray = new object[listview.Items.Count, visiblecolumncount];

                //��ǰ�����е�����
                //int currentcolumnindex = 1;
                //��ǰ�����е�����
                for (int i = 0; i < listview.Items.Count; i++)   //ѭ���������
                {
                    currentcolumnindex = 1;
                    for (int j = 0; j < listview.Columns.Count; j++)
                    {

                        if (listview.Items[i].SubItems[j].Text != null)  //�����Ԫ�����ݲ�Ϊ��
                        {
                            dataArray[i, currentcolumnindex - 1] = listview.Items[i].SubItems[j].Text;
                        }
                        currentcolumnindex++;

                    }
                }
                for (int i = 0; i < listview.Items.Count; i++)
                {
                    int iRow = i + 4;
                    for (int j = 0; j < listview.Columns.Count; j++)
                    {
                        Mylxls.get_Range(Mylxls.Cells[iRow, j + 1], Mylxls.Cells[iRow, j + 1]).NumberFormat = "@";

                    }
                }
                Mylxls.get_Range(Mylxls.Cells[4, 1], Mylxls.Cells[listview.Items.Count + 3, visiblecolumncount]).Value2 = dataArray; //���ñ߿�

                Mylxls.get_Range(Mylxls.Cells[4, 1], Mylxls.Cells[listview.Items.Count + 3, visiblecolumncount]).Cells.Borders.LineStyle = 1; //���ñ߿�
                Mylxls.Visible = true;

            }
            catch
            {
                MessageBox.Show("��Ϣ����ʧ�ܣ���ȷ����Ļ�����װ��Microsoft Office Excel 2003��", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {

            }
        }

        /// <summary>
        /// ����Ի���
        /// </summary>
        /// <param name="Caption"></param>
        /// <param name="Hint"></param>
        /// <param name="Default"></param>
        /// <returns></returns>
        public  static  string InputBox(string Caption, string Hint, string Default)
        {

            Form InputForm = new Form();
            InputForm.MinimizeBox = false;
            InputForm.MaximizeBox = false;
            InputForm.StartPosition = FormStartPosition.CenterScreen;
            InputForm.Width = 220;
            InputForm.Height = 150;
            //InputForm.Font.Name = "����";
            //InputForm.Font.Size = 10;

            InputForm.Text = Caption;
            System.Windows.Forms.Label lbl = new System.Windows.Forms.Label();
            lbl.Text = Hint;
            lbl.Left = 10;
            lbl.Top = 20;
            lbl.Parent = InputForm;
            lbl.AutoSize = true;
            System.Windows.Forms.TextBox tb = new System.Windows.Forms.TextBox();
            tb.Left = 30;
            tb.Top = 45;
            tb.Width = 160;
            tb.Parent = InputForm;
            tb.Text = Default;
            tb.SelectAll();
            System.Windows.Forms.Button btnok = new System.Windows.Forms.Button();
            btnok.Left = 30;
            btnok.Top = 80;
            btnok.Parent = InputForm;
            btnok.Text = "ȷ��";
            InputForm.AcceptButton = btnok;//�س���Ӧ

            btnok.DialogResult = DialogResult.OK;
            System.Windows.Forms.Button btncancal = new System.Windows.Forms.Button();
            btncancal.Left = 120;
            btncancal.Top = 80;
            btncancal.Parent = InputForm;
            btncancal.Text = "ȡ��";
            btncancal.DialogResult = DialogResult.Cancel;
            try
            {
                if (InputForm.ShowDialog() == DialogResult.OK)
                {
                    return tb.Text;
                }
                else
                {
                    return null;
                }
            }
            finally
            {
                InputForm.Dispose();
            }

        }
    }
}
