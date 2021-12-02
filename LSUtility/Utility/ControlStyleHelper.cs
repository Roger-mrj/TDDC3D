using System.Collections;

using System.Windows.Forms;

namespace RCIS.Utility
{
    public class ControlStyleHelper
    {
        /// <summary>
        /// combox中找到 对应的 值，改变其索引到 这个值 isAlias表示项目文本中是否包含别名
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
        /// 禁止Datagridview排序
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
        /// 初始化Form窗体
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
        /// 初始化DataGridView样式
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
        #region//初始化ListView样式
        /// <summary>
        /// 初始化ListView样式
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

        #region 初始化ToolStrip样式
        /// <summary>
        /// 初始化ToolStrip样式
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
        #region 自定义变量与listview排序有关
        public static int currentCol = -1;
        public static bool sort;
        #endregion
        /// <summary>
        /// 继承自IComparer
        /// </summary>
        public class ListViewColumnSorter : IComparer
        {
            /// <summary>
            /// 指定按照哪个列排序
            /// </summary>
            private int ColumnToSort;
            /// <summary>
            /// 指定排序的方式
            /// </summary>
            private SortOrder OrderOfSort;
            /// <summary>
            /// 声明CaseInsensitiveComparer类对象
            /// </summary>
            private CaseInsensitiveComparer ObjectCompare;

            /// <summary>
            /// 构造函数
            /// </summary>
            public ListViewColumnSorter()
            {
                // 默认按第一列排序
                ColumnToSort = 0;

                // 排序方式为不排序
                OrderOfSort = SortOrder.None;
                // 初始化CaseInsensitiveComparer类对象
                ObjectCompare = new CaseInsensitiveComparer();
            }


            /// <summary>
            /// 重写IComparer接口.
            /// </summary>
            /// <param name="x">要比较的第一个对象</param>
            /// <param name="y">要比较的第二个对象</param>
            /// <returns>比较的结果.如果相等返回0，如果x大于y返回1，如果x小于y返回-1</returns>
            public int Compare(object x, object y)
            {
                int compareResult;
                ListViewItem listviewX, listviewY;

                // 将比较对象转换为ListViewItem对象
                listviewX = (ListViewItem)x;
                listviewY = (ListViewItem)y;

                // 比较
                compareResult = ObjectCompare.Compare(OtherHelper.ChangeNullToDoubleZero(listviewX.SubItems[ColumnToSort].Text), OtherHelper.ChangeNullToDoubleZero(listviewY.SubItems[ColumnToSort].Text));

                // 根据上面的比较结果返回正确的比较结果
                if (OrderOfSort == SortOrder.Ascending)
                {
                    // 因为是正序排序，所以直接返回结果
                    return compareResult;
                }
                else if (OrderOfSort == SortOrder.Descending)
                {
                    // 如果是反序排序，所以要取负值再返回ITPUB个人空间8S U"kkY
                    return (-compareResult);
                }
                else
                {
                    // 如果相等返回0
                    return 0;
                }
            }

            /// <summary>
            /// 获取或设置按照哪一列排序.
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
            /// 获取或设置排序方式.
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
            //如何获得系统字体列表 
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

            // 检查点击的列是不是现在的排序列.
            if (e.Column == (lv.ListViewItemSorter as ControlStyleHelper.ListViewColumnSorter).SortColumn)
            {
                // 重新设置此列的排序方法
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
                // 设置排序列，默认为正向排序
                (lv.ListViewItemSorter as ControlStyleHelper.ListViewColumnSorter).SortColumn = e.Column;
                (lv.ListViewItemSorter as ControlStyleHelper.ListViewColumnSorter).Order = SortOrder.Ascending;
            }
            // 用新的排序方法对ListView排序
            ((ListView)sender).Sort();
        }
    }
}
