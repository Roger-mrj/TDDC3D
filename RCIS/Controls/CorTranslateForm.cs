using System;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections;
using System.IO;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;



namespace ElseTransform
{
    public partial class CorTranslateForm : Form
    {
        public CorTranslateForm()
        {
            InitializeComponent();
        }

        private ArrayList m_needCS=new ArrayList();		//�������飬����Ϊ:6*m_nW;
        private int m_nW=0;

        public ESRI.ArcGIS.Controls.AxMapControl m_MapControl=null ;

        private void CorTranslateForm_Load(object sender, EventArgs e)
        {
            dataGrid1.DataSource = null;
            dataGrid1.DataMember = null;
            dataGrid1.TableStyles.Clear();

            DataTable myTable = new DataTable("��Ϣ��");
            myTable.Columns.Add(new DataColumn("�����"));
            myTable.Columns.Add(new DataColumn("Դ����X"));
            myTable.Columns.Add(new DataColumn("Դ����Y"));
            myTable.Columns.Add(new DataColumn("Ŀ������X"));
            myTable.Columns.Add(new DataColumn("Ŀ������Y"));
            myTable.Columns.Add(new DataColumn("������Vx"));
            myTable.Columns.Add(new DataColumn("������Vy"));

            DataSet myDataSet = new DataSet("MyDataSet");
            myDataSet.Tables.Add(myTable);
            dataGrid1.SetDataBinding(myDataSet, "��Ϣ��");

            DataGridTableStyle dgt = new DataGridTableStyle();
            dgt.MappingName = "��Ϣ��";
            dgt.ReadOnly = true;
            dgt.RowHeadersVisible = false;
            dgt.AlternatingBackColor = System.Drawing.Color.AliceBlue;

            DataGridTextBoxColumn dgtbc = new DataGridTextBoxColumn();
            dgtbc.MappingName = "�����";
            dgtbc.HeaderText = "�����";
            dgt.GridColumnStyles.Add(dgtbc);

            dgtbc = new DataGridTextBoxColumn();
            dgtbc.MappingName = "Դ����X";
            dgtbc.HeaderText = "Դ����X";
            dgt.GridColumnStyles.Add(dgtbc);
            dataGrid1.TableStyles.Add(dgt);

            dgtbc = new DataGridTextBoxColumn();
            dgtbc.MappingName = "Դ����Y";
            dgtbc.HeaderText = "Դ����Y";
            dgt.GridColumnStyles.Add(dgtbc);
            dataGrid1.TableStyles.Add(dgt);

            dgtbc = new DataGridTextBoxColumn();
            dgtbc.MappingName = "Ŀ������X";
            dgtbc.HeaderText = "Ŀ������X";
            dgt.GridColumnStyles.Add(dgtbc);
            dataGrid1.TableStyles.Add(dgt);

            dgtbc = new DataGridTextBoxColumn();
            dgtbc.MappingName = "Ŀ������Y";
            dgtbc.HeaderText = "Ŀ������Y";
            dgt.GridColumnStyles.Add(dgtbc);
            dataGrid1.TableStyles.Add(dgt);

            dgtbc = new DataGridTextBoxColumn();
            dgtbc.MappingName = "������Vx";
            dgtbc.HeaderText = "������Vx";
            dgt.GridColumnStyles.Add(dgtbc);
            dataGrid1.TableStyles.Add(dgt);

            dgtbc = new DataGridTextBoxColumn();
            dgtbc.MappingName = "������Vy";
            dgtbc.HeaderText = "������Vy";
            dgt.GridColumnStyles.Add(dgtbc);
            dataGrid1.TableStyles.Add(dgt);

            myTable.AcceptChanges();
            dataGrid1.Refresh();

            //[02] ��ʼ��tabPage2�е�Shape���·��:
            //С��Ҫ����ڿ�ִ���ļ��ϼ�Ŀ¼��\template��:
            string sExePath = Application.StartupPath;
            int nPos = sExePath.LastIndexOf('\\');
            if (nPos != -1)
            {
                string sNewPath = sExePath.Substring(0, nPos);
                sNewPath = sNewPath + @"\template";
                sExePath = sNewPath;
            }
            textBox1.Text = sExePath;
        }

