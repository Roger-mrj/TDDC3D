using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RCIS.Utility
{
    public partial class LS_FormError : Form
    {

        public LS_FormError()
        {
            InitializeComponent();
        }

        public LS_FormError(Exception ex)
        {
            InitializeComponent();
            this.Exception = ex;
        }
        #region Members

        Exception m_ex;
        string m_sErr = "";

        #endregion

        #region Properties

        public Exception Exception
        {
            set
            {
                m_ex = value;
            }
            get
            {
                return m_ex;
            }
        }
        public string Error
        {
            set
            {
                m_sErr = value;
            }
            get
            {
                return m_sErr;
            }
        }

        #endregion

        private void buttonOkay_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void FormError_Load(object sender, EventArgs e)
        {
            this.LoadForm(m_sErr, m_ex);
        }

        private void LoadForm(string sErr, Exception ex)
        {
            string sError = "";
            if (ex == null)
            {
                if (sErr == "")
                {
                    sError = "地星系统友情提示：\r\n错误原因可能为：\r\n" +
                       "1：用户误操作；\r\n" +
                       "2：数据不规范；\r\n" +
                       "3：系统漏洞；\r\n" +
                       "\r\n请用户复制此错误，填写详细的操作步骤，发送给地星公司，本公司会尽快解决，谢谢合作！";
                }
                else
                {
                    sError = "地星系统友情提示:\r\n" +
                       "错误原因可能为:" + sErr.ToString() + "\r\n" +
                       "\r\n请用户复制此错误，填写详细的操作步骤，发送给地星公司，本公司会尽快解决，谢谢合作！";
                }
            }
            else
            {
                if (sErr == "")
                {
                    sError = "地星管理系统友情提示：\r\n错误原因可能为：\r\n" +
                       "1：用户误操作；\r\n" +
                       "2：数据不规范；\r\n" +
                       "3：系统漏洞；\r\n" +
                       "错误源：\r\n" + ex.Source.ToString() +
                       "\r\n" +
                       "\r\n错误信息：" + ex.Message.ToString() +
                       "\r\n" +
                       "\r\n错误列：" + ex.StackTrace.ToString() +
                       "\r\n" +
                       "\r\n请用户复制此错误，填写详细的操作步骤，发送给地星公司，本公司会尽快解决，谢谢合作！";
                }
                else
                {
                    sError = "地星管理系统友情提示:\r\n" +
                       "错误原因可能为:" + sErr.ToString() + "\r\n" +
                       "错误源:" + ex.Source.ToString() +
                       "\r\n" +
                       "\r\n错误信息：" + ex.Message.ToString() +
                       "\r\n" +
                       "\r\n错误列：" + ex.StackTrace.ToString() +
                       "\r\n" +
                       "\r\n请用户复制此错误，填写详细的操作步骤，发送给地星公司，本公司会尽快解决，谢谢合作！";
                }
            }
            this.textBoxErrorMessage.Text = sError;
        }
    }
}
