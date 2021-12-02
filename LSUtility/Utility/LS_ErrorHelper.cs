using System;
using System.Collections.Generic;
using System.Text;

namespace RCIS.Utility
{
    public class LS_ErrorHelper
    {
        public static void ShowErrorForm(Exception ex, string sErr)
        {
            LS_FormError frmError = new LS_FormError();
            frmError.Exception = ex;
            frmError.Error = sErr;
            frmError.ShowDialog();
        }
    }
}