        private void btnCalParmeters_Click(object sender, EventArgs e)
        {
            //������������dataGrid1:

            //[01] ����û�ѡ��ķ��䷽ʽ:
            string sMode = comboBox1.Text.Trim();
            int nW = -1;
            if (sMode.Equals("һ�׷���[6����]") == true)
                nW = 1;
            else if (sMode.Equals("���׷���[12����]") == true)
                nW = 2;
            if (nW == -1)
            {
                MessageBox.Show("ѡ��ת��ģʽ: һ�׷��䣿���׷���?");
                return;
            }


            //[02] ��dataGrid1�еõ�������:
            IPointCollection FromPs = new MultipointClass();
            IPointCollection ToPs = new MultipointClass();
            object oo = Type.Missing;

            DataSet myDataSet = (DataSet)dataGrid1.DataSource;
            DataTable myTable = myDataSet.Tables[0];
            DataRowCollection myRows = myTable.Rows;
            int nRowGS = myRows.Count;
            for (int i = 0; i < nRowGS; i++)
            {
                DataRow curRow = myRows[i];
                object[] aa = curRow.ItemArray;
                string sXH = (string)aa[0];
                string sX1 = (string)aa[1];
                string sY1 = (string)aa[2];
                string sX2 = (string)aa[3];
                string sY2 = (string)aa[4];

                IPoint p1 = new PointClass();
                p1.X = Convert.ToDouble(sX1);
                p1.Y = Convert.ToDouble(sY1);
                p1.Z = 0.0;
                FromPs.AddPoint(p1, ref oo, ref oo);
                IPoint p2 = new PointClass();
                p2.X = Convert.ToDouble(sX2);
                p2.Y = Convert.ToDouble(sY2);
                p2.Z = 0.0;
                ToPs.AddPoint(p2, ref oo, ref oo);
            }

            if (nW == 1)
            {
                if (FromPs.PointCount < 4)
                {
                    MessageBox.Show("һά�任��������Ҫ���Ե�!");
                    return;
                }
            }
            if (nW == 2)
            {
                if (FromPs.PointCount < 7)
                {
                    MessageBox.Show("��ά�任������Ҫ�٣��Ե�!");
                    return;
                }
            }

            //�������:
            int nDDS = FromPs.PointCount;	//���м���ĵ����
            int nCGS = nW * 6;						//����������һάΪ6������ά����Ϊ12������

            //A����:
            double[,] A = new double[nDDS * 2, nCGS];
            if (nW == 1)
            {
                int nPJS = 0;
                for (int i = 0; i < nDDS * 2; i = i + 2)
                {
                    IPoint pp = FromPs.get_Point(nPJS++);
                    A[i, 0] = pp.X;
                    A[i, 1] = pp.Y;
                    A[i, 2] = 1.0;
                    A[i, 3] = A[i, 4] = A[i, 5] = 0.0;

                    A[i + 1, 0] = A[i + 1, 1] = A[i + 1, 2] = 0.0;
                    A[i + 1, 3] = pp.X;
                    A[i + 1, 4] = pp.Y;
                    A[i + 1, 5] = 1.0;
                }
            }
            else if (nW == 2)
            {
                int nPJS = 0;
                for (int i = 0; i < nDDS * 2; i = i + 2)
                {
                    IPoint pp = FromPs.get_Point(nPJS++);
                    A[i, 0] = pp.X * pp.X;
                    A[i, 1] = pp.X * pp.Y;
                    A[i, 2] = pp.Y * pp.Y;
                    A[i, 3] = pp.X;
                    A[i, 4] = pp.Y;
                    A[i, 5] = 1.0;
                    A[i, 6] = 0.0;
                    A[i, 7] = 0.0;
                    A[i, 8] = 0.0;
                    A[i, 9] = 0.0;
                    A[i, 10] = 0.0;
                    A[i, 11] = 0.0;

                    A[i + 1, 0] = A[i + 1, 1] = A[i + 1, 2] = A[i + 1, 3] = A[i + 1, 4] = A[i + 1, 5] = 0.0;
                    A[i + 1, 6] = pp.X * pp.X;
                    A[i + 1, 7] = pp.X * pp.Y;
                    A[i + 1, 8] = pp.Y * pp.Y;
                    A[i + 1, 9] = pp.X;
                    A[i + 1, 10] = pp.Y;
                    A[i + 1, 11] = 1.0;
                }
            }

            //AT����:
            double[,] AT = new double[nCGS, nDDS * 2];
            for (int i = 0; i < nDDS * 2; i++)
            {
                for (int j = 0; j < nCGS; j++)
                {
                    AT[j, i] = A[i, j];
                }
            }

            //(AT)*A==>ATA
            double[,] ATA = new double[nCGS, nCGS + 1];	//���������1��
            for (int i = 0; i < nCGS; i++)
            {
                for (int j = 0; j < nCGS; j++)
                {
                    ATA[i, j] = 0.0;
                    for (int k = 0; k < nDDS * 2; k++)
                    {
                        ATA[i, j] = ATA[i, j] + AT[i, k] * A[k, j];
                    }
                }
            }

            //L����:
            double[] L = new double[nDDS * 2];
            int nPS = 0;
            for (int i = 0; i < nDDS * 2; i = i + 2)
            {
                IPoint pp = ToPs.get_Point(nPS++);
                L[i] = pp.X;
                L[i + 1] = pp.Y;
            }

            //(At)*L:
            double[] ATL = new double[nCGS];
            for (int i = 0; i < nCGS; i++)
            {
                ATL[i] = 0.0;
                for (int j = 0; j < nDDS * 2; j++)
                {
                    ATL[i] = ATL[i] + AT[i, j] * L[j];
                }
            }

            //���:
            int M = nCGS, N = M + 1;
            for (int i = 0; i < nCGS; i++)
                ATA[i, nCGS] = ATL[i];	//ATA�����һ�з�ATL
            for (int k = 0; k < M; k++)
            {
                double P = ATA[k, k];
                for (int j = k; j < N; j++)
                    ATA[k, j] = ATA[k, j] / P;
                for (int i = 0; i < M; i++)
                {
                    if (i == k) continue;
                    else
                    {
                        double aik = ATA[i, k];
                        for (int j = k; j < N; j++)
                            ATA[i, j] = ATA[i, j] - aik * ATA[k, j];
                    }
                }
            }

            //���:
            double[] needCS = new double[nCGS];
            for (int i = 0; i < nCGS; i++)
                needCS[i] = ATA[i, N - 1];


            //����ÿ��X,Y�ĸ�����: (A*needCS-L)[nCGS,1]
            double[] GZSz = new double[nDDS * 2];
            for (int i = 0; i < nDDS * 2; i++)
            {
                GZSz[i] = 0.0;
                for (int j = 0; j < nCGS; j++)
                {
                    GZSz[i] = GZSz[i] + A[i, j] * needCS[j];
                }
                GZSz[i] = GZSz[i] - L[i];
            }

            //����dataGrid1��Vx,Vy::
            myDataSet = (DataSet)dataGrid1.DataSource;
            myTable = myDataSet.Tables[0];
            myRows = myTable.Rows;
            nRowGS = myRows.Count;
            int nB = 0;
            for (int i = 0; i < nRowGS; i++)
            {
                DataRow curRow = myRows[i];
                object[] aa = curRow.ItemArray;
                aa[5] = (object)GZSz[nB++].ToString("F4");
                aa[6] = (object)GZSz[nB++].ToString("F4");
                curRow.ItemArray = aa;
            }
            dataGrid1.Refresh();

            //�Ѽ�������ӵ�:comboBox2
            comboBox2.Items.Clear();
            string sParaName = "ABCDEFGHIJKL";
            for (int i = 0; i < nCGS; i++)
            {
                string sDisp;
                sDisp = sParaName.Substring(i, 1) + "=" + needCS[i].ToString();
                comboBox2.Items.Add(sDisp);
            }
            comboBox2.SelectedIndex = 0;

            //�Ѳ�������ϵͳ����m_needCS,�����ط�������
            m_needCS.Clear();
            m_needCS.AddRange(needCS);
            m_nW = nW;
        }

