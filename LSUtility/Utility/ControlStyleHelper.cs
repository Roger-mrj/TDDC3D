using System.Collections;

using System.Windows.Forms;

namespace RCIS.Utility
{
    public class ControlStyleHelper
    {
        /// <summary>
        /// combox���ҵ� ��Ӧ�� ֵ���ı��������� ���ֵ isAlias��ʾ��Ŀ�ı����Ƿ��������
        /// </summary>
        /// <param name="cmb"></param>
        /// <param name="value"></param>
        public static void FindComboxItem(DevExpress.XtraEditors.ComboBoxEdit cmb,string value,bool isAlias)
        {
            int idx = -1;
            for (int i = 0; i < cmb.Properties.Items.Count; i++)
            {
                string s = cmb.Properties.Items[i].ToString().Trim();
                if (isAlias)
                {
                    s = OtherHelper.GetLeftName(s);
                }

                if (s.ToUpper()==value.ToUpper())
                {
                    idx = i;
                    break;
                }
            }
            cmb.SelectedIndex = idx;
        }


        /// <summary>
        /// ��ֹDatagridview����
        /// </summary>
        /// <param name="dgv"></param>
        public static void ForbidSortColumn(DataGridView dgv)
        {
            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                dgv.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        /// <summary>
        /// ��ʼ��Form����
        /// </summary>
        /// <param name="frm"></param>
        public static void InitFormStyle(Form frm)
        {
            frm.ShowInTaskbar = false;
            frm.MaximizeBox = false;
            frm.MinimizeBox = false;
            frm.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            frm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            frm.ShowDialog();
        }
        public static void InitFormShowStyle(Form frm)
        {
            frm.ShowInTaskbar = true;
            frm.MaximizeBox = false;
            frm.MinimizeBox = false;
            frm.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            frm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            frm.Show();
        }
        /// <summary>
        /// ��ʼ��DataGridView��ʽ
        /// </summary>
        /// <param name="dataGridView1"></param>
        public static void InitDataGridView(DataGridView dataGridView1)
        {
            dataGridView1.RowHeadersDefaultCellStyle.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridView1.BackgroundColor = System.Drawing.Color.White;
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.DefaultCellStyle = dataGridViewCellStyle1;
            dataGridView1.ClearSelection();
            dataGridView1.AllowUserToAddRows = false;
            //dataGridView1.ReadOnly = true;

        }
        #region//��ʼ��ListView��ʽ
        /// <summary>
        /// ��ʼ��ListView��ʽ
        /// </summary>
        /// <param name="listView1"></param>
        public static void InitListViewStyle(ListView listView1)
        {
            listView1.View = View.Details;
            listView1.ShowItemToolTips = true;
            listView1.GridLines = true;
            listView1.FullRowSelect = true;
            listView1.CheckBoxes = true;

        }
        #endregion

        #region ��ʼ��ToolStrip��ʽ
        /// <summary>
        /// ��ʼ��ToolStrip��ʽ
        /// </summary>
        /// <param name="toolStrip1"></param>
        public static void InitToolStrip(ToolStrip toolStrip1)
        {
            toolStrip1.BackColor = System.Drawing.Color.AliceBlue;
        }
        #endregion


        public static void InitDevComboBox(DevExpress.XtraEditors.ComboBoxEdit cbo)
        {
            cbo.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;

        }
        #region �Զ��������listview�����й�
        public static int currentCol = -1;
        public static bool sort;
        #endregion
        /// <summary>
        /// �̳���IComparer
        /// </summary>
        public class ListViewColumnSorter : IComparer
        {
            /// <summary>
            /// ָ�������ĸ�������
            /// </summary>
            private int ColumnToSort;
            /// <summary>
            /// ָ������ķ�ʽ
            /// </summary>
            private SortOrder OrderOfSort;
            /// <summary>
            /// ����CaseInsensitiveComparer�����
            /// </summary>
            private CaseInsensitiveComparer ObjectCompare;

            /// <summary>
            /// ���캯��
            /// </summary>
            public ListViewColumnSorter()
            {
                // Ĭ�ϰ���һ������
                ColumnToSort = 0;

                // ����ʽΪ������
                OrderOfSort = SortOrder.None;
                // ��ʼ��CaseInsensitiveComparer�����
                ObjectCompare = new CaseInsensitiveComparer();
            }


            /// <summary>
            /// ��дIComparer�ӿ�.
            /// </summary>
            /// <param name="x">Ҫ�Ƚϵĵ�һ������</param>
            /// <param name="y">Ҫ�Ƚϵĵڶ�������</param>
            /// <returns>�ȽϵĽ��.�����ȷ���0�����x����y����1�����xС��y����-1</returns>
            public int Compare(object x, object y)
            {
                int compareResult;
                ListViewItem listviewX, listviewY;

                // ���Ƚ϶���ת��ΪListViewItem����
                listviewX = (ListViewItem)x;
                listviewY = (ListViewItem)y;

                // �Ƚ�
                compareResult = ObjectCompare.Compare(OtherHelper.ChangeNullToDoubleZero(listviewX.SubItems[ColumnToSort].Text), OtherHelper.ChangeNullToDoubleZero(listviewY.SubItems[ColumnToSort].Text));

                // ��������ıȽϽ��������ȷ�ıȽϽ��
                if (OrderOfSort == SortOrder.Ascending)
                {
                    // ��Ϊ��������������ֱ�ӷ��ؽ��
                    return compareResult;
                }
                else if (OrderOfSort == SortOrder.Descending)
                {
                    // ����Ƿ�����������Ҫȡ��ֵ�ٷ���ITPUB���˿ռ�8S U"kkY
                    return (-compareResult);
                }
                else
                {
                    // �����ȷ���0
                    return 0;
                }
            }

            /// <summary>
            /// ��ȡ�����ð�����һ������.
            /// </summary>
            public int SortColumn
            {
                set
                {
                    ColumnToSort = value;
                }
                get
                {
                    return ColumnToSort;
                }
            }

            /// <summary>
            /// ��ȡ����������ʽ.
            /// </summary>
            public SortOrder Order
            {
                set
                {
                    OrderOfSort = value;
                }
                get
                {
                    return OrderOfSort;
                }
            }
        }


        public static void LoadAllFont(DevExpress.XtraEditors.ComboBoxEdit cmb)
        {
            //��λ��ϵͳ�����б� 
            cmb.Properties.Items.Clear();
            System.Drawing.Text.InstalledFontCollection fonts = new System.Drawing.Text.InstalledFontCollection();
            foreach (System.Drawing.FontFamily family in fonts.Families)
            {
                cmb.Properties.Items.Add(family.Name);
            }

        }


        public static void SortListViewColumn(object sender, ColumnClickEventArgs e)
        {

            string Asc = ((char)0x25bc).ToString().PadLeft(4, ' ');
            string Des = ((char)0x25b2).ToString().PadLeft(4, ' ');

            ListView lv = sender as ListView;
            if (sort == false)
            {
                sort = true;
                string oldStr = lv.Columns[e.Column].Text.TrimEnd((char)0x25bc, (char)0x25b2, ' ');
                lv.Columns[e.Column].Text = oldStr + Des;
            }
            else if (sort == true)
            {
                sort = false;
                string oldStr = lv.Columns[e.Column].Text.TrimEnd((char)0x25bc, (char)0x25b2, ' ');
                lv.Columns[e.Column].Text = oldStr + Asc;
            }

            // ����������ǲ������ڵ�������.
            if (e.Column == (lv.ListViewItemSorter as ControlStyleHelper.ListViewColumnSorter).SortColumn)
            {
                // �������ô��е����򷽷�
                if ((lv.ListViewItemSorter as ControlStyleHelper.ListViewColumnSorter).Order == SortOrder.Ascending)
                {
                    (lv.ListViewItemSorter as ControlStyleHelper.ListViewColumnSorter).Order = SortOrder.Descending;
                }
                else
                {
                    (lv.ListViewItemSorter as ControlStyleHelper.ListViewColumnSorter).Order = SortOrder.Ascending;
                }
            }
            else
            {
                // ���������У�Ĭ��Ϊ��������
                (lv.ListViewItemSorter as ControlStyleHelper.ListViewColumnSorter).SortColumn = e.Column;
                (lv.ListViewItemSorter as ControlStyleHelper.ListViewColumnSorter).Order = SortOrder.Ascending;
            }
            // ���µ����򷽷���ListView����
            ((ListView)sender).Sort();
        }
    }
}