        private void btnGetKZDFile_Click(object sender, EventArgs e)
        {
            //�Ҽ�������ĵ���ı��ļ�����ʽӦ��Ϊ:
            //Դ����
            //x1,y1,z2
            //x2,y2,z2
            //...
            //xn,yn,zn
            //Ŀ������
            //x1,y1,z1
            //x2,y2,z2
            //...
            //xn,yn,zn

            string sFilter = "�������ļ�(*.txt)|*.txt";
            openFileDialog1.Filter = sFilter;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor; ;

                string sNeedFile = openFileDialog1.FileName;
                StreamReader sr = new StreamReader(sNeedFile, System.Text.Encoding.GetEncoding("GB2312"));
                ArrayList SourcePSz = new ArrayList();
                ArrayList TargetPSz = new ArrayList();

                string sReadBuffer = sr.ReadLine();
                while (sReadBuffer != null)
                {
                    string s1 = sReadBuffer.Trim();
                    if (s1.Length >= 3 && s1.Substring(0, 3).Equals("Դ����") == true)
                    {
                        string sNewBuffer = sr.ReadLine();
                        while (sNewBuffer != null)
                        {
                            string s2 = sNewBuffer.Trim();
                            if (s2.Length >= 4 && s2.Substring(0, 4).Equals("Ŀ������") == true)
                            {
                                s1 = s2;
                                break;
                            }
                            else
                            {
                                if (s2.Length > 0)
                                    SourcePSz.Add(s2);
                            }
                            sNewBuffer = sr.ReadLine();
                        } //while(sNewBuffer!=null);
                    } //if(s1.Length>=6
                    if (s1.Equals("Ŀ������") == true)
                    {
                        string sNewBuffer = sr.ReadLine();
                        while (sNewBuffer != null)
                        {
                            string s2 = sNewBuffer.Trim();
                            if (s2.Length > 0)
                                TargetPSz.Add(s2);

                            sNewBuffer = sr.ReadLine();
                        } //while(sNewBuffer!=null);
                    }

                    sReadBuffer = sr.ReadLine();
                } //while(sReadBuffer!=null)
                sr.Close();

                if (SourcePSz.Count != TargetPSz.Count)
                {
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                    MessageBox.Show("������Ƶ�Ը�����ƥ��!");
                    return;
                }
                for (int i = 0; i < SourcePSz.Count; i++)
                {
                    string sCor = SourcePSz[i] as string;
                    int[] nCommaSz = new int[5];
                    int nA = 0;
                    for (int k = 0; k < sCor.Length; k++)
                    {
                        if (sCor[k].Equals(',') == true)
                            nCommaSz[nA++] = k;
                    }
                    if (nA == 1 || nA == 2) ;
                    else
                    {
                        this.Cursor = System.Windows.Forms.Cursors.Default;
                        MessageBox.Show("Դ������: '" + sCor + "' ��ʽ����!");
                        return;
                    }
                    string sX, sY;
                    if (nA == 1)
                    {
                        sX = sCor.Substring(0, nCommaSz[0]).Trim();
                        sY = sCor.Substring(nCommaSz[0] + 1);
                    }
                    else
                    {
                        sX = sCor.Substring(0, nCommaSz[0]).Trim();
                        sY = sCor.Substring(nCommaSz[0] + 1, nCommaSz[1] - nCommaSz[0] - 1).Trim();
                    }
                    IPoint pp = new PointClass();
                    pp.X = Convert.ToDouble(sX);
                    pp.Y = Convert.ToDouble(sY);
                    pp.Z = 0.0;
                    SourcePSz[i] = pp;
                }
                for (int i = 0; i < TargetPSz.Count; i++)
                {
                    string sCor = TargetPSz[i] as string;
                    int[] nCommaSz = new int[5];
                    int nA = 0;
                    for (int k = 0; k < sCor.Length; k++)
                    {
                        if (sCor[k].Equals(',') == true)
                            nCommaSz[nA++] = k;
                    }
                    if (nA == 1 || nA == 2) ;
                    else
                    {
                        this.Cursor = System.Windows.Forms.Cursors.Default;
                        MessageBox.Show("Ŀ��������: '" + sCor + "' ��ʽ����!");
                        return;
                    }
                    string sX, sY;
                    if (nA == 1)
                    {
                        sX = sCor.Substring(0, nCommaSz[0]).Trim();
                        sY = sCor.Substring(nCommaSz[0] + 1);
                    }
                    else
                    {
                        sX = sCor.Substring(0, nCommaSz[0]).Trim();
                        sY = sCor.Substring(nCommaSz[0] + 1, nCommaSz[1] - nCommaSz[0] - 1).Trim();
                    }
                    IPoint pp = new PointClass();
                    pp.X = Convert.ToDouble(sX);
                    pp.Y = Convert.ToDouble(sY);
                    pp.Z = 0.0;
                    TargetPSz[i] = pp;
                }


                //�������Լӵ�dataGrid1��ʾ:
                dataGrid1.DataSource = null;
                dataGrid1.DataMember = null;
                dataGrid1.TableStyles.Clear();

                DataTable myTable = new DataTable("��Ϣ��");
                myTable.Columns.Add(new DataColumn("�����"));
                myTable.Columns.Add(new DataColumn("Դ����X"));
                myTable.Columns.Add(new DataColumn("Դ����Y"));
                myTable.Columns.Add(new DataColumn("Ŀ������X"));
                myTable.Columns.Add(new DataColumn("Ŀ������Y"));
                myTable.Columns.Add(new DataColumn("������Vx"));
                myTable.Columns.Add(new DataColumn("������Vy"));

                DataSet myDataSet = new DataSet("MyDataSet");
                myDataSet.Tables.Add(myTable);
                dataGrid1.SetDataBinding(myDataSet, "��Ϣ��");

                DataGridTableStyle dgt = new DataGridTableStyle();
                dgt.MappingName = "��Ϣ��";
                dgt.ReadOnly = true;
                dgt.RowHeadersVisible = false;
                dgt.AlternatingBackColor = System.Drawing.Color.AliceBlue;

                DataGridTextBoxColumn dgtbc = new DataGridTextBoxColumn();
                dgtbc.MappingName = "�����";
                dgtbc.HeaderText = "�����";
                dgt.GridColumnStyles.Add(dgtbc);

                dgtbc = new DataGridTextBoxColumn();
                dgtbc.MappingName = "Դ����X";
                dgtbc.HeaderText = "Դ����X";
                dgt.GridColumnStyles.Add(dgtbc);
                dataGrid1.TableStyles.Add(dgt);

                dgtbc = new DataGridTextBoxColumn();
                dgtbc.MappingName = "Դ����Y";
                dgtbc.HeaderText = "Դ����Y";
                dgt.GridColumnStyles.Add(dgtbc);
                dataGrid1.TableStyles.Add(dgt);

                dgtbc = new DataGridTextBoxColumn();
                dgtbc.MappingName = "Ŀ������X";
                dgtbc.HeaderText = "Ŀ������X";
                dgt.GridColumnStyles.Add(dgtbc);
                dataGrid1.TableStyles.Add(dgt);

                dgtbc = new DataGridTextBoxColumn();
                dgtbc.MappingName = "Ŀ������Y";
                dgtbc.HeaderText = "Ŀ������Y";
                dgt.GridColumnStyles.Add(dgtbc);
                dataGrid1.TableStyles.Add(dgt);

                dgtbc = new DataGridTextBoxColumn();
                dgtbc.MappingName = "������Vx";
                dgtbc.HeaderText = "������Vx";
                dgt.GridColumnStyles.Add(dgtbc);
                dataGrid1.TableStyles.Add(dgt);

                dgtbc = new DataGridTextBoxColumn();
                dgtbc.MappingName = "������Vy";
                dgtbc.HeaderText = "������Vy";
                dgt.GridColumnStyles.Add(dgtbc);
                dataGrid1.TableStyles.Add(dgt);

                for (int i = 0; i < SourcePSz.Count; i++)
                {
                    DataRow row = myTable.NewRow();

                    int nXH = i + 1;
                    string sXH;
                    sXH = nXH.ToString();
                    row["�����"] = sXH;

                    string sX = ((IPoint)SourcePSz[i]).X.ToString("F3");
                    row["Դ����X"] = sX;
                    string sY = ((IPoint)SourcePSz[i]).Y.ToString("F3");
                    row["Դ����Y"] = sY;

                    sX = ((IPoint)TargetPSz[i]).X.ToString("F3");
                    row["Ŀ������X"] = sX;
                    sY = ((IPoint)TargetPSz[i]).Y.ToString("F3");
                    row["Ŀ������Y"] = sY;

                    row["������Vx"] = "n/a";
                    row["������Vy"] = "n/a";

                    myTable.Rows.Add(row);
                }
                myTable.AcceptChanges();
                dataGrid1.Refresh();

                this.Cursor = System.Windows.Forms.Cursors.Default;
                //... ...
            }		
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            string sHelp = "[01] '���Ƶ�������ļ�' �����ʽ:\r\n" +
                "Դ����" + "\r\n" +
                "X1,Y1(,Z1)" + "\r\n" +
                "... ..." + "\r\n" +
                "Xn,Yn(,Zn)" + "\r\n" +
                "Ŀ������" + "\r\n" +
                "X1,Y1(,Z1)" + "\r\n" +
                "... ..." + "\r\n" +
                "Xn,Yn(,Zn)" + "\r\n" +
                "ע��: ���ļ��е�'Դ����'��'Ŀ������'������,Z������п���!" + "\r\n" +

                "[02] 'ת���ļ�' ��ť�Ĺ�����: ��һ���ı��ļ��ڵ�����Դ����ת����Ŀ������." + "\r\n" +
                "�ı��ļ��ĸ�������:" + "\r\n" +
                "X1,Y1(,Z1)" + "\r\n" +
                "... ..." + "\r\n" +
                "Xn,Yn(,Zn)" + "\r\n\r\n";
            MessageBox.Show(sHelp, "Help", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);	
        }

        private void btnCalParmeters_Click_1(object sender, EventArgs e)
        {
            //������������dataGrid1:

            string sMode = comboBox1.Text.Trim();
            int nW = -1;
            if (sMode.Equals("һ�׷���[6����]") == true)
                nW = 1;
            else if (sMode.Equals("���׷���[12����]") == true)
                nW = 2;
            if (nW == -1)
            {
                MessageBox.Show("ѡ��ת��ģʽ: һ�׷��䣿���׷���?");
                return;
            }

            IPointCollection FromPs = new MultipointClass();
            IPointCollection ToPs = new MultipointClass();
            object oo = Type.Missing;

            DataSet myDataSet = (DataSet)dataGrid1.DataSource;
            DataTable myTable = myDataSet.Tables[0];
            DataRowCollection myRows = myTable.Rows;
            int nRowGS = myRows.Count;
            for (int i = 0; i < nRowGS; i++)
            {
                DataRow curRow = myRows[i];
                object[] aa = curRow.ItemArray;
                string sXH = (string)aa[0];
                string sX1 = (string)aa[1];
                string sY1 = (string)aa[2];
                string sX2 = (string)aa[3];
                string sY2 = (string)aa[4];

                IPoint p1 = new PointClass();
                p1.X = Convert.ToDouble(sX1);
                p1.Y = Convert.ToDouble(sY1);
                p1.Z = 0.0;
                FromPs.AddPoint(p1, ref oo, ref oo);
                IPoint p2 = new PointClass();
                p2.X = Convert.ToDouble(sX2);
                p2.Y = Convert.ToDouble(sY2);
                p2.Z = 0.0;
                ToPs.AddPoint(p2, ref oo, ref oo);
            }

            if (nW == 1)
            {
                if (FromPs.PointCount < 4)
                {
                    MessageBox.Show("һά�任��������Ҫ���Ե�!");
                    return;
                }
            }
            if (nW == 2)
            {
                if (FromPs.PointCount < 7)
                {
                    MessageBox.Show("��ά�任������Ҫ�٣��Ե�!");
                    return;
                }
            }

            int nDDS = FromPs.PointCount;
            int nCGS = nW * 6;
            double[,] A = new double[nDDS * 2, nCGS];
            if (nW == 1)
            {
                int nPJS = 0;
                for (int i = 0; i < nDDS * 2; i = i + 2)
                {
                    IPoint pp = FromPs.get_Point(nPJS++);
                    A[i, 0] = pp.X;
                    A[i, 1] = pp.Y;
                    A[i, 2] = 1.0;
                    A[i, 3] = A[i, 4] = A[i, 5] = 0.0;

                    A[i + 1, 0] = A[i + 1, 1] = A[i + 1, 2] = 0.0;
                    A[i + 1, 3] = pp.X;
                    A[i + 1, 4] = pp.Y;
                    A[i + 1, 5] = 1.0;
                }
            }
            else if (nW == 2)
            {
                int nPJS = 0;
                for (int i = 0; i < nDDS * 2; i = i + 2)
                {
                    IPoint pp = FromPs.get_Point(nPJS++);
                    A[i, 0] = pp.X * pp.X;
                    A[i, 1] = pp.X * pp.Y;
                    A[i, 2] = pp.Y * pp.Y;
                    A[i, 3] = pp.X;
                    A[i, 4] = pp.Y;
                    A[i, 5] = 1.0;
                    A[i, 6] = 0.0;
                    A[i, 7] = 0.0;
                    A[i, 8] = 0.0;
                    A[i, 9] = 0.0;
                    A[i, 10] = 0.0;
                    A[i, 11] = 0.0;

                    A[i + 1, 0] = A[i + 1, 1] = A[i + 1, 2] = A[i + 1, 3] = A[i + 1, 4] = A[i + 1, 5] = 0.0;
                    A[i + 1, 6] = pp.X * pp.X;
                    A[i + 1, 7] = pp.X * pp.Y;
                    A[i + 1, 8] = pp.Y * pp.Y;
                    A[i + 1, 9] = pp.X;
                    A[i + 1, 10] = pp.Y;
                    A[i + 1, 11] = 1.0;
                }
            }

            double[,] AT = new double[nCGS, nDDS * 2];
            for (int i = 0; i < nDDS * 2; i++)
            {
                for (int j = 0; j < nCGS; j++)
                {
                    AT[j, i] = A[i, j];
                }
            }

            //(AT)*A==>ATA
            double[,] ATA = new double[nCGS, nCGS + 1];	//���������1��
            for (int i = 0; i < nCGS; i++)
            {
                for (int j = 0; j < nCGS; j++)
                {
                    ATA[i, j] = 0.0;
                    for (int k = 0; k < nDDS * 2; k++)
                    {
                        ATA[i, j] = ATA[i, j] + AT[i, k] * A[k, j];
                    }
                }
            }

            //L����:
            double[] L = new double[nDDS * 2];
            int nPS = 0;
            for (int i = 0; i < nDDS * 2; i = i + 2)
            {
                IPoint pp = ToPs.get_Point(nPS++);
                L[i] = pp.X;
                L[i + 1] = pp.Y;
            }

            //(At)*L:
            double[] ATL = new double[nCGS];
            for (int i = 0; i < nCGS; i++)
            {
                ATL[i] = 0.0;
                for (int j = 0; j < nDDS * 2; j++)
                {
                    ATL[i] = ATL[i] + AT[i, j] * L[j];
                }
            }

            //���:
            int M = nCGS, N = M + 1;
            for (int i = 0; i < nCGS; i++)
                ATA[i, nCGS] = ATL[i];	//ATA�����һ�з�ATL
            for (int k = 0; k < M; k++)
            {
                double P = ATA[k, k];
                for (int j = k; j < N; j++)
                    ATA[k, j] = ATA[k, j] / P;
                for (int i = 0; i < M; i++)
                {
                    if (i == k) continue;
                    else
                    {
                        double aik = ATA[i, k];
                        for (int j = k; j < N; j++)
                            ATA[i, j] = ATA[i, j] - aik * ATA[k, j];
                    }
                }
            }

            //���:
            double[] needCS = new double[nCGS];
            for (int i = 0; i < nCGS; i++)
                needCS[i] = ATA[i, N - 1];


            double[] GZSz = new double[nDDS * 2];
            for (int i = 0; i < nDDS * 2; i++)
            {
                GZSz[i] = 0.0;
                for (int j = 0; j < nCGS; j++)
                {
                    GZSz[i] = GZSz[i] + A[i, j] * needCS[j];
                }
                GZSz[i] = GZSz[i] - L[i];
            }

            myDataSet = (DataSet)dataGrid1.DataSource;
            myTable = myDataSet.Tables[0];
            myRows = myTable.Rows;
            nRowGS = myRows.Count;
            int nB = 0;
            for (int i = 0; i < nRowGS; i++)
            {
                DataRow curRow = myRows[i];
                object[] aa = curRow.ItemArray;
                aa[5] = (object)GZSz[nB++].ToString("F4");
                aa[6] = (object)GZSz[nB++].ToString("F4");
                curRow.ItemArray = aa;
            }
            dataGrid1.Refresh();

            comboBox2.Items.Clear();
            string sParaName = "ABCDEFGHIJKL";
            for (int i = 0; i < nCGS; i++)
            {
                string sDisp;
                sDisp = sParaName.Substring(i, 1) + "=" + needCS[i].ToString();
                comboBox2.Items.Add(sDisp);
            }
            comboBox2.SelectedIndex = 0;

            m_needCS.Clear();
            m_needCS.AddRange(needCS);
            m_nW = nW;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //����m_needCS�м�¼�Ĳ��������û�ѡ���������ļ�����ת����
            //�����ļ��ĸ�ʽΪ:
            //		x1,y1(,z1)
            //		... ...
            //		xn,yn(,zn)
            if (m_nW == 0 || m_needCS.Count == 0)
            {
                MessageBox.Show("�ȼ��������Ȼ����ʹ�ñ���ť�Ĺ���!");
                return;
            }

            string sFilter = "��ת���������ļ�(*.txt)|*.txt";
            openFileDialog1.Filter = sFilter;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;


                string sCorFile = openFileDialog1.FileName;
                StreamReader sr = new StreamReader(sCorFile, System.Text.Encoding.GetEncoding("GB2312"));

                ArrayList CorSz = new ArrayList();
                string sReadBuffer = sr.ReadLine();
                while (sReadBuffer != null)
                {
                    string s1 = sReadBuffer.Trim();

                    int[] nCommaSz = new int[5];
                    int nA = 0;
                    for (int k = 0; k < s1.Length; k++)
                    {
                        if (s1.Substring(k, 1) == ",")
                            nCommaSz[nA++] = k;
                    }

                    if (nA == 1)
                    {
                        //1��","��ֻ��XY
                        string sX = s1.Substring(0, nCommaSz[0]);
                        string sY = s1.Substring(nCommaSz[0] + 1);
                        IPoint pp = new PointClass();
                        pp.X = Convert.ToDouble(sX);
                        pp.Y = Convert.ToDouble(sY);

                        CorSz.Add(pp);
                    }
                    else if (nA == 2)
                    {
                        //2��","��XYZ:
                        string sX = s1.Substring(0, nCommaSz[0]);
                        string sY = s1.Substring(nCommaSz[0] + 1, nCommaSz[1] - nCommaSz[0] - 1);
                        IPoint pp = new PointClass();
                        pp.X = Convert.ToDouble(sX);
                        pp.Y = Convert.ToDouble(sY);

                        CorSz.Add(pp);
                    }

                    sReadBuffer = sr.ReadLine();
                }
                sr.Close();
                if (CorSz.Count == 0)
                {
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                    MessageBox.Show("�������ļ��м��������κε�����!");
                    return;
                }

                //����m_needCS��CorSz�е�����任:
                double dA, dB, dC, dD, dE, dF, dG, dH, dI, dJ, dK, dL;
                dA = dB = dC = dD = dE = dF = dG = dH = dI = dJ = dK = dL = 0.0;
                if (m_nW == 1)
                {
                    dA = (double)m_needCS[0];
                    dB = (double)m_needCS[1];
                    dC = (double)m_needCS[2];

                    dD = (double)m_needCS[3];
                    dE = (double)m_needCS[4];
                    dF = (double)m_needCS[5];
                }
                else if (m_nW == 2)
                {
                    dA = (double)m_needCS[0];
                    dB = (double)m_needCS[1];
                    dC = (double)m_needCS[2];
                    dD = (double)m_needCS[3];
                    dE = (double)m_needCS[4];
                    dF = (double)m_needCS[5];

                    dG = (double)m_needCS[6];
                    dH = (double)m_needCS[7];
                    dI = (double)m_needCS[8];
                    dJ = (double)m_needCS[9];
                    dK = (double)m_needCS[10];
                    dL = (double)m_needCS[11];
                }

                int nC = sCorFile.LastIndexOf("\\");
                string sNewCorFile = sCorFile.Substring(0, nC) + @"\ת���������ļ�.TXT";

                StreamWriter sw = new StreamWriter(sNewCorFile);
                for (int i = 0; i < CorSz.Count; i++)
                {
                    IPoint pp = CorSz[i] as IPoint;
                    double X = pp.X;
                    double Y = pp.Y;

                    if (m_nW == 1)
                    {
                        double dNewX = dA * X + dB * Y + dC;
                        double dNewY = dD * X + dE * Y + dF;
                        sw.WriteLine("(" + X.ToString("F3") + "," + Y.ToString("F3") + ") --> (" + dNewX.ToString("F3") + "," + dNewY.ToString("F3") + ")");
                    }
                    else
                    {
                        double dNewX = dA * X * X + dB * X * Y + dC * Y * Y + dD * X + dE * Y + dF;
                        double dNewY = dG * X * X + dH * X * Y + dI * Y * Y + dJ * X + dK * Y + dL;
                        sw.WriteLine("(" + X.ToString("F3") + "," + Y.ToString("F3") + ") --> (" + dNewX.ToString("F3") + "," + dNewY.ToString("F3") + ")");
                    }
                }
                //�Ѳ���ģ��Ҳд��:
                sw.WriteLine("");
                sw.WriteLine("��ʾ: ������߱任�����µĲ���ģ��");
                if (m_nW == 2)
                {
                    sw.WriteLine(" Xnew=A X^2+B XY+C Y^2+D X+E Y+F");
                    sw.WriteLine(" Ynew=G X^2+H XY+I Y^2+J X+K Y+L");
                    sw.WriteLine("");
                    sw.WriteLine("���տ��Ƶ���ļ�����ĸ���������:");
                    sw.WriteLine(" A=" + dA.ToString());
                    sw.WriteLine(" B=" + dB.ToString());
                    sw.WriteLine(" C=" + dC.ToString());
                    sw.WriteLine(" D=" + dD.ToString());
                    sw.WriteLine(" E=" + dE.ToString());
                    sw.WriteLine(" F=" + dF.ToString());
                    sw.WriteLine(" G=" + dG.ToString());
                    sw.WriteLine(" H=" + dH.ToString());
                    sw.WriteLine(" I=" + dI.ToString());
                    sw.WriteLine(" J=" + dJ.ToString());
                    sw.WriteLine(" K=" + dK.ToString());
                    sw.WriteLine(" L=" + dL.ToString());
                }
                if (m_nW == 1)
                {
                    sw.WriteLine(" Xnew=A X+B Y+C");
                    sw.WriteLine(" Ynew=D X+E Y+F");
                    sw.WriteLine("");
                    sw.WriteLine("���� '���Ƶ���ļ�' ����ĸ���������:");
                    sw.WriteLine(" A=" + dA.ToString());
                    sw.WriteLine(" B=" + dB.ToString());
                    sw.WriteLine(" C=" + dC.ToString());
                    sw.WriteLine(" D=" + dD.ToString());
                    sw.WriteLine(" E=" + dE.ToString());
                    sw.WriteLine(" F=" + dF.ToString());
                }
                sw.Close();

                //

                //�����ļ�sNewCorFile:
                Process myProcess = new Process();
                myProcess.StartInfo.FileName = sNewCorFile;
                myProcess.Start();
                //Dim myprocess As System.Diagnostics.Process = New
                //System.Diagnostics.Process()
                //myprocess.StartInfo.FileName = "whatever.exe"
                //myprocess.StartInfo.WorkingDirectory = "C:\Program Files\program directory\"
                //myprocess.StartInfo.Arguments= "-E" 'put your command line parameters in
                //here
                //myprocess.Start()		

                this.Cursor = System.Windows.Forms.Cursors.Default;
                return;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            //��:
            if (radioButton1.Checked == true)
            {
                try
                {
                    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                    listBox1.Items.Clear();

                    //�õ���ǰMap��ͼ����õ�listBox1:
                    IMap myMap = m_MapControl.ActiveView.FocusMap;
                    for (int i = 0; i < myMap.LayerCount; i++)
                    {
                        ILayer lyr = myMap.get_Layer(i);
                        if (lyr.Valid == true)
                        {
                            if (lyr is IFeatureLayer)
                            {
                                IFeatureLayer flyr = lyr as IFeatureLayer;
                                string sName = lyr.Name.Trim();
                                if (flyr.FeatureClass.ShapeType == esriGeometryType.esriGeometryPoint)
                                    listBox1.Items.Add(sName);
                            }
                            else if (lyr is IGroupLayer)
                            {
                                ICompositeLayer compositeLyr = lyr as ICompositeLayer;
                                for (int kk = 0; kk < compositeLyr.Count; kk++)
                                {
                                    ILayer childLyr = compositeLyr.get_Layer(kk);
                                    if (childLyr is IFeatureLayer)
                                    {
                                        IFeatureLayer flyr = childLyr as IFeatureLayer;
                                        string sName = lyr.Name.Trim();
                                        if (flyr.FeatureClass.ShapeType == esriGeometryType.esriGeometryPoint)
                                            listBox1.Items.Add(sName);
                                    }
                                }
                            }
                            
                           
                        }
                    }

                    this.Cursor = System.Windows.Forms.Cursors.Default;
                    textBox2.Text = "CorTranslate_Point.shp";
                }
                catch (Exception E)
                {
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                    MessageBox.Show(E.Message);
                    return;
                }
            }

        }


        void GetNewXY(PointClass InP,out PointClass OutP)
		{
			double dA,dB,dC,dD,dE,dF,dG,dH,dI,dJ,dK,dL;
			dA=dB=dC=dD=dE=dF=dG=dH=dI=dJ=dK=dL=0.0;
			if(m_nW==1) 
			{
				dA=(double)m_needCS[0];
				dB=(double)m_needCS[1];
				dC=(double)m_needCS[2];

				dD=(double)m_needCS[3];
				dE=(double)m_needCS[4];
				dF=(double)m_needCS[5];
			} 
			else if(m_nW==2) 
			{
				dA=(double)m_needCS[0];
				dB=(double)m_needCS[1];
				dC=(double)m_needCS[2];
				dD=(double)m_needCS[3];
				dE=(double)m_needCS[4];
				dF=(double)m_needCS[5];

				dG=(double)m_needCS[6];
				dH=(double)m_needCS[7];
				dI=(double)m_needCS[8];
				dJ=(double)m_needCS[9];
				dK=(double)m_needCS[10];
				dL=(double)m_needCS[11];
			}

			double X=InP.X;
			double Y=InP.Y;
			OutP=new PointClass();
			if(m_nW==1) 
			{
				double dNewX=dA*X+dB*Y+dC;
				double dNewY=dD*X+dE*Y+dF;
				OutP.X=dNewX;
				OutP.Y=dNewY;
			} 
			else 
			{
				double dNewX=dA*X*X+dB*X*Y+dC*Y*Y+dD*X+dE*Y+dF;
				double dNewY=dG*X*X+dH*X*Y+dI*Y*Y+dJ*X+dK*Y+dL;
				OutP.X=dNewX;
				OutP.Y=dNewY;
			}
			return;
		}

        private void btntransformLayer_Click(object sender, EventArgs e)
        {
            //��ѡ�е�FeatureClassִ������任:

			//������Ƿ��Ѿ������˲���:
			if(m_nW>=1 && m_nW<=2);
			else 
			{
				MessageBox.Show("�ȼ������!");
				return;
			}
			if(m_needCS.Count==0) 
			{
				MessageBox.Show("�ȼ������!");
				return;
			}


			this.Cursor=System.Windows.Forms.Cursors.WaitCursor;
			try 
			{
				IMap myMap=m_MapControl.ActiveView.FocusMap;

				//[01] ���·���Ƿ����:
				string sPath=textBox1.Text.Trim();
				if(Directory.Exists(sPath)==false) 
				{
					this.Cursor=System.Windows.Forms.Cursors.Default;
					MessageBox.Show("ָ�������·�������� !");
					return;
				}

				//[02] �淶��ShapeFileName:
				string sShapeFileName=textBox2.Text.Trim();
				int nLen=sShapeFileName.Length;
				int nGS=0;
				for(int i=0;i<nLen;i++) 
				{
					if(sShapeFileName[i]=='.')
						nGS++;
				}
				if(nGS!=1) 
				{
					this.Cursor=System.Windows.Forms.Cursors.Default;
					MessageBox.Show("SHAPE�ļ����Ǹ�ʽ[A] !");
					return;
				}
				int nPos=sShapeFileName.LastIndexOf(".");
				string sExt=sShapeFileName.Substring(nPos+1).Trim().ToUpper();
				if(sExt.Equals("SHP")==false) 
				{
					this.Cursor=System.Windows.Forms.Cursors.Default;
					MessageBox.Show("SHAPE�ļ����Ǹ�ʽ[B] !");
					return;
				}

				//[03] �õ���ǰҪת����FeatureClass: transFC & selGeometryType
				object oo=listBox1.SelectedItem;
				if(oo==null) 
				{
					this.Cursor=System.Windows.Forms.Cursors.Default;
					MessageBox.Show(" ����ߵ��б���ڡ���ѡ��Ҫת����ͼ��! ");
					return;
				}
				string sNeedLayerName=(string)oo;
				sNeedLayerName=sNeedLayerName.Trim().ToUpper();
				esriGeometryType selGeometryType=esriGeometryType.esriGeometryNull;
				if(radioButton1.Checked==true)
					selGeometryType=esriGeometryType.esriGeometryPoint;
				else if(radioButton2.Checked==true)
					selGeometryType=esriGeometryType.esriGeometryPolyline;
				else if(radioButton3.Checked==true)
					selGeometryType=esriGeometryType.esriGeometryPolygon;
				if(selGeometryType==esriGeometryType.esriGeometryNull) 
				{
					this.Cursor=System.Windows.Forms.Cursors.Default;
					MessageBox.Show(" ��ת����ͼ��ļ�������? ");
					return;
				}
				IFeatureClass transFC=null;

                IFeatureLayer transLayer = RCIS.GISCommon.LayerHelper.QueryLayerByModelName(myMap, sNeedLayerName);

                if (transLayer == null) 
				{
					this.Cursor=System.Windows.Forms.Cursors.Default;
					MessageBox.Show(" û�ҵ���Ҫת����������! ");
					return;
				}
                transFC = transLayer.FeatureClass;

				//[04] ���û�ָ����·���½���SHAPE�ļ�:
				double dXMin=0.0,dYMin=0.0,dZMin=0.0;
				double dPrecision=100.0;
				ISpatialReference pSR=new UnknownCoordinateSystemClass();
				pSR.SetFalseOriginAndUnits(dXMin,dYMin,dPrecision);	//��Shape�ļ���˵�������û��
				pSR.SetZFalseOriginAndUnits(dZMin,dPrecision);			//
				pSR.SetMFalseOriginAndUnits(dZMin,dPrecision);			//

				esriGeometryType NewFeatureClassType=esriGeometryType.esriGeometryNull;		//!!
				if(radioButton1.Checked==true) 
					NewFeatureClassType=esriGeometryType.esriGeometryPoint;
				else if(radioButton2.Checked==true)
					NewFeatureClassType=esriGeometryType.esriGeometryPolyline;
				else if(radioButton3.Checked==true)
					NewFeatureClassType=esriGeometryType.esriGeometryPolygon;

				nPos=sShapeFileName.LastIndexOf(".");
				nLen=sShapeFileName.Length;
				string sNewFeatureClassName=sShapeFileName.Substring(0,nPos);	//!!

				IWorkspaceFactory pWSF=new ShapefileWorkspaceFactoryClass();
				IFeatureWorkspace pWS=pWSF.OpenFromFile(sPath,0) as IFeatureWorkspace;

				IEnumDataset pEnumDataset=((IWorkspace)pWS).get_Datasets(esriDatasetType.esriDTFeatureClass);
				pEnumDataset.Reset();
				IDataset pDataset=pEnumDataset.Next();
				while(pDataset!=null) 
				{
					string sName=pDataset.Name;
					if(sName.Equals(sNewFeatureClassName)==true)
						pDataset.Delete();
					pDataset=pEnumDataset.Next();
				}
				IFields pFields=new ESRI.ArcGIS.Geodatabase.FieldsClass();
				IFieldsEdit pFieldsEdit=pFields as IFieldsEdit;
				IField pField=new ESRI.ArcGIS.Geodatabase.FieldClass();
				IFieldEdit pFieldEdit= pField as IFieldEdit;
				pFieldEdit.Name_2="OBJECTID";
				pFieldEdit.Type_2=esriFieldType.esriFieldTypeOID;
				pFieldsEdit.AddField(pField);
				pField=new FieldClass();
				pFieldEdit=pField as IFieldEdit;
				pFieldEdit.Name_2="Shape";
				pFieldEdit.Type_2=esriFieldType.esriFieldTypeGeometry;
				IGeometryDef pGeoDef=new GeometryDefClass();
				IGeometryDefEdit pGeoDefEdit=pGeoDef as IGeometryDefEdit;
				pGeoDefEdit.SpatialReference_2=pSR;
				pGeoDefEdit.GridCount_2=1;
				pGeoDefEdit.set_GridSize(0,0.5);
				pGeoDefEdit.AvgNumPoints_2=2;
				pGeoDefEdit.HasM_2=false;
				pGeoDefEdit.HasZ_2=false;
				pGeoDefEdit.GeometryType_2=NewFeatureClassType;
				pFieldEdit.GeometryDef_2=pGeoDef;
				pFieldsEdit.AddField(pField);
				string sShapeFieldName="";
				for(int i=0;i<pFields.FieldCount;i++) 
				{
					if(pFields.get_Field(i).Type==esriFieldType.esriFieldTypeGeometry) 
					{
						sShapeFieldName=pFields.get_Field(i).Name;
						break;
					}
				}
				IFeatureClass targetFC=pWS.CreateFeatureClass(sNewFeatureClassName,pFields,null,null,esriFeatureType.esriFTSimple,sShapeFieldName,"");
				IDataset ds=transFC as IDataset;
				string sSourceTable=ds.Name;
				string sTargetTable=sNewFeatureClassName;

				IFeatureWorkspace pFWS1=ds.Workspace as IFeatureWorkspace;
				IWorkspaceEdit pWSE1=pFWS1 as IWorkspaceEdit;
				IFeatureWorkspace pFWS2=pWS as IFeatureWorkspace;
				IWorkspaceEdit pWSE2=pFWS2 as IWorkspaceEdit;

				if(pWSE2.IsBeingEdited()==false)
					pWSE2.StartEditing(true);

				int nRcds=1;
				ITable pTable1=pFWS1.OpenTable(sSourceTable);
				int nSumRcds=pTable1.RowCount(null);
				
				//������Ϣ:
				Tips.Visible=true;
				SourceFC.Text="Դ: "+sNeedLayerName;
				TargetFC.Text="SHAPE�ļ�: "+sShapeFileName;
				progressBar1.Maximum=nSumRcds;
				progressBar1.Minimum=0;
				int nTipsGS=1;
				Application.DoEvents();

				ITable pTable2=pFWS2.OpenTable(sTargetTable);
				IQueryFilter qry=new QueryFilterClass();
				ICursor pRows=pTable1.Search(qry,false);
				IRow pCurRow=pRows.NextRow();
				while(pCurRow!=null) 
				{
					ICursor pCursor=pTable2.Insert(true);
					IRowBuffer pBuffer=pTable2.CreateRowBuffer();

					IFields flds1=pCurRow.Fields;
					IFields flds2=pBuffer.Fields;
					bool bNeedProcess=false;
					for(int j=0;j<flds1.FieldCount;j++) 
					{
						IField fld1=flds1.get_Field(j);
						if(fld1.Type==esriFieldType.esriFieldTypeGeometry) 
						{
							oo=pCurRow.get_Value(j);
							if(oo is System.DBNull)
								continue;
							IGeometry Geo=oo as IGeometry;
							if(Geo.IsEmpty==true)
								continue;

							int nShapePos=flds2.FindField("SHAPE");
							if(selGeometryType==esriGeometryType.esriGeometryPoint) 
							{
								IPoint pP=oo as IPoint;
								PointClass curP=new PointClass();
								curP.X=pP.X;
								curP.Y=pP.Y;
								PointClass newP=null;
								GetNewXY(curP,out newP);
								
								pBuffer.set_Value(nShapePos,(object)(newP));
								bNeedProcess=true;
							} 
							if(selGeometryType==esriGeometryType.esriGeometryPolyline)
							{
								IPolyline curLine=oo as IPolyline;
								IGeometryCollection curGC=curLine as IGeometryCollection;
								PolylineClass newLine=new PolylineClass();
								IGeometryCollection newGC=newLine as IGeometryCollection;
								nGS=curGC.GeometryCount;
								for(int j2=0;j2<nGS;j2++) 
								{
									IPath tmpPath=curGC.get_Geometry(j2) as IPath;
									IPointCollection tmpPC=tmpPath as IPointCollection;

									PolylineClass tmpLine=new PolylineClass();
									IPointCollection tmpPC2=tmpLine as IPointCollection;
									for(int k=0;k<tmpPC.PointCount;k++) 
									{
										IPoint pp=tmpPC.get_Point(k);
                                                                                
										PointClass curP=new PointClass();
										curP.X=pp.X;
										curP.Y=pp.Y;
										PointClass newP=null;
										GetNewXY(curP,out newP);
                                        

										object o1=Type.Missing;
										object o2=Type.Missing;
										tmpPC2.AddPoint((IPoint)newP,ref o1,ref o2);
									} //for(int k=0;...

									IGeometryCollection tmpGC=tmpLine as IGeometryCollection;
									tmpPath=tmpGC.get_Geometry(0) as IPath;
									oo=Type.Missing;
									newGC.AddGeometry((IGeometry)tmpPath,ref oo,ref oo);
								} //for(int j2=0;
				
								pBuffer.set_Value(nShapePos,(object)(newLine));
								bNeedProcess=true;
							} 
							if(selGeometryType==esriGeometryType.esriGeometryPolygon)
							{
								IPolygon curPoly=oo as IPolygon;
								IGeometryCollection curGC=curPoly as IGeometryCollection;
								IPolygon newPoly=new PolygonClass();
								IGeometryCollection newGC=newPoly as IGeometryCollection;
								nGS=curGC.GeometryCount;
								for(int j2=0;j2<nGS;j2++) 
								{
									IRing tmpRing=curGC.get_Geometry(j2) as IRing;
									IPointCollection tmpPC=tmpRing as IPointCollection;

									IPolygon tmpPoly=new PolygonClass();
									IPointCollection tmpPC2=tmpPoly as IPointCollection;
									for(int k=0;k<tmpPC.PointCount;k++) 
									{
										IPoint pp=tmpPC.get_Point(k);

										PointClass curP=new PointClass();
										curP.X=pp.X;
										curP.Y=pp.Y;
										PointClass newP=null;
										GetNewXY(curP,out newP);

										object o1=Type.Missing;
										object o2=Type.Missing;
										tmpPC2.AddPoint((IPoint)newP,ref o1,ref o2);
									}

									IGeometryCollection tmpGC=tmpPoly as IGeometryCollection;
									tmpRing=tmpGC.get_Geometry(0) as IRing;
									oo=Type.Missing;
									newGC.AddGeometry((IGeometry)tmpRing,ref oo,ref oo);
								} //for(int j2=0;
				
								pBuffer.set_Value(nShapePos,(object)(newPoly));
								bNeedProcess=true;
							} 
						}
					} //for(int j=0;i<...
					if(bNeedProcess==true) 
					{
						pWSE2.StartEditOperation();
						int nNewOID=(int)pCursor.InsertRow(pBuffer);
						pWSE2.StopEditOperation();
						System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
					}
					
					progressBar1.Value=nTipsGS++;
					pCurRow=pRows.NextRow();
				} //while(pRow!=null)
				System.Runtime.InteropServices.Marshal.ReleaseComObject(pRows);
				pWSE2.StopEditing(true);
				Tips.Visible=false;

				this.Cursor=System.Windows.Forms.Cursors.Default;
				MessageBox.Show("translate OK!");
				//... ...
			} 
			catch(Exception E) 
			{
				this.Cursor=System.Windows.Forms.Cursors.Default;
				MessageBox.Show(E.Message);
				return;
			}
			this.Cursor=System.Windows.Forms.Cursors.Default;	
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            //��:
			if(radioButton2.Checked==true) 
			{
				try 
				{
					this.Cursor=System.Windows.Forms.Cursors.WaitCursor;
					listBox1.Items.Clear();

					//�õ���ǰMap��ͼ����õ�listBox1:
					IMap myMap=m_MapControl.ActiveView.FocusMap;
					for(int i=0;i<myMap.LayerCount;i++) 
					{
						ILayer lyr=myMap.get_Layer(i);
                        if (lyr is IFeatureLayer)
                        {
                            IFeatureLayer flyr = lyr as IFeatureLayer;
                            string sName = lyr.Name.Trim();
                            if (flyr.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolyline)
                                listBox1.Items.Add(sName);
                        }
                        else if (lyr is IGroupLayer)
                        {
                            ICompositeLayer pCLyr = lyr as ICompositeLayer;
                            for (int j = 0; j < pCLyr.Count; j++)
                            {
                                IFeatureLayer flyr = pCLyr.get_Layer(j) as IFeatureLayer;
                                if (flyr == null) continue;
                                string sName = flyr.Name.Trim();
                                if (flyr.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolyline)
                                    listBox1.Items.Add(sName);
                            }
                        }
                       
					}

					this.Cursor=System.Windows.Forms.Cursors.Default;
					textBox2.Text="CorTranslate_Polyline.shp";
				} 
				catch(Exception E) 
				{
					this.Cursor=System.Windows.Forms.Cursors.Default;
					MessageBox.Show(E.Message);
					return;
				}
			}	
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            //��:
			if(radioButton3.Checked==true) 
			{
				try 
				{
					this.Cursor=System.Windows.Forms.Cursors.WaitCursor;
					listBox1.Items.Clear();

					//�õ���ǰMap��ͼ����õ�listBox1:
					IMap myMap=m_MapControl.ActiveView.FocusMap;
					for(int i=0;i<myMap.LayerCount;i++) 
					{
						ILayer lyr=myMap.get_Layer(i);

                        if (lyr is IFeatureLayer)
                        {
                            IFeatureLayer flyr = lyr as IFeatureLayer;
                            string sName = lyr.Name.Trim();
                            if (flyr.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolyline)
                                listBox1.Items.Add(sName);
                        }
                        else if (lyr is IGroupLayer)
                        {
                            ICompositeLayer pCLyr = lyr as ICompositeLayer;
                            for (int j = 0; j < pCLyr.Count; j++)
                            {
                                IFeatureLayer flyr = pCLyr.get_Layer(j) as IFeatureLayer;
                                if (flyr == null) continue;
                                string sName = flyr.Name.Trim();
                                if (flyr.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolyline)
                                    listBox1.Items.Add(sName);
                            }
                        }


                       
					}

					this.Cursor=System.Windows.Forms.Cursors.Default;
					textBox2.Text="CorTranslate_Polygon.shp";
				} 
				catch(Exception E) 
				{
					this.Cursor=System.Windows.Forms.Cursors.Default;
					MessageBox.Show(E.Message);
					return;
				}
			}		
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if(folderBrowserDialog1.ShowDialog()==DialogResult.OK) 
			{
				textBox1.Text=folderBrowserDialog1.SelectedPath;
			}
        }

        
    }
}